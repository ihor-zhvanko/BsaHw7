using System;
using NUnit.Framework;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper;

using Airport.Common.DTOs;
using Airport.Common.Exceptions;

using Airport.Data.Repositories;
using Airport.Data.UnitOfWork;
using Airport.Data.Models;

using Airport.BusinessLogic.Services;

namespace Airport.Tests.Units.Services
{
  [TestFixture]
  public class DepartureServiceTests
  {
    protected IValidator<DepartureDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<DepartureDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<DepartureDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<DepartureDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<DepartureDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<DepartureDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_Departure_with_new_id_is_returned()
    {
      // Arrange
      var departureMock = new Departure()
      {
        Id = 1,
        Date = new DateTime(2018, 10, 1),
        FlightId = 1,
        PlaneId = 1,
        CrewId = 1
      };

      var departureDTOToCreate = new DepartureDTO()
      {
        Date = new DateTime(2018, 10, 1),
        FlightId = 1,
        PlaneId = 1,
        CrewId = 1
      };

      var expectedDepartureDTO = new DepartureDTO()
      {
        Id = 1,
        Date = new DateTime(2018, 10, 1),
        FlightId = 1,
        PlaneId = 1,
        CrewId = 1
      };
      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      A.CallTo(() => departureRepositoryFake.Create(A<Departure>._)).Returns(departureMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);

      var departureService = new DepartureService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = departureService.Create(departureDTOToCreate);

      // Assert
      Assert.AreEqual(expectedDepartureDTO.Id, result.Id);
      Assert.AreEqual(expectedDepartureDTO.Date, result.Date);
      Assert.AreEqual(expectedDepartureDTO.FlightId, result.FlightId);
      Assert.AreEqual(expectedDepartureDTO.PlaneId, result.PlaneId);
      Assert.AreEqual(expectedDepartureDTO.CrewId, result.CrewId);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var departureDTOToCreate = new DepartureDTO()
      {
        Date = new DateTime(2018, 10, 1),
        FlightId = 1,
        PlaneId = 1,
        CrewId = 1
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);

      var departureService = new DepartureService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = departureService.Create(departureDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => departureRepositoryFake.Create(A<Departure>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var departureMock = new Departure()
      {
        Id = 2,
        Date = new DateTime(2018, 10, 1),
        FlightId = 2,
        PlaneId = 2,
        CrewId = 2
      };

      var departureDTOToCreate = new DepartureDTO()
      {
        Date = new DateTime(2018, 10, 1),
        FlightId = 2,
        PlaneId = 2,
        CrewId = 2
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      A.CallTo(() => departureRepositoryFake.Create(A<Departure>._)).Returns(departureMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);

      var departureService = new DepartureService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => departureService.Create(departureDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var departureDTOToCreate = new DepartureDTO()
      {
        Date = new DateTime(2018, 10, 1),
        FlightId = 2,
        PlaneId = 2,
        CrewId = 2
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);

      var departureService = new DepartureService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => departureService.Create(departureDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => departureRepositoryFake.Create(A<Departure>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.DepartureRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var departureMock = new Departure()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };

      var departureDTOToUpdate = new DepartureDTO()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };

      var expectedDepartureDTO = new DepartureDTO()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };
      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      A.CallTo(() => departureRepositoryFake.Update(A<Departure>._)).Returns(departureMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);

      var departureService = new DepartureService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = departureService.Update(departureDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedDepartureDTO.Id, result.Id, "Id");
      Assert.AreEqual(expectedDepartureDTO.PlaneId, result.PlaneId);
      Assert.AreEqual(expectedDepartureDTO.FlightId, result.FlightId);
      Assert.AreEqual(expectedDepartureDTO.Date, result.Date);
      Assert.AreEqual(expectedDepartureDTO.CrewId, result.CrewId);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var departureDTOToUpdate = new DepartureDTO()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).Returns(departureRepositoryFake);
      var departureService = new DepartureService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = departureService.Update(departureDTOToUpdate);

      // Assert
      A.CallTo(() => departureRepositoryFake.Update(A<Departure>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var departureDTOToUpdate = new DepartureDTO()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var departureService = new DepartureService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => departureService.Update(departureDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var departureDTOToUpdate = new DepartureDTO()
      {
        Id = 3,
        Date = new DateTime(2018, 10, 1),
        FlightId = 3,
        PlaneId = 3,
        CrewId = 3
      };

      var departureRepositoryFake = A.Fake<IDepartureRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var departureService = new DepartureService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => departureService.Update(departureDTOToUpdate));

      A.CallTo(() => departureRepositoryFake.Update(A<Departure>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.DepartureRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Departure>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
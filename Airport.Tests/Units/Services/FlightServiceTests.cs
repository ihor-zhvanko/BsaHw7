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
  public class FlightServiceTests
  {
    protected IValidator<FlightDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<FlightDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<FlightDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<FlightDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<FlightDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<FlightDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_flight_with_new_id_is_returned()
    {
      // Arrange
      var flightMock = new Flight
      {
        Id = 1,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightDTOToCreate = new FlightDTO
      {
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var expectedFlightDTO = new FlightDTO
      {
        Id = 1,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };
      var flightRepositoryFake = A.Fake<IFlightRepository>();
      A.CallTo(() => flightRepositoryFake.Create(A<Flight>._)).Returns(flightMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);

      var flightService = new FlightService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = flightService.Create(flightDTOToCreate);

      // Assert
      Assert.AreEqual(expectedFlightDTO.Id, result.Id);
      Assert.AreEqual(expectedFlightDTO.Number, result.Number);
      Assert.AreEqual(expectedFlightDTO.DeparturePlace, result.DeparturePlace);
      Assert.AreEqual(expectedFlightDTO.DepartureTime, result.DepartureTime);
      Assert.AreEqual(expectedFlightDTO.ArrivalPlace, result.ArrivalPlace);
      Assert.AreEqual(expectedFlightDTO.ArrivalTime, result.ArrivalTime);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var flightDTOToCreate = new FlightDTO()
      {
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);

      var flightService = new FlightService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = flightService.Create(flightDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => flightRepositoryFake.Create(A<Flight>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var flightMock = new Flight()
      {
        Id = 2,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightDTOToCreate = new FlightDTO()
      {
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();
      A.CallTo(() => flightRepositoryFake.Create(A<Flight>._)).Returns(flightMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);

      var flightService = new FlightService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => flightService.Create(flightDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var flightDTOToCreate = new FlightDTO()
      {
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);

      var flightService = new FlightService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => flightService.Create(flightDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => flightRepositoryFake.Create(A<Flight>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.FlightRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var flightMock = new Flight()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightDTOToUpdate = new FlightDTO()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var expectedFlightDTO = new FlightDTO()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };
      var flightRepositoryFake = A.Fake<IFlightRepository>();
      A.CallTo(() => flightRepositoryFake.Update(A<Flight>._)).Returns(flightMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);

      var flightService = new FlightService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = flightService.Update(flightDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedFlightDTO.Id, result.Id);
      Assert.AreEqual(expectedFlightDTO.Number, result.Number);
      Assert.AreEqual(expectedFlightDTO.DeparturePlace, result.DeparturePlace);
      Assert.AreEqual(expectedFlightDTO.DepartureTime, result.DepartureTime);
      Assert.AreEqual(expectedFlightDTO.ArrivalPlace, result.ArrivalPlace);
      Assert.AreEqual(expectedFlightDTO.ArrivalTime, result.ArrivalTime);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var flightDTOToUpdate = new FlightDTO()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).Returns(flightRepositoryFake);
      var flightService = new FlightService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = flightService.Update(flightDTOToUpdate);

      // Assert
      A.CallTo(() => flightRepositoryFake.Update(A<Flight>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var flightDTOToUpdate = new FlightDTO()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var flightService = new FlightService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => flightService.Update(flightDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var flightDTOToUpdate = new FlightDTO()
      {
        Id = 3,
        Number = "AABBCC",
        DeparturePlace = "from place",
        DepartureTime = new DateTime(1, 1, 1, 12, 0, 0),
        ArrivalPlace = "to place",
        ArrivalTime = new DateTime(1, 1, 1, 18, 0, 0)
      };

      var flightRepositoryFake = A.Fake<IFlightRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var flightService = new FlightService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => flightService.Update(flightDTOToUpdate));

      A.CallTo(() => flightRepositoryFake.Update(A<Flight>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.FlightRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Flight>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
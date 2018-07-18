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
  public class PilotServiceTests
  {
    protected IValidator<PilotDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<PilotDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<PilotDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<PilotDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<PilotDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<PilotDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_pilot_with_new_id_is_returned()
    {
      // Arrange
      var pilotMock = new Pilot()
      {
        Id = 1,
        FirstName = "Pilot1",
        LastName = "Pilot1",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotDTOToCreate = new PilotDTO()
      {
        FirstName = "Pilot1",
        LastName = "Pilot1",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var expectedPilotDTO = new PilotDTO()
      {
        Id = 1,
        FirstName = "Pilot1",
        LastName = "Pilot1",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };
      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      A.CallTo(() => pilotRepositoryFake.Create(A<Pilot>._)).Returns(pilotMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);

      var pilotService = new PilotService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = pilotService.Create(pilotDTOToCreate);

      // Assert
      Assert.AreEqual(expectedPilotDTO.Id, result.Id);
      Assert.AreEqual(expectedPilotDTO.FirstName, result.FirstName);
      Assert.AreEqual(expectedPilotDTO.LastName, result.LastName);
      Assert.AreEqual(expectedPilotDTO.BirthDate, result.BirthDate);
      Assert.AreEqual(expectedPilotDTO.Experience, result.Experience);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var pilotDTOToCreate = new PilotDTO()
      {
        FirstName = "Pilot1",
        LastName = "Pilot1",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);

      var pilotService = new PilotService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = pilotService.Create(pilotDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => pilotRepositoryFake.Create(A<Pilot>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var pilotMock = new Pilot()
      {
        Id = 2,
        FirstName = "Pilot2",
        LastName = "Pilot2",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotDTOToCreate = new PilotDTO()
      {
        FirstName = "Pilot2",
        LastName = "Pilot2",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      A.CallTo(() => pilotRepositoryFake.Create(A<Pilot>._)).Returns(pilotMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);

      var pilotService = new PilotService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => pilotService.Create(pilotDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var pilotDTOToCreate = new PilotDTO()
      {
        FirstName = "Pilot2",
        LastName = "Pilot2",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);

      var pilotService = new PilotService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => pilotService.Create(pilotDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => pilotRepositoryFake.Create(A<Pilot>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PilotRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var pilotMock = new Pilot()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotDTOToUpdate = new PilotDTO()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var expectedPilotDTO = new PilotDTO()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };
      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      A.CallTo(() => pilotRepositoryFake.Update(A<Pilot>._)).Returns(pilotMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);

      var pilotService = new PilotService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = pilotService.Update(pilotDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedPilotDTO.Id, result.Id, "Id");
      Assert.AreEqual(expectedPilotDTO.FirstName, result.FirstName);
      Assert.AreEqual(expectedPilotDTO.LastName, result.LastName);
      Assert.AreEqual(expectedPilotDTO.BirthDate, result.BirthDate);
      Assert.AreEqual(expectedPilotDTO.Experience, result.Experience);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var pilotDTOToUpdate = new PilotDTO()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).Returns(pilotRepositoryFake);
      var pilotService = new PilotService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = pilotService.Update(pilotDTOToUpdate);

      // Assert
      A.CallTo(() => pilotRepositoryFake.Update(A<Pilot>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var pilotDTOToUpdate = new PilotDTO()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var pilotService = new PilotService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => pilotService.Update(pilotDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var pilotDTOToUpdate = new PilotDTO()
      {
        Id = 3,
        FirstName = "Pilot3",
        LastName = "Pilot3",
        BirthDate = new DateTime(1970, 10, 1),
        Experience = 15
      };

      var pilotRepositoryFake = A.Fake<IPilotRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var pilotService = new PilotService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => pilotService.Update(pilotDTOToUpdate));

      A.CallTo(() => pilotRepositoryFake.Update(A<Pilot>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PilotRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Pilot>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
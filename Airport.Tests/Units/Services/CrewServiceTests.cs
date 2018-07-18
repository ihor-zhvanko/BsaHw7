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
  public class CrewServiceTests
  {
    protected IValidator<CrewDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<CrewDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<CrewDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<CrewDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<CrewDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<CrewDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_Crew_with_new_id_is_returned()
    {
      // Arrange
      var crewMock = new Crew()
      {
        Id = 1,
        PilotId = 1
      };

      var crewDTOToCreate = new CrewDTO()
      {
        PilotId = 1
      };

      var expectedCrewDTO = new CrewDTO()
      {
        Id = 1,
        PilotId = 1
      };
      var crewRepositoryFake = A.Fake<ICrewRepository>();
      A.CallTo(() => crewRepositoryFake.Create(A<Crew>._)).Returns(crewMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);

      var crewService = new CrewService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = crewService.Create(crewDTOToCreate);

      // Assert
      Assert.AreEqual(expectedCrewDTO.Id, result.Id);
      Assert.AreEqual(expectedCrewDTO.PilotId, result.PilotId);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var crewDTOToCreate = new CrewDTO()
      {
        PilotId = 1
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);

      var crewService = new CrewService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = crewService.Create(crewDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => crewRepositoryFake.Create(A<Crew>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var crewMock = new Crew()
      {
        Id = 2,
        PilotId = 2
      };

      var crewDTOToCreate = new CrewDTO()
      {
        PilotId = 2
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();
      A.CallTo(() => crewRepositoryFake.Create(A<Crew>._)).Returns(crewMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);

      var crewService = new CrewService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => crewService.Create(crewDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var crewDTOToCreate = new CrewDTO()
      {
        PilotId = 2
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);

      var crewService = new CrewService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => crewService.Create(crewDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => crewRepositoryFake.Create(A<Crew>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.CrewRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var crewMock = new Crew()
      {
        Id = 3,
        PilotId = 10
      };

      var crewDTOToUpdate = new CrewDTO()
      {
        Id = 3,
        PilotId = 2 // SERVICE.UPDATE SHOULD RETURN WHAT RETURNS REPOSITORY.UPDATE
      };

      var expectedCrewDTO = new CrewDTO()
      {
        Id = 3,
        PilotId = 10
      };
      var crewRepositoryFake = A.Fake<ICrewRepository>();
      A.CallTo(() => crewRepositoryFake.Update(A<Crew>._)).Returns(crewMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);

      var crewService = new CrewService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = crewService.Update(crewDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedCrewDTO.Id, result.Id, "Id");
      Assert.AreEqual(expectedCrewDTO.PilotId, result.PilotId);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var crewDTOToUpdate = new CrewDTO()
      {
        Id = 3,
        PilotId = 3
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).Returns(crewRepositoryFake);
      var crewService = new CrewService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = crewService.Update(crewDTOToUpdate);

      // Assert
      A.CallTo(() => crewRepositoryFake.Update(A<Crew>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var crewDTOToUpdate = new CrewDTO()
      {
        Id = 3,
        PilotId = 3
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var crewService = new CrewService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => crewService.Update(crewDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var crewDTOToUpdate = new CrewDTO()
      {
        Id = 3,
        PilotId = 3
      };

      var crewRepositoryFake = A.Fake<ICrewRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var crewService = new CrewService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => crewService.Update(crewDTOToUpdate));

      A.CallTo(() => crewRepositoryFake.Update(A<Crew>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.CrewRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Crew>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
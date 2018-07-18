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
  public class AirhostessServiceTests
  {
    protected IValidator<AirhostessDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<AirhostessDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<AirhostessDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<AirhostessDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<AirhostessDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<AirhostessDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_airhostess_with_new_id_is_returned()
    {
      // Arrange
      var airhostessMock = new Airhostess()
      {
        Id = 1,
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1,
        Crew = null
      };

      var airhostessDTOToCreate = new AirhostessDTO()
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };

      var expectedAirhostessDTO = new AirhostessDTO()
      {
        Id = 1,
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      A.CallTo(() => airhostessRepositoryFake.Create(A<Airhostess>._)).Returns(airhostessMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);

      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = airhostessService.Create(airhostessDTOToCreate);

      // Assert
      Assert.AreEqual(expectedAirhostessDTO.Id, result.Id);
      Assert.AreEqual(expectedAirhostessDTO.FirstName, result.FirstName);
      Assert.AreEqual(expectedAirhostessDTO.LastName, result.LastName);
      Assert.AreEqual(expectedAirhostessDTO.BirthDate, result.BirthDate);
      Assert.AreEqual(expectedAirhostessDTO.CrewId, result.CrewId);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var airhostessDTOToCreate = new AirhostessDTO()
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);

      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = airhostessService.Create(airhostessDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => airhostessRepositoryFake.Create(A<Airhostess>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var airhostessMock = new Airhostess()
      {
        Id = 2,
        FirstName = "Airhostess2",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 2,
        Crew = null
      };

      var airhostessDTOToCreate = new AirhostessDTO()
      {
        FirstName = "",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 2
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      A.CallTo(() => airhostessRepositoryFake.Create(A<Airhostess>._)).Returns(airhostessMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);

      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => airhostessService.Create(airhostessDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var airhostessDTOToCreate = new AirhostessDTO()
      {
        FirstName = "",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 2
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);

      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => airhostessService.Create(airhostessDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => airhostessRepositoryFake.Create(A<Airhostess>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.AirhostessRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var airhostessMock = new Airhostess()
      {
        Id = 3,
        FirstName = "Airhostess3",
        LastName = "Airhostess3",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 3,
        Crew = null
      };

      var airhostessDTOToUpdate = new AirhostessDTO()
      {
        Id = 3,
        FirstName = "Airhostess2",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };

      var expectedAirhostessDTO = new AirhostessDTO()
      {
        Id = 3,
        FirstName = "Airhostess3",
        LastName = "Airhostess3",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 3
      };
      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      A.CallTo(() => airhostessRepositoryFake.Update(A<Airhostess>._)).Returns(airhostessMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);

      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = airhostessService.Update(airhostessDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedAirhostessDTO.Id, result.Id, "Id");
      Assert.AreEqual(expectedAirhostessDTO.FirstName, result.FirstName);
      Assert.AreEqual(expectedAirhostessDTO.LastName, result.LastName);
      Assert.AreEqual(expectedAirhostessDTO.BirthDate, result.BirthDate);
      Assert.AreEqual(expectedAirhostessDTO.CrewId, result.CrewId);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var airhostessDTOToUpdate = new AirhostessDTO()
      {
        Id = 3,
        FirstName = "Airhostess2",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).Returns(airhostessRepositoryFake);
      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = airhostessService.Update(airhostessDTOToUpdate);

      // Assert
      A.CallTo(() => airhostessRepositoryFake.Update(A<Airhostess>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var airhostessDTOToUpdate = new AirhostessDTO()
      {
        Id = 3,
        FirstName = "Airhostess2",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 2
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => airhostessService.Update(airhostessDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var airhostessDTOToUpdate = new AirhostessDTO()
      {
        Id = 3,
        FirstName = "Airhostess2",
        LastName = "Airhostess2",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 2
      };

      var airhostessRepositoryFake = A.Fake<IAirhostessRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var airhostessService = new AirhostessService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => airhostessService.Update(airhostessDTOToUpdate));

      A.CallTo(() => airhostessRepositoryFake.Update(A<Airhostess>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.AirhostessRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Airhostess>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
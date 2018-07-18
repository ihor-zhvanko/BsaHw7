using System;
using System.Collections.Generic;
using NUnit.Framework;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper;

using Airport.Common.DTOs;
using Airport.Common.Exceptions;

using Airport.BusinessLogic.Services;

using Airport.Api.Controllers;

namespace Airport.Tests.Units.Controllers
{
  [TestFixture]
  public class AirhostessControllerTests
  {
    protected IValidator<AirhostessDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<AirhostessDTO> AlwaysInvalidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<AirhostessDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<AirhostessDTO>._)).Returns(validValidationResult);

      AlwaysInvalidValidator = A.Fake<IValidator<AirhostessDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInvalidValidator.Validate(A<AirhostessDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Get_When_is_called_Then_service_get_all_is_called()
    {
      // Arrange
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysValidValidator);

      // Act
      airhostessController.Get();

      // Assert
      A.CallTo(() => airhostessServiceFake.GetAll()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Get_id_When_is_called_Then_service_get_by_id_is_called()
    {
      // Arrange
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysValidValidator);

      // Act
      airhostessController.Get(1);

      // Assert
      A.CallTo(() => airhostessServiceFake.GetById(1)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Post_When_is_called_Then_service_create_is_called()
    {
      // Arrange
      var airhostessDTO = new AirhostessDTO
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysValidValidator);

      // Act
      airhostessController.Post(airhostessDTO);

      // Assert
      A.CallTo(() => airhostessServiceFake.Create(airhostessDTO)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Post_When_dto_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var airhostessDTO = new AirhostessDTO
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysInvalidValidator);

      // Act
      // Assert
      var exception = Assert.Throws<BadRequestException>(() => airhostessController.Post(airhostessDTO));

      Assert.AreEqual(exception.Message, "Is Invalid");
      A.CallTo(() => airhostessServiceFake.Create(airhostessDTO)).MustNotHaveHappened();
    }

    [Test]
    public void Put_When_is_called_Then_service_update_is_called()
    {
      // Arrange
      var airhostessId = 1;
      var airhostessDTO = new AirhostessDTO
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysValidValidator);

      // Act
      airhostessController.Put(airhostessId, airhostessDTO);

      // Assert
      airhostessDTO.Id = airhostessId;
      A.CallTo(() => airhostessServiceFake.Update(airhostessDTO)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Put_When_dto_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var airhostessId = 1;
      var airhostessInputDTO = new AirhostessDTO
      {
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessDTO = new AirhostessDTO
      {
        Id = 1,
        FirstName = "Airhostess1",
        LastName = "Airhostess1",
        BirthDate = new DateTime(1970, 10, 1),
        CrewId = 1
      };
      var airhostessServiceFake = A.Fake<IAirhostessService>();
      var airhostessController = new AirhostessesController(airhostessServiceFake, AlwaysInvalidValidator);

      // Act
      // Assert
      var exception = Assert.Throws<BadRequestException>(() =>
        airhostessController.Put(airhostessId, airhostessInputDTO)
      );

      Assert.AreEqual(exception.Message, "Is Invalid");
      A.CallTo(() => airhostessServiceFake.Update(airhostessDTO)).MustNotHaveHappened();
    }
  }
}
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
  public class PlaneTypeServiceTests
  {
    protected IValidator<PlaneTypeDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<PlaneTypeDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<PlaneTypeDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<PlaneTypeDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<PlaneTypeDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<PlaneTypeDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_plane_type_with_new_id_is_returned()
    {
      // Arrange
      var planeTypeMock = new PlaneType()
      {
        Id = 1,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeDTOToCreate = new PlaneTypeDTO()
      {
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var expectedPlaneTypeDTO = new PlaneTypeDTO()
      {
        Id = 1,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };
      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      A.CallTo(() => planeTypeRepositoryFake.Create(A<PlaneType>._)).Returns(planeTypeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);

      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeTypeService.Create(planeTypeDTOToCreate);

      // Assert
      Assert.AreEqual(expectedPlaneTypeDTO.Id, result.Id);
      Assert.AreEqual(expectedPlaneTypeDTO.Model, result.Model);
      Assert.AreEqual(expectedPlaneTypeDTO.Seats, result.Seats);
      Assert.AreEqual(expectedPlaneTypeDTO.Carrying, result.Carrying);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeTypeDTOToCreate = new PlaneTypeDTO()
      {
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);

      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeTypeService.Create(planeTypeDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => planeTypeRepositoryFake.Create(A<PlaneType>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var planeTypeMock = new PlaneType()
      {
        Id = 2,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeDTOToCreate = new PlaneTypeDTO()
      {
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      A.CallTo(() => planeTypeRepositoryFake.Create(A<PlaneType>._)).Returns(planeTypeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);

      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeTypeService.Create(planeTypeDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeTypeDTOToCreate = new PlaneTypeDTO()
      {
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);

      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeTypeService.Create(planeTypeDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => planeTypeRepositoryFake.Create(A<PlaneType>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PlaneTypeRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var planeTypeMock = new PlaneType()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeDTOToUpdate = new PlaneTypeDTO()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var expectedPlaneTypeDTO = new PlaneTypeDTO()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };
      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      A.CallTo(() => planeTypeRepositoryFake.Update(A<PlaneType>._)).Returns(planeTypeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);

      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeTypeService.Update(planeTypeDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedPlaneTypeDTO.Id, result.Id);
      Assert.AreEqual(expectedPlaneTypeDTO.Model, result.Model);
      Assert.AreEqual(expectedPlaneTypeDTO.Seats, result.Seats);
      Assert.AreEqual(expectedPlaneTypeDTO.Carrying, result.Carrying);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeTypeDTOToUpdate = new PlaneTypeDTO()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).Returns(planeTypeRepositoryFake);
      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeTypeService.Update(planeTypeDTOToUpdate);

      // Assert
      A.CallTo(() => planeTypeRepositoryFake.Update(A<PlaneType>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var planeTypeDTOToUpdate = new PlaneTypeDTO()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeTypeService.Update(planeTypeDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeTypeDTOToUpdate = new PlaneTypeDTO()
      {
        Id = 3,
        Model = "AAABBBCCC",
        Seats = 500,
        Carrying = 400
      };

      var planeTypeRepositoryFake = A.Fake<IPlaneTypeRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var planeTypeService = new PlaneTypeService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeTypeService.Update(planeTypeDTOToUpdate));

      A.CallTo(() => planeTypeRepositoryFake.Update(A<PlaneType>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PlaneTypeRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<PlaneType>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
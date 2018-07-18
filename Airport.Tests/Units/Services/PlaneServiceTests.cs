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
  public class PlaneServiceTests
  {
    protected IValidator<PlaneDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<PlaneDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<PlaneDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<PlaneDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<PlaneDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<PlaneDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_plane_with_new_id_is_returned()
    {
      // Arrange
      var planeMock = new Plane()
      {
        Id = 1,
        Name = "Mock Plane 1",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeDTOToCreate = new PlaneDTO()
      {
        Name = "Mock Plane 1",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var expectedPlaneDTO = new PlaneDTO()
      {
        Id = 1,
        Name = "Mock Plane 1",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };
      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      A.CallTo(() => planeRepositoryFake.Create(A<Plane>._)).Returns(planeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);

      var planeService = new PlaneService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeService.Create(planeDTOToCreate);

      // Assert
      Assert.AreEqual(expectedPlaneDTO.Id, result.Id);
      Assert.AreEqual(expectedPlaneDTO.Name, result.Name);
      Assert.AreEqual(expectedPlaneDTO.ReleaseDate, result.ReleaseDate);
      Assert.AreEqual(expectedPlaneDTO.PlaneTypeId, result.PlaneTypeId);
      Assert.AreEqual(expectedPlaneDTO.ServiceLife, result.ServiceLife);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeDTOToCreate = new PlaneDTO()
      {
        Name = "Mock Plane 1",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);

      var planeService = new PlaneService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeService.Create(planeDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => planeRepositoryFake.Create(A<Plane>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var planeMock = new Plane()
      {
        Id = 2,
        Name = "Mock Plane 2",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeDTOToCreate = new PlaneDTO()
      {
        Name = "Mock Plane 2",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 1,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      A.CallTo(() => planeRepositoryFake.Create(A<Plane>._)).Returns(planeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);

      var planeService = new PlaneService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeService.Create(planeDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeDTOToCreate = new PlaneDTO()
      {
        Name = "Mock Plane 2",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 2,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);

      var planeService = new PlaneService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeService.Create(planeDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => planeRepositoryFake.Create(A<Plane>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PlaneRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var planeMock = new Plane()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeDTOToUpdate = new PlaneDTO()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var expectedPlaneDTO = new PlaneDTO()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };
      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      A.CallTo(() => planeRepositoryFake.Update(A<Plane>._)).Returns(planeMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);

      var planeService = new PlaneService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeService.Update(planeDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedPlaneDTO.Id, result.Id);
      Assert.AreEqual(expectedPlaneDTO.Name, result.Name);
      Assert.AreEqual(expectedPlaneDTO.ReleaseDate, result.ReleaseDate);
      Assert.AreEqual(expectedPlaneDTO.PlaneTypeId, result.PlaneTypeId);
      Assert.AreEqual(expectedPlaneDTO.ServiceLife, result.ServiceLife);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeDTOToUpdate = new PlaneDTO()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).Returns(planeRepositoryFake);
      var planeService = new PlaneService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = planeService.Update(planeDTOToUpdate);

      // Assert
      A.CallTo(() => planeRepositoryFake.Update(A<Plane>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var planeDTOToUpdate = new PlaneDTO()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var planeService = new PlaneService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeService.Update(planeDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var planeDTOToUpdate = new PlaneDTO()
      {
        Id = 3,
        Name = "Mock Plane 3",
        ReleaseDate = new DateTime(1950, 10, 10),
        PlaneTypeId = 3,
        ServiceLife = new TimeSpan(10_000 * 24, 0, 0)
      };

      var planeRepositoryFake = A.Fake<IPlaneRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var planeService = new PlaneService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => planeService.Update(planeDTOToUpdate));

      A.CallTo(() => planeRepositoryFake.Update(A<Plane>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.PlaneRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Plane>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
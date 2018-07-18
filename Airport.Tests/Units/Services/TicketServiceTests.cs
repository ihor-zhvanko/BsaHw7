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
  public class TicketServiceTests
  {
    protected IValidator<TicketDTO> AlwaysValidValidator { get; private set; }
    protected IValidator<TicketDTO> AlwaysInValidValidator { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<TicketDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<TicketDTO>._)).Returns(validValidationResult);

      AlwaysInValidValidator = A.Fake<IValidator<TicketDTO>>();
      var validationFailure = new ValidationFailure("Property", "Is Invalid");
      var invalidValidationResult = new ValidationResult(new[] { validationFailure });
      A.CallTo(() => AlwaysInValidValidator.Validate(A<TicketDTO>._)).Returns(invalidValidationResult);
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_plane_type_with_new_id_is_returned()
    {
      // Arrange
      var ticketMock = new Ticket()
      {
        Id = 1,
        Price = 10.23,
        FlightId = 1
      };

      var ticketDTOToCreate = new TicketDTO()
      {
        Price = 10.23,
        FlightId = 1
      };

      var expectedTicketDTO = new TicketDTO()
      {
        Id = 1,
        Price = 10.23,
        FlightId = 1
      };
      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      A.CallTo(() => ticketRepositoryFake.Create(A<Ticket>._)).Returns(ticketMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);

      var ticketService = new TicketService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = ticketService.Create(ticketDTOToCreate);

      // Assert
      Assert.AreEqual(expectedTicketDTO.Id, result.Id);
      Assert.AreEqual(expectedTicketDTO.Price, result.Price);
      Assert.AreEqual(expectedTicketDTO.FlightId, result.FlightId);
    }

    [Test] // behaviour test
    public void Create_When_entity_is_created_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var ticketDTOToCreate = new TicketDTO()
      {
        Price = 10.23,
        FlightId = 1
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);

      var ticketService = new TicketService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = ticketService.Create(ticketDTOToCreate);

      // Assert. Just behaviour
      A.CallTo(() => ticketRepositoryFake.Create(A<Ticket>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var ticketMock = new Ticket()
      {
        Id = 2,
        Price = 10.23,
        FlightId = 2
      };

      var ticketDTOToCreate = new TicketDTO()
      {
        Price = 10.23,
        FlightId = 2
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      A.CallTo(() => ticketRepositoryFake.Create(A<Ticket>._)).Returns(ticketMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);

      var ticketService = new TicketService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => ticketService.Create(ticketDTOToCreate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] // behaviour test
    public void Create_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var ticketDTOToCreate = new TicketDTO()
      {
        Price = 10.23,
        FlightId = 2
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);

      var ticketService = new TicketService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => ticketService.Create(ticketDTOToCreate));

      // Assert. Just behaviour
      A.CallTo(() => ticketRepositoryFake.Create(A<Ticket>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.TicketRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }

    [Test]
    public void Update_When_entity_is_updated_Then_updated_entity_is_returned()
    {
      // Arrange
      var ticketMock = new Ticket()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };

      var ticketDTOToUpdate = new TicketDTO()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };

      var expectedTicketDTO = new TicketDTO()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };
      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      A.CallTo(() => ticketRepositoryFake.Update(A<Ticket>._)).Returns(ticketMock);

      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);

      var ticketService = new TicketService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = ticketService.Update(ticketDTOToUpdate);

      // Assert
      Assert.AreEqual(expectedTicketDTO.Id, result.Id);
      Assert.AreEqual(expectedTicketDTO.Price, result.Price);
      Assert.AreEqual(expectedTicketDTO.FlightId, result.FlightId);
    }

    [Test] // behaviour
    public void Update_When_entity_is_updated_Then_it_makes_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var ticketDTOToUpdate = new TicketDTO()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).Returns(ticketRepositoryFake);
      var ticketService = new TicketService(unitOfWorkFake, AlwaysValidValidator);

      // Act
      var result = ticketService.Update(ticketDTOToUpdate);

      // Assert
      A.CallTo(() => ticketRepositoryFake.Update(A<Ticket>._)).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).MustHaveHappenedOnceExactly();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Update_When_entity_is_invalid_Then_bad_request_exception_is_thrown()
    {
      // Arrange
      var ticketDTOToUpdate = new TicketDTO()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var ticketService = new TicketService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => ticketService.Update(ticketDTOToUpdate), "");

      Assert.AreEqual(exception.Message, "Is Invalid");
    }

    [Test] //behavior test
    public void Update_When_entity_is_invalid_Then_it_makes_no_calls_to_repository_and_unit_of_work()
    {
      // Arrange
      var ticketDTOToUpdate = new TicketDTO()
      {
        Id = 3,
        Price = 10.23,
        FlightId = 3
      };

      var ticketRepositoryFake = A.Fake<ITicketRepository>();
      var unitOfWorkFake = A.Fake<IUnitOfWork>();
      var ticketService = new TicketService(unitOfWorkFake, AlwaysInValidValidator);

      // Act + Assert
      var exception = Assert.Throws<BadRequestException>(() => ticketService.Update(ticketDTOToUpdate));

      A.CallTo(() => ticketRepositoryFake.Update(A<Ticket>._)).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.TicketRepository).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.Set<Ticket>()).MustNotHaveHappened();
      A.CallTo(() => unitOfWorkFake.SaveChanges()).MustNotHaveHappened();
    }
  }
}
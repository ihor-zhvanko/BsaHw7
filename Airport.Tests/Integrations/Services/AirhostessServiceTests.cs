using System;
using System.Linq;
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
using Airport.Data.DatabaseContext;

using Airport.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Airport.Tests.Integrations.Services
{
  [TestFixture]
  public class AirhostessServiceTests
  {
    protected IValidator<AirhostessDTO> AlwaysValidValidator { get; private set; }

    protected AirportDbContext AirportDbContext { get; private set; }

    [SetUp]
    public void Setup()
    {
      AlwaysValidValidator = A.Fake<IValidator<AirhostessDTO>>();
      var validValidationResult = new ValidationResult();
      A.CallTo(() => AlwaysValidValidator.Validate(A<AirhostessDTO>._)).Returns(validValidationResult);

      ServicesTestsSetup.AirportInitializer.Seed().Wait();
    }

    [TearDown]
    public void TearDown()
    {
      ServicesTestsSetup.AirportInitializer.AntiSeed().Wait();
    }

    [Test]
    public void Create_When_entity_is_created_Then_new_airhostess_with_new_id_is_returned()
    {
      // Arrange
      var entities = ServicesTestsSetup.AirportDbContext.Airhostess.ToList();
    }
  }
}
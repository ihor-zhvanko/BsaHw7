using System;
using System.IO;
using NUnit.Framework;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Airport.Common.Mappers;
using Airport.Data.UnitOfWork;
using Airport.Data.Repositories;
using Airport.Data.DatabaseContext;
using Airport.Data.AirportInitializer;
using Airport.Data.MockData;

namespace Airport.Tests.Integrations.Services
{
  [SetUpFixture]
  public class ServicesTestsSetup
  {
    public static IUnitOfWork UnitOfWork { get; private set; }
    public static AirportDbContext AirportDbContext { get; private set; }
    public static AirportInitializer AirportInitializer { get; private set; }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
      MapperConfig.InitMappers();

      var options = new DbContextOptionsBuilder(new DbContextOptions<AirportDbContext>());
      options.UseSqlServer("Server=.\\SQLEXPRESS;Database=AirportDevelopmentTests;Trusted_Connection=True;", b => b.MigrationsAssembly("Airport.Data"));
      var airportDbContext = new AirportDbContext(options.Options as DbContextOptions<AirportDbContext>);
      var airhostessRepository = new AirhostessRepository(airportDbContext);
      var crewRepository = new CrewRepository(airportDbContext);
      var departureRepository = new DepartureRepository(airportDbContext);
      var flightRepository = new FlightRepository(airportDbContext);
      var pilotRepository = new PilotRepository(airportDbContext);
      var planeRepository = new PlaneRepository(airportDbContext);
      var planeTypeRepository = new PlaneTypeRepository(airportDbContext);
      var ticketRepository = new TicketRepository(airportDbContext);

      UnitOfWork = new UnitOfWork(
        airportDbContext,
        airhostessRepository,
        crewRepository,
        departureRepository,
        flightRepository,
        pilotRepository,
        planeRepository,
        planeTypeRepository,
        ticketRepository
      );
      AirportDbContext = airportDbContext;
      var directory = Directory.GetCurrentDirectory();
      var parent = Directory.GetParent(directory);
      var subDirs = Directory.GetDirectories(parent.FullName);

      AirportInitializer = new AirportInitializer(airportDbContext, new DataSource());
      AirportDbContext.Database.Migrate();
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
      UnitOfWork.Dispose();
      Mapper.Reset();
    }
  }
}
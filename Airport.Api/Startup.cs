using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Airport.Common.DTOs;
using Airport.Common.InputModels;
using Airport.Common.Mappers;
using Airport.Common.Validation;

using Airport.Data.MockData;
using Airport.Data.UnitOfWork;
using Airport.Data.DatabaseContext;
using Airport.Data.AirportInitializer;
using Airport.Data.Repositories;

using Airport.BusinessLogic.Services;

using Airport.Api.Middleware;

namespace Airport.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      MapperConfig.InitMappers();

      services.AddSingleton<DataSource>();

      string connection = Configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<AirportDbContext>(options =>
        options.UseSqlServer(connection, b => b.MigrationsAssembly("Airport.Data"))
      , ServiceLifetime.Transient);

      services.AddTransient<AirportInitializer>();

      services.AddSingleton<IValidator<AirhostessDTO>, AirhostessDTOValidator>();
      services.AddSingleton<IValidator<CrewInputModel>, CrewInputModelValidator>();
      services.AddSingleton<IValidator<DepartureDTO>, DepartureDTOValidator>();
      services.AddSingleton<IValidator<FlightDTO>, FlightDTOValidator>();
      services.AddSingleton<IValidator<PilotDTO>, PilotDTOValidator>();
      services.AddSingleton<IValidator<PlaneDTO>, PlaneDTOValidator>();
      services.AddSingleton<IValidator<PlaneTypeDTO>, PlaneTypeDTOValidator>();
      services.AddSingleton<IValidator<TicketDTO>, TicketDTOValidator>();

      services.AddScoped<IAirhostessRepository, AirhostessRepository>();
      services.AddScoped<ICrewRepository, CrewRepository>();
      services.AddScoped<IDepartureRepository, DepartureRepository>();
      services.AddScoped<IFlightRepository, FlightRepository>();
      services.AddScoped<IPilotRepository, PilotRepository>();
      services.AddScoped<IPlaneRepository, PlaneRepository>();
      services.AddScoped<IPlaneTypeRepository, PlaneTypeRepository>();
      services.AddScoped<ITicketRepository, TicketRepository>();

      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.AddScoped<IAirhostessService, AirhostessService>();
      services.AddScoped<ICrewService, CrewService>();
      services.AddScoped<IDepartureService, DepartureService>();
      services.AddScoped<IFlightService, FlightService>();
      services.AddScoped<IPilotService, PilotService>();
      services.AddScoped<IPlaneService, PlaneService>();
      services.AddScoped<IPlaneTypeService, PlaneTypeService>();
      services.AddScoped<ITicketService, TicketService>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, AirportInitializer airportInitializer)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseErrorHandlingMiddleware();
      app.UseMvc();

      airportInitializer.Seed().Wait();
    }
  }
}

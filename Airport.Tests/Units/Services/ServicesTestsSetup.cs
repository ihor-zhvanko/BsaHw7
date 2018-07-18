using System;
using NUnit.Framework;
using AutoMapper;

using Airport.Common.Mappers;

namespace Airport.Tests.Units.Services
{
  [SetUpFixture]
  public class ServicesTestsSetup
  {
    [OneTimeSetUp]
    public void GlobalSetup()
    {
      MapperConfig.InitMappers();
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
      Mapper.Reset();
    }
  }
}
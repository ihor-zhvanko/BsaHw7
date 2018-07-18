using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation.Results;

using Airport.Common.Helpers;

namespace Airport.Common.Exceptions
{
  public class NotFoundException : Exception
  {
    public NotFoundException() : base() { }
    public NotFoundException(string msg) : base(msg) { }
    public NotFoundException(string msg, Exception inner) : base(msg, inner) { }
  }
}
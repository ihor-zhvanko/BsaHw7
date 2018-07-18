using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation.Results;

using Airport.Common.Helpers;

namespace Airport.Common.Exceptions
{
  public class BadRequestException : Exception
  {
    public BadRequestException() : base() { }
    public BadRequestException(string msg) : base(msg) { }
    public BadRequestException(string msg, Exception inner) : base(msg, inner) { }
    public override string Message { get; }
    public BadRequestException(IList<ValidationFailure> errors) : base()
    {
      var stringList = errors.Select(x => x.ErrorMessage).ToList();
      Message = stringList.JoinString(Environment.NewLine);
    }
  }
}
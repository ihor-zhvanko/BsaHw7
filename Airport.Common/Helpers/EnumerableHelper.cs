using System;
using System.Linq;
using System.Collections.Generic;

namespace Airport.Common.Helpers
{
  public static class EnumerableHelper
  {
    public static string JoinString<T>(this IList<T> list, string separator)
    {
      if (list.Count == 0) return "";

      var joined = list.First().ToString();
      list.Skip(1).Select(x => joined + separator + x.ToString());

      return joined;
    }
  }
}
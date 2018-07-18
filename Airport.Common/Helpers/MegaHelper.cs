using System;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Airport.Common.Helpers
{
  public static class MegaDelayHelper
  {
    public static Task<TReturn> DoFakeDelayUsingTimer<TReturn>(
      double delaySeconds,
      Func<TReturn> todo
    )
    {
      var timeCompletionSource = new TaskCompletionSource<TReturn>();
      var delay = delaySeconds * 1000;

      var timer = new Timer();

      timer.Elapsed += (x, y) =>
      {
        try
        {
          var result = todo();

          timeCompletionSource.SetResult(result);
        }
        catch (Exception e)
        {
          timeCompletionSource.SetException(e);
        }
      };

      return timeCompletionSource.Task;
    }
  }
}
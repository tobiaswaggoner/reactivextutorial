using System.Reactive.Linq;

// ReSharper disable UnusedType.Global

// Is this still elegant or already confusing?
// Attention: With "GroupBy" we create a Observable of GroupedObservables, which means, the "groups" (one per ErrorClass) are distinct streams in their own right
// only by "flatMap"ping (aka .SelectMany) we take the output of all these streams and push them again into one single stream.
// but before this happens, we have one stream for "BIGPROBLEMs" and another one for "WARNING"s which we could handle differently if we wanted to.

namespace RxTutorial.Lessons.L2_ConsumingSequences;

public class L24Aggregation : ILesson
{
    public string Name => "Publish warning messages";

    public IDisposable ExecuteLesson()
    {
        var rnd = new Random();

        var timer = Observable.Timer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) )
                              .Select( i => rnd.NextInt64( 1, 100 ) )
                              // ###### NEW CODE START
                              .Select( GetErrorClass )
                              .Where( errorClass => errorClass != "OK" )
                              .GroupBy( errorClass => errorClass )
                              .SelectMany( CountErrorsPerClass )
                              .Where( monitor => monitor.ErrorClass == "BIGPROBLEM" && monitor.ErrorCount > 5 || monitor.ErrorClass == "WARNING" && monitor.ErrorCount > 11 );
        // ###### NEW CODE END

        return timer.Subscribe( i => Console.WriteLine( $"{i.ErrorClass}: {i.ErrorCount} " ) );
    }

    private record ErrorsPerClass( string ErrorClass, long ErrorCount );

    private static IObservable<ErrorsPerClass> CountErrorsPerClass( IGroupedObservable<string, string> group ) =>
            group.Buffer( TimeSpan.FromSeconds( 2 ) )
                 .Select( buffer =>
                                  new ErrorsPerClass
                                          (
                                           ErrorClass : group.Key,
                                           ErrorCount : buffer.Count
                                          ) );

    private static string GetErrorClass( long errorCount ) =>
            errorCount switch
            {
                    < 11 => "OK",
                    > 10 and < 71 => "WARNING",
                    _ => "BIGPROBLEM"
            };
}
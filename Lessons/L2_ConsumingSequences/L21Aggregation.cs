using System.Reactive.Linq;

// ReSharper disable UnusedType.Global

// Basically 2 LOC. Cool, right? Try this with an imperative style.

namespace RxTutorial.Lessons.L2_ConsumingSequences;

public class L21Aggregation : ILesson
{
    public string Name => "Calculate the Average of the last 3 builds and publish again";

    public IDisposable ExecuteLesson()
    {
        var rnd = new Random();
        var timer = Observable.Timer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) )
                              .Select( i => rnd.NextInt64( 1, 100 ) )
// ###### NEW CODE START                               
                              .Buffer( 3 )
                              .Select( DumpBufferForClarity() )
                              .Select( buffer => (long) buffer.Average() );
// ###### NEW CODE END                               
        return timer.Subscribe( i => Console.WriteLine( $"The Rx.Net timer ticked: {i}" ) );
    }

    private static Func<IList<long>, IList<long>> DumpBufferForClarity()
    {
        return buffer =>
        {
            Console.WriteLine( $"Buffered ${buffer.Count} values: {string.Join( ',', buffer.Select( l => l.ToString() ) )}" );
            return buffer;
        };
    }
}
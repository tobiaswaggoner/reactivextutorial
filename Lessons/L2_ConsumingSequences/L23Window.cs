using System.Reactive.Linq;

// ReSharper disable UnusedType.Global

// 0 new LOC. Just take another overload of "Buffer" to pack them by Time.

namespace RxTutorial.Lessons.L2_ConsumingSequences;

public class L23Window : ILesson
{
    public string Name => "Calculate the Average of the all build within the last 2 seconds";

    public IDisposable ExecuteLesson()
    {
        var rnd = new Random();
        var timer = Observable.Timer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) )
                              .Select( i => rnd.NextInt64( 1, 100 ) )
                              // ###### NEW CODE START                               
                              .Buffer( TimeSpan.FromSeconds( 2 ) )
                              // ###### NEW CODE END                               
                              .Select( DumpBufferForClarity() )
                              .Select( buffer => (long)buffer.Average() );
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
using System.Reactive.Linq;

// ReSharper disable UnusedType.Global

// 1 LOC. Do you also see all the "myLastReceiveDate", "myFirstReceiveDate", Dictionary<DateTime, long> variables in an imperative approach?

namespace RxTutorial.Lessons.L2_ConsumingSequences;

public class L22TSampling : ILesson
{
    public string Name => "Calculate the Average of the last 3 builds but publish only once every 2 seconds";

    public IDisposable ExecuteLesson()
    {
        var rnd = new Random();
        var timer = Observable.Timer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) )
                              .Select( i => rnd.NextInt64( 1, 100 ) )
                              .Buffer( 3 )
                              .Sample( TimeSpan.FromSeconds( 2 ) )
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
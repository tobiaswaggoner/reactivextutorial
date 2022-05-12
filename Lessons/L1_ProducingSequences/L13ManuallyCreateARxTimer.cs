using RxTutorial.Lessons.Timers;

namespace RxTutorial.Lessons.L1_ProducingSequences;

// ReSharper disable UnusedType.Global

// If there would not have been a ready implementation in System.Reactive.
// How would one implement the timer as an Observable "from scratch".
// Do a Comparison between MyOwnTimer and RxTimer --> not that much, right?

public class L13ManuallyCreateARxTimer : ILesson
{
    public string Name => "Manually create a timer as an IObservable";

    public IDisposable ExecuteLesson()
    {
        var timer = new RxTimer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) );
        return timer.Subscribe( i => Console.WriteLine( $"My self written observable timer event ticked: {i}") );
    }
}
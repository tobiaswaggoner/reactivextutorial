using RxTutorial.Lessons.Timers;

namespace RxTutorial.Lessons.L1_ProducingSequences;

// ReSharper disable UnusedType.Global

// Lets start here: A simple timer with an "OnTick" event, running on a background task
// Don't forget to make it Disposable because else the task will run indefinitely.

public class L11ManuallyCreateATimer : ILesson
{
    public string Name => "Manually create a simple timer";

    public IDisposable ExecuteLesson()
    {
        var ownTimer = new MyOwnTimer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) );
        ownTimer.OnTick += ( _, i ) => Console.WriteLine( $"My own timer event ticked: {i}" );
        ownTimer.Start();
        return ownTimer;
    }
}
using System.Reactive.Disposables;
using System.Reactive.Linq;

using RxTutorial.Lessons.Timers;

namespace RxTutorial.Lessons.L1_ProducingSequences;

// ReSharper disable UnusedType.Global

// Just for completeness sake. There are a myriad ways to create observables
// from Tasks, from Single values, from IEnumerations, ...
// One is from a .NET Event. So we can jus "convert" or own Timer to an observable like so:

public class L14ConvertTimerToObservable : ILesson
{
    public string Name => "Convert manual timer to observable";

    public IDisposable ExecuteLesson()
    {
        var timer = new MyOwnTimer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) );
        var observable = Observable.FromEventPattern<long>( h => timer.OnTick += h, h => timer.OnTick -= h );
        var subscription = observable.Subscribe( evt =>   Console.WriteLine($"The converted Rx.Net timer ticked: {evt.EventArgs}") );
        timer.Start();

        return Disposable.Create( () =>
        {
            timer.Dispose();
            subscription.Dispose();
        } );
    }
}
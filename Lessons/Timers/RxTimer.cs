using System.Reactive.Disposables;

namespace RxTutorial.Lessons.Timers;

public class RxTimer : IObservable<long>
{
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _period;
    private bool _canceled;

    public RxTimer( TimeSpan dueTime, TimeSpan period )
    {
        _dueTime = dueTime;
        _period = period;
    }

    // The "Subscribe" function:
    public IDisposable Subscribe( IObserver<long> observer )
    {
        Task.Run( () => MyTimerFunction( observer, _dueTime, _period ) );

        return Disposable.Create( () =>
        {
            _canceled = true;
        } );
    }

    private void MyTimerFunction( IObserver<long> observer, TimeSpan dueTime, TimeSpan period )
    {
        try
        {
            var rnd = new Random();
            Thread.Sleep( dueTime );
            while( _canceled == false )
            {
                observer.OnNext( rnd.NextInt64(1,100) );
                Thread.Sleep( period );
            }

            observer.OnCompleted();
        }
        catch( Exception exception )
        {
            observer.OnError( exception );
        }
    }
}
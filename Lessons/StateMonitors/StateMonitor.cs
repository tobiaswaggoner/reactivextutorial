using System.Reactive.Disposables;

namespace RxTutorial.Lessons.StateMonitors;

public class StateMonitor : IObservable<string>
{
    private readonly int _upPercentage;
    private bool _canceled;

    public StateMonitor( int upPercentage )
    {
        _upPercentage = upPercentage;
    }

    // The "Subscribe" function:
    public IDisposable Subscribe( IObserver<string> observer )
    {
        Task.Run( () => MyTimerFunction( observer, _upPercentage ) );

        return Disposable.Create( () =>
        {
            _canceled = true;
        } );
    }

    private void MyTimerFunction( IObserver<string> observer, int upPercentage )
    {
        try
        {
            var rnd = new Random();
            var currentState = "WARN";
            while( _canceled == false )
            {
                Thread.Sleep( TimeSpan.FromMilliseconds(500) );
                var newState = rnd.NextInt64( 0, 100 ) < upPercentage ? "OK" : "WARN";
                if( newState != currentState )
                {
                    currentState = newState;
                    observer.OnNext( currentState );
                }
            }

            observer.OnCompleted();
        }
        catch( Exception exception )
        {
            observer.OnError( exception );
        }
    }
}
namespace RxTutorial.Lessons.Timers;

public class MyOwnTimer : IDisposable
{
    public event EventHandler<long>? OnTick;
    private readonly TimeSpan _dueTime;
    private readonly TimeSpan _period;
    private bool _canceled;

    public MyOwnTimer( TimeSpan dueTime, TimeSpan period )
    {
        _dueTime = dueTime;
        _period = period;
    }

    public void Start()
    {
        Task.Run( () => MyTimerFunction( _dueTime, _period ) );
    }

    private void MyTimerFunction( TimeSpan dueTime, TimeSpan period )
    {
        try
        {
            var rnd = new Random();
            Thread.Sleep( dueTime );
            while( _canceled == false )
            {
                OnTick?.Invoke( this, rnd.NextInt64(1, 100) );
                Thread.Sleep( period );
            }
        }
        catch( Exception )
        {
            Console.WriteLine( "Ouch" );
        }
    }

    public void Dispose()
    {
        _canceled = true;
    }
}
using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;

namespace RxTutorial;

public static class LessonExecutor
{
    public static Task Execute( this ILesson lesson )
    {
        Console.WriteLine($"\r\nExecuting {lesson.Name}...\r\n");
        var lessonExec = lesson.ExecuteLesson();
        return CancelAfterSeconds( lessonExec, 10 );
    }
    
    private static Task CancelAfterSeconds( IDisposable sub, int seconds )
    {
        return Task.Run( () =>
        {
            Thread.Sleep( seconds * 1000);
            sub.Dispose();
        } );
    }
    
    public static IObservable<ILesson?> AllLessons(  )
    {
        return Observable.Create<ILesson>( observer =>
        {
            Task.Run( () => RunInputLoop( observer ));
            return Disposable.Empty;
        } );
    }

    private static void RunInputLoop( IObserver<ILesson> observer )
    {
        try
        {
            IObservable<string> a;
            var lessons = InitializeLessons();
            ILesson? nextLesson = null;

            do
            {
                nextLesson = GetNextLesson( lessons );
                if( nextLesson != null )
                {
                    observer.OnNext( nextLesson );
                }
            } while( nextLesson != null );

            observer.OnCompleted();
        }
        catch( Exception exception )
        {
            observer.OnError( exception );
        }
    }


    private static ILesson? GetNextLesson( ImmutableList<ILesson> lessons )
    {
            Console.WriteLine("\r\n");
            lessons.ForEach( l=> Console.WriteLine($"{lessons.IndexOf(l) + 1} ... {l.Name}"));
            Console.WriteLine( $"0 ... Quit" );
            Console.Write("> ");
            do
            {
                var rawInput = Console.ReadLine();
                if( !int.TryParse( rawInput, out var inputNumber ) )
                {
                    continue;
                }

                return inputNumber switch
                {
                        0 => null,
                        _ => lessons[inputNumber - 1]
                };

            } while( true );
    }
    
    private static ImmutableList<ILesson> InitializeLessons()
    {
        Console.WriteLine( "\r\nStarting RxTutorial..." );
        return ImmutableList<ILesson>
               .Empty
               .AddRange(
                         Assembly.GetExecutingAssembly()
                                 .GetTypes()
                                 .Where( t => t.GetInterface( nameof( ILesson ) ) != null )
                                 .OrderBy( t=>t.Name)
                                 .Select( Activator.CreateInstance )
                                 .Cast<ILesson>()
                        );
    }
}
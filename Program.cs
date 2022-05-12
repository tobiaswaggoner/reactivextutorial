using System.Reactive.Linq;

using RxTutorial;

#pragma warning disable CS0618

LessonExecutor.AllLessons()
              .ForEach( ExecuteLesson );

static void ExecuteLesson( ILesson? l )
{
    if( l == null )
    {
        return;
    }

    var run = l.Execute();
    run.Wait();
}
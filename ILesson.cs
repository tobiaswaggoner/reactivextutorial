namespace RxTutorial;

public interface ILesson
{
    public string Name { get;  }
    public IDisposable ExecuteLesson( );
}
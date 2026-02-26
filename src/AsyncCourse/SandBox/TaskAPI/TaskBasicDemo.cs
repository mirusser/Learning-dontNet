namespace SandBox.TaskAPI;

public class TaskBasicDemo
{
    public void Basic()
    {
        for (int i = 0; i < 10; i++)
        {
            var temp = i;
            Task.Run(() => Console.WriteLine(temp));
        }
    }

    public void Foo()
    {
        for (int i = 0; i < 10; i++)
        {
            Task.Factory.StartNew(Console.WriteLine, i);
        }
    }
}
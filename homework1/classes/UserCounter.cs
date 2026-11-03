namespace homework1.classes;

using homework1.interfaces;

public class UserCounter : IUserCounter
{
    private int _counter;
    public int Counter => _counter;

    public int UpdateCounter () => Interlocked.Increment(ref _counter);
}
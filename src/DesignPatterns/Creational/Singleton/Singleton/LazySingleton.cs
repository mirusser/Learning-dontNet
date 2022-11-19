namespace Singleton;

public sealed class LazySingleton
{
    private static readonly Lazy<LazySingleton> lazy = new(() => new LazySingleton());

    public static LazySingleton Instance { get { return lazy.Value; } }

    private LazySingleton() { }
}
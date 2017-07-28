namespace Assets.Scripts {

    public interface IDeepCloneable
    {
        object DeepClone();
    }

    /// <summary>
    ///     Provides a deep clone interface to avoid the use of either copy constructors or the ICloneable.
    /// </summary>
    /// <typeparam name="T">The type of the object being cloned</typeparam>
    /// <remarks>
    ///     ICloneable is to be avoided because the .NET Framework never states whether a .Clone() is a deep or shallow copy.
    ///     On the whole, we avoid copy constructors in .NET because the intent is less than clear.
    ///     For more information see https://stackoverflow.com/questions/3345389/copy-constructor-versus-clone and
    ///     https://blogs.msdn.microsoft.com/brada/2003/04/09/implementing-icloneable/
    /// </remarks>
    public interface IDeepCloneable<T> : IDeepCloneable {
        new T DeepClone();
    }

    
}
namespace Uncas.Core
{
    using System;

    /// <summary>
    /// Represents an inversion-of-control container.
    /// </summary>
    [Obsolete("Use Uncas.Core.Ioc.IIocContainer instead.")]
    public interface IIocContainer
    {
        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        [Obsolete("Use Uncas.Core.Ioc.IIocContainer instead.")]
        void RegisterType(Type implementationType, Type interfaceType);

        /// <summary>
        /// Resolves the type T.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>An instance of type T.</returns>
        [Obsolete("Use Uncas.Core.Ioc.IIocContainer instead.")]
        T Resolve<T>();
    }
}
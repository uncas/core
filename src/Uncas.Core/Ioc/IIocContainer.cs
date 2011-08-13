namespace Uncas.Core.Ioc
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents an inversion-of-control container.
    /// </summary>
    public interface IIocContainer
    {
        /// <summary>
        /// Determines whether the service is registered.
        /// </summary>
        /// <param name="type">The type of the service.</param>
        /// <returns>
        ///   <c>True</c> if the service is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Registers the controllers.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void RegisterControllers(Assembly assembly);

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        void RegisterType(Type implementationType, Type interfaceType);

        /// <summary>
        /// Resolves the type T.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>An instance of type T.</returns>
        T Resolve<T>();
    }
}
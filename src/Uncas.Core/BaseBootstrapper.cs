namespace Uncas.Core
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Redirect to new namespace.
    /// </summary>
    [Obsolete("Use Uncas.Core.Ioc.BaseBootstrapper instead.")]
    public class BaseBootstrapper : Ioc.BaseBootstrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        [Obsolete("Use Uncas.Core.Ioc.BaseBootstrapper instead.")]
        public BaseBootstrapper(
            Ioc.IIocContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assembly">The assembly.</param>
        [Obsolete("Use Uncas.Core.Ioc.BaseBootstrapper instead.")]
        public BaseBootstrapper(
            Ioc.IIocContainer container,
            Assembly assembly)
            : base(container, assembly)
        {
        }
    }
}
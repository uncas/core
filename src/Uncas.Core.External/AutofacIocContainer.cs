namespace Uncas.Core.External
{
    using System;
    using Autofac;
    using Autofac.Integration.Web;
    using Uncas.Core.Ioc;

    /// <summary>
    /// Autofac IOC container.
    /// </summary>
    public class AutofacIocContainer : IIocContainer
    {
        private static ContainerBuilder _builder;

        private static IContainer _container;

        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public static ContainerBuilder Builder
        {
            get
            {
                if (_builder == null)
                {
                    _builder = new ContainerBuilder();
                }

                return _builder;
            }
        }

        /// <summary>
        /// Gets the container provider.
        /// </summary>
        /// <value>The container provider.</value>
        public static IContainerProvider ContainerProvider
        {
            get
            {
                return new ContainerProvider(Container);
            }
        }

        private static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = _builder.Build();
                }

                return _container;
            }
        }

        /// <summary>
        /// Resolves an instance of type T.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <returns>An instance of type T.</returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        public void RegisterType(
            Type implementationType,
            Type interfaceType)
        {
            Builder.RegisterType(implementationType).As(interfaceType);
        }
    }
}
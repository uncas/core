﻿namespace Uncas.Core.External
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Uncas.Core.Ioc;

    /// <summary>
    /// Autofac IOC container.
    /// </summary>
    public class AutofacIocContainer : IIocContainer
    {
        private static ContainerBuilder _builder;

        private static IContainer _container;

        /// <summary>
        /// Gets the dependency resolver.
        /// </summary>
        /// <value>The dependency resolver.</value>
        public static IDependencyResolver DependencyResolver
        {
            get { return new AutofacDependencyResolver(Container); }
        }

        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        private static ContainerBuilder Builder
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

        private static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = Builder.Build();
                }

                return _container;
            }
        }

        /// <summary>
        /// Determines whether the service is registered.
        /// </summary>
        /// <param name="type">The type of the service.</param>
        /// <returns>
        ///   <c>True</c> if the service is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered(Type type)
        {
            return Container.IsRegistered(type);
        }

        /// <summary>
        /// Registers the controllers.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void RegisterControllers(Assembly assembly)
        {
            _builder.RegisterControllers(assembly);
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

        /// <summary>
        /// Resolves an instance of type T.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <returns>An instance of type T.</returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
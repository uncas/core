using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Uncas.TimeWatcher
{
    /// <summary>
    /// Holds common bootstrapper logic.
    /// </summary>
    public class BaseBootstrapper
    {
        private IIocContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public BaseBootstrapper(IIocContainer container)
        {
            _container = container;
            RegisterAutomagically();
        }

        /// <summary>
        /// Resolves the given type.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>
        /// An instance of the given type.
        /// </returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private void RegisterAutomagically()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            RegisterImplementationsInAssembly(
                entryAssembly);
            IEnumerable<AssemblyName> referencedAssemblyNames =
                entryAssembly.GetReferencedAssemblies()
                .Where(x => x.Name.StartsWith("Uncas.", StringComparison.OrdinalIgnoreCase));
            foreach (var referencedAssemblyName in referencedAssemblyNames)
            {
                Assembly referencedAssembly =
                    Assembly.Load(referencedAssemblyName);
                RegisterImplementationsInAssembly(
                    referencedAssembly);
            }
        }

        private void RegisterImplementationsInAssembly(
            Assembly assembly)
        {
            foreach (Type implementationType in assembly.GetTypes())
            {
                Type interfaceType = implementationType
                    .GetInterfaces()
                    .SingleOrDefault(
                        x => x.Name == "I" + implementationType.Name);
                if (interfaceType == null)
                {
                    continue;
                }

                _container.RegisterType(implementationType, interfaceType);
            }
        }
    }
}

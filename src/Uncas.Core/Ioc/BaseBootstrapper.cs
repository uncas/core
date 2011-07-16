using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Uncas.Core
{
    /// <summary>
    /// Holds common bootstrapper logic.
    /// </summary>
    public class BaseBootstrapper
    {
        private readonly IIocContainer _container;

        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        [Obsolete("Use other overload which takes assembly explicitly instead.")]
        public BaseBootstrapper(
            IIocContainer container)
            : this(container, Assembly.GetEntryAssembly())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBootstrapper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assembly">The assembly.</param>
        public BaseBootstrapper(
            IIocContainer container,
            Assembly assembly)
        {
            _container = container;
            _assembly = assembly;
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

        private static IEnumerable<AssemblyName> GetReferencedAssemblies(
            Assembly assembly)
        {
            var result = new List<AssemblyName>();
            var referencedAssemblies = assembly.GetReferencedAssemblies();
            var uncas = referencedAssemblies
                .Where(x => x.Name.StartsWith("Uncas.", StringComparison.OrdinalIgnoreCase));
            result.AddRange(uncas);
            foreach (var assemblyName in uncas)
            {
                AddAssemblies(result, GetReferencedAssemblies(Assembly.Load(assemblyName)));
            }

            return result;
        }

        private static void AddAssemblies(
            List<AssemblyName> existing,
            IEnumerable<AssemblyName> found)
        {
            foreach (var an in found)
            {
                if (!existing.Any(x => x.Name == an.Name))
                {
                    existing.Add(an);
                }
            }
        }

        private void RegisterAutomagically()
        {
            RegisterImplementationsInAssembly(
                _assembly);
            IEnumerable<AssemblyName> uncasReferencedAssemblyNames =
                GetReferencedAssemblies(_assembly);
            foreach (var referencedAssemblyName in uncasReferencedAssemblyNames)
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

namespace Uncas.Core.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Holds common bootstrapper logic.
    /// </summary>
    public class BaseBootstrapper
    {
        private readonly Assembly _assembly;
        private readonly IIocContainer _container;

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

        private static void AddAssemblies(
            List<AssemblyName> existing,
            IEnumerable<AssemblyName> found)
        {
            foreach (AssemblyName an in found)
            {
                if (!existing.Any(x => x.Name == an.Name))
                {
                    existing.Add(an);
                }
            }
        }

        private static IEnumerable<AssemblyName> GetReferencedAssemblies(
            Assembly assembly)
        {
            var result = new List<AssemblyName>();
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            IEnumerable<AssemblyName> uncas = referencedAssemblies
                .Where(x => x.Name.StartsWith("Uncas.", StringComparison.OrdinalIgnoreCase));
            result.AddRange(uncas);
            foreach (AssemblyName assemblyName in uncas)
            {
                AddAssemblies(result, GetReferencedAssemblies(Assembly.Load(assemblyName)));
            }

            return result;
        }

        private static bool ShouldBeIgnored(Type implementationType)
        {
            object[] ignoreAttributes =
                implementationType.GetCustomAttributes(
                    typeof(IocIgnoreAttribute),
                    true);
            return ignoreAttributes.Count() > 0;
        }

        private void RegisterAutomagically()
        {
            RegisterImplementationsInAssembly(
                _assembly);
            IEnumerable<AssemblyName> uncasReferencedAssemblyNames =
                GetReferencedAssemblies(_assembly);
            foreach (AssemblyName referencedAssemblyName in uncasReferencedAssemblyNames)
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

                if (ShouldBeIgnored(implementationType))
                {
                    continue;
                }

                _container.RegisterType(implementationType, interfaceType);
            }
        }
    }
}
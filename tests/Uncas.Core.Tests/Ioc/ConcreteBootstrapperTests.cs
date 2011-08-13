namespace Uncas.Core.Tests.Ioc
{
    using NUnit.Framework;
    using Uncas.Core.External;
    using Uncas.Core.Ioc;
    using Uncas.Core.Logging;

    [TestFixture]
    public class ConcreteBootstrapperTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            _container = new AutofacIocContainer();
            _bootstrapper = new BaseBootstrapper(
                _container,
                GetType().Assembly);
        }

        #endregion

        private IIocContainer _container;

        private BaseBootstrapper _bootstrapper;

        [Test]
        public void BaseBootstrapper_WithBasicSetupOfTestAssembly_RunsWithoutFailing()
        {
            // Simply runs the setup method (BeforeEach) and assures that's OK...
        }

        //// TODO: Resolve all registered types...

        [Test]
        public void BaseBootstrapper_WithTestAssembly_ResolvesLogger()
        {
            Assume.That(_container.IsRegistered(typeof(ILogger)));
            var logger = _container.Resolve<ILogger>();
            Assert.NotNull(logger);
        }
    }
}
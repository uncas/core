namespace Uncas.Core.Tests.Ioc
{
    using Moq;
    using NUnit.Framework;
    using Uncas.Core.Data.Migration;
    using Uncas.Core.Ioc;

    [TestFixture]
    public class BaseBootstrapperTests
    {
        [Test]
        public void BaseBootstrapper_WithBasicSetupOfTestAssembly_RunsWithoutFailing()
        {
            var containerMock = new Mock<IIocContainer>();

            var baseBootstrapper = new BaseBootstrapper(
                containerMock.Object,
                GetType().Assembly);
        }

        [Test]
        public void BaseBootstrapper_TestAssembly_RunsWithoutRegisteringMigrationChange()
        {
            var containerMock = new Mock<IIocContainer>();

            var baseBootstrapper = new BaseBootstrapper(
                containerMock.Object,
                GetType().Assembly);

            containerMock.Verify(
                x => x.RegisterType(typeof(MigrationChange), typeof(IMigrationChange)),
                Times.Never());
        }

        [Test]
        public void BaseBootstrapper_TestAssembly_RunsWithRegisteringMigrationService()
        {
            var containerMock = new Mock<IIocContainer>();

            var baseBootstrapper = new BaseBootstrapper(
                containerMock.Object,
                GetType().Assembly);

            containerMock.Verify(
                x => x.RegisterType(typeof(MigrationService), typeof(IMigrationService)),
                Times.Once());
        }

        // TODO: Test resolving all registered types...
    }
}

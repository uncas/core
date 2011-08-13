namespace Uncas.Core.Tests.Data.Migration
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Uncas.Core.Data.Migration;

    [TestFixture]
    public class MigrationServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            _migrationService = new MigrationService();
        }

        #endregion

        private IMigrationService _migrationService;

        private static IMigrationChange GetChange(string id)
        {
            var mock = new Mock<IMigrationChange>();
            mock.Setup(x => x.Id).Returns(id);
            return mock.Object;
        }

        private static MigrationChange GetConcreteChange(string id)
        {
            return new MigrationChange(id);
        }

        [Test]
        public void Migrate_NoChanges_NoMigration()
        {
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object,
                migrationTarget.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Never());
            destinationMock.Verify(
                x => x.AddAppliedChange(It.IsAny<MigrationChange>()), Times.Never());
        }

        [Test]
        public void Migrate_OneChange_IsMigrated()
        {
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();
            var changesToApply = new List<MigrationChange>();
            MigrationChange change = GetConcreteChange("A");
            changesToApply.Add(change);
            sourceMock.Setup(x => x.GetAvailableChanges()).Returns(changesToApply);

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object,
                migrationTarget.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Once());
            destinationMock.Verify(
                x => x.AddAppliedChange(change), Times.Once());
        }

        [Test]
        public void Migrate_OneOldAndTwoNewChanges_TwoAreMigrated()
        {
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
            var changesToApply = new List<MigrationChange>();
            MigrationChange changeA = GetConcreteChange("A");
            MigrationChange changeB = GetConcreteChange("B");
            MigrationChange changeC = GetConcreteChange("C");
            changesToApply.Add(changeA);
            changesToApply.Add(changeB);
            changesToApply.Add(changeC);
            sourceMock.Setup(x => x.GetAvailableChanges()).Returns(changesToApply);

            var destinationMock = new Mock<IAppliedChangeRepository>();
            var appliedChanges = new List<MigrationChange>();
            appliedChanges.Add(changeA);
            destinationMock.Setup(x => x.GetAppliedChanges()).Returns(appliedChanges);

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object,
                migrationTarget.Object);

            destinationMock.Verify(
                x => x.AddAppliedChange(It.IsAny<MigrationChange>()), Times.Exactly(2));
            destinationMock.Verify(
                x => x.AddAppliedChange(changeB), Times.Once());
            destinationMock.Verify(
                x => x.AddAppliedChange(changeC), Times.Once());
        }

        [Test]
        public void Migrate_OneOldChange_NotMigrated()
        {
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var changesToApply = new List<MigrationChange>();
            changesToApply.Add(GetConcreteChange("A"));
            sourceMock.Setup(x => x.GetAvailableChanges()).Returns(changesToApply);
            destinationMock.Setup(x => x.GetAppliedChanges()).Returns(changesToApply);

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object,
                migrationTarget.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Once());
            destinationMock.Verify(
                x => x.AddAppliedChange(It.IsAny<MigrationChange>()), Times.Never());
        }
    }
}
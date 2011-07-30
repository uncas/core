namespace Uncas.Core.Tests.Data.Migration
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class MigrationServiceTests
    {
        private IMigrationService _migrationService;

        [SetUp]
        public void BeforeEach()
        {
            _migrationService = new MigrationService();
        }

        [Test]
        public void Migrate_NoChanges_NoMigration()
        {
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var destinationMock = new Mock<IAppliedChangeRepository>();

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Never());
            destinationMock.Verify(
                x => x.AddAppliedChange(It.IsAny<MigrationChange>()), Times.Never());
        }

        [Test]
        public void Migrate_OneChange_IsMigrated()
        {
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var changesToApply = new List<MigrationChange>();
            MigrationChange change = GetChange("A");
            changesToApply.Add(change);
            sourceMock.Setup(x => x.GetAvailableChanges()).Returns(changesToApply);

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Once());
            destinationMock.Verify(
                x => x.AddAppliedChange(change), Times.Once());
        }

        [Test]
        public void Migrate_OneOldAndTwoNewChanges_TwoAreMigrated()
        {
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var changesToApply = new List<MigrationChange>();
            MigrationChange changeA = GetChange("A");
            MigrationChange changeB = GetChange("B");
            MigrationChange changeC = GetChange("C");
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
                destinationMock.Object);

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
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var changesToApply = new List<MigrationChange>();
            changesToApply.Add(GetChange("A"));
            sourceMock.Setup(x => x.GetAvailableChanges()).Returns(changesToApply);
            destinationMock.Setup(x => x.GetAppliedChanges()).Returns(changesToApply);

            _migrationService.Migrate(
                sourceMock.Object,
                destinationMock.Object);

            sourceMock.Verify(x => x.GetAvailableChanges(), Times.Once());
            destinationMock.Verify(x => x.GetAppliedChanges(), Times.Once());
            destinationMock.Verify(
                x => x.AddAppliedChange(It.IsAny<MigrationChange>()), Times.Never());
        }

        private static MigrationChange GetChange(string id)
        {
            var mock = new Mock<MigrationChange>();
            mock.Setup(x => x.Id).Returns(id);
            return mock.Object;
        }
    }

    public interface IMigrationService
    {
        void Migrate(
            IAvailableChangeRepository source,
            IAppliedChangeRepository destination);
    }

    public class MigrationService : IMigrationService
    {
        public void Migrate(
            IAvailableChangeRepository source,
            IAppliedChangeRepository destination)
        {
            IEnumerable<MigrationChange> availableChanges =
                source.GetAvailableChanges();
            if (availableChanges.Count() == 0)
            {
                return;
            }

            IEnumerable<MigrationChange> appliedChanges =
                destination.GetAppliedChanges();
            foreach (var change in availableChanges)
            {
                if (!IsAlreadyApplied(appliedChanges, change))
                {
                    ApplyChange(destination, change);
                }
            }
        }

        private bool IsAlreadyApplied(
            IEnumerable<MigrationChange> appliedChanges,
            MigrationChange change)
        {
            return appliedChanges.Any(x => x.Id == change.Id);
        }

        private void ApplyChange(
            IAppliedChangeRepository destination,
            MigrationChange change)
        {
            // TODO: Apply change.
            destination.AddAppliedChange(change);
        }
    }

    public interface IAvailableChangeRepository
    {
        IEnumerable<MigrationChange> GetAvailableChanges();
    }

    public interface IAppliedChangeRepository
    {
        IEnumerable<MigrationChange> GetAppliedChanges();

        void AddAppliedChange(MigrationChange change);
    }

    public abstract class MigrationChange
    {
        public virtual string Id { get; set; }
    }
}

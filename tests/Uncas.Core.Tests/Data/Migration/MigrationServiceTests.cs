﻿namespace Uncas.Core.Tests.Data.Migration
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    public interface IMigrationService
    {
        void Migrate(
            IAvailableChangeRepository source,
            IAppliedChangeRepository destination,
            IMigrationTarget migrationTarget);
    }

    public interface IAvailableChangeRepository
    {
        IEnumerable<MigrationChange> GetAvailableChanges();
    }

    public interface IMigrationTarget
    {
        void ApplyChange(MigrationChange change);
    }

    public interface IAppliedChangeRepository
    {
        IEnumerable<MigrationChange> GetAppliedChanges();

        void AddAppliedChange(MigrationChange change);
    }

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
            var migrationTarget = new Mock<IMigrationTarget>();

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
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var migrationTarget = new Mock<IMigrationTarget>();
            var changesToApply = new List<MigrationChange>();
            MigrationChange change = GetChange("A");
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
            var migrationTarget = new Mock<IMigrationTarget>();
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
            var migrationTarget = new Mock<IMigrationTarget>();
            var sourceMock = new Mock<IAvailableChangeRepository>();
            var destinationMock = new Mock<IAppliedChangeRepository>();
            var changesToApply = new List<MigrationChange>();
            changesToApply.Add(GetChange("A"));
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

        private static MigrationChange GetChange(string id)
        {
            var mock = new Mock<MigrationChange>();
            mock.Setup(x => x.Id).Returns(id);
            return mock.Object;
        }
    }

    public abstract class MigrationChange
    {
        public virtual string Id { get; set; }
    }

    public class MigrationService : IMigrationService
    {
        public void Migrate(
            IAvailableChangeRepository availableChangeRepository,
            IAppliedChangeRepository appliedChangeRepository,
            IMigrationTarget migrationTarget)
        {
            IEnumerable<MigrationChange> availableChanges =
                availableChangeRepository.GetAvailableChanges();
            if (availableChanges.Count() == 0)
            {
                return;
            }

            IEnumerable<MigrationChange> appliedChanges =
                appliedChangeRepository.GetAppliedChanges();
            foreach (var change in availableChanges)
            {
                if (!IsAlreadyApplied(appliedChanges, change))
                {
                    ApplyChange(
                        appliedChangeRepository,
                        change,
                        migrationTarget);
                }
            }
        }

        private static bool IsAlreadyApplied(
            IEnumerable<MigrationChange> appliedChanges,
            MigrationChange change)
        {
            return appliedChanges.Any(x => x.Id == change.Id);
        }

        private static void ApplyChange(
            IAppliedChangeRepository appliedChangeRepository,
            MigrationChange change,
            IMigrationTarget migrationTarget)
        {
            migrationTarget.ApplyChange(change);
            appliedChangeRepository.AddAppliedChange(change);
        }
    }
}

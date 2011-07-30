namespace Uncas.Core.Tests.Data.Migration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Uncas.Core.Data;

    public interface IMigrationService
    {
        void Migrate<T>(
            IAvailableChangeRepository<T> source,
            IAppliedChangeRepository destination,
            IMigrationTarget<T> migrationTarget)
            where T : MigrationChange;
    }

    public interface IAvailableChangeRepository<T> where T : MigrationChange
    {
        IEnumerable<T> GetAvailableChanges();
    }

    public interface IMigrationTarget<T> where T : MigrationChange
    {
        void ApplyChange(T change);
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
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
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
            var migrationTarget = new Mock<IMigrationTarget<MigrationChange>>();
            var sourceMock = new Mock<IAvailableChangeRepository<MigrationChange>>();
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

    public interface IMigrationChange
    {
        string Id { get; }
    }

    public class MigrationChange
    {
        public MigrationChange(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }

    public class MigrationService : IMigrationService
    {
        public void Migrate<T>(
            IAvailableChangeRepository<T> availableChangeRepository,
            IAppliedChangeRepository appliedChangeRepository,
            IMigrationTarget<T> migrationTarget) where T : MigrationChange
        {
            IEnumerable<T> availableChanges =
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
                    ApplyChange<T>(
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

        private static void ApplyChange<T>(
            IAppliedChangeRepository appliedChangeRepository,
            T change,
            IMigrationTarget<T> migrationTarget) where T : MigrationChange
        {
            migrationTarget.ApplyChange(change);
            appliedChangeRepository.AddAppliedChange(change);
        }
    }

    public class DbChange : MigrationChange
    {
        public DbChange(
            string id,
            string changeScript)
            : base(id)
        {
            ChangeScript = changeScript;
        }

        public string ChangeScript { get; private set; }
    }

    public class DbTarget : DbContext, IMigrationTarget<DbChange>
    {
        public DbTarget(
            DbProviderFactory factory,
            string connectionString)
            : base(factory, connectionString)
        {
        }

        public void ApplyChange(DbChange change)
        {
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = change.ChangeScript;
                ModifyData(command);
            }
        }
    }

    public class DbAvailableChangeRepository : IAvailableChangeRepository<DbChange>
    {
        public IEnumerable<DbChange> GetAvailableChanges()
        {
            // TODO: Make real implementation here
            // depending on where the scripts are stored:
            var result = new List<DbChange>();
            result.Add(new DbChange("1", "CREATE TABLE ..."));
            result.Add(new DbChange("2", "ALTER TABLE ..."));
            result.Add(new DbChange("3", "DROP TABLE ..."));
            return result;
        }
    }

    public class DbAppliedChangeRepository : DbContext, IAppliedChangeRepository
    {
        public DbAppliedChangeRepository(
            DbProviderFactory factory,
            string connectionString)
            : base(factory, connectionString)
        {
        }

        public IEnumerable<MigrationChange> GetAppliedChanges()
        {
            InitializeDatabase();
            using (DbCommand command = CreateCommand())
            {
                return GetObjects(command, MapToObject);
            }
        }

        public void AddAppliedChange(MigrationChange change)
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // TODO: Implement migration db initialization...
            throw new NotImplementedException();
        }

        private static MigrationChange MapToObject(DbDataReader reader)
        {
            return new MigrationChange((string)reader["Id"]);
        }
    }
}

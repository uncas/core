namespace Uncas.Core.Tests.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using NUnit.Framework;
    using Uncas.Core.Logging;

    [TestFixture]
    public class LogRepositoryTests
    {
        private const string LogFileName = "Test.db";

        private ILogRepository _logRepository;

        [SetUp]
        public void BeforeEach()
        {
            BeforeAndAfterEach();
            var logRepositoryConfiguration =
                new SQLiteLogRepositoryConfiguration(LogFileName);
            var logRepository =
                new LogRepository(logRepositoryConfiguration);
            logRepository.MigrateSchema();
            _logRepository = logRepository;
        }

        [TearDown]
        public void AfterEach()
        {
            BeforeAndAfterEach();
        }

        [Test]
        public void GetLogEntries_OneHourBack_ReturnsNone()
        {
            IEnumerable<LogEntry> logEntries =
                _logRepository.GetLogEntries(DateTime.Now.AddHours(-1d));

            Assert.AreEqual(0, logEntries.Count());
        }

        [Test]
        public void GetLogEntries_SavedOneDayAgo_ReturnsNone()
        {
            SystemTime.Now = () => DateTime.Now.AddDays(-1d);
            var logEntry = new LogEntry(
                LogType.Error,
                "Description",
                null,
                "test");
            _logRepository.Save(logEntry);

            IEnumerable<LogEntry> logEntries =
                _logRepository.GetLogEntries(DateTime.Now.AddHours(-1d));

            Assert.AreEqual(0, logEntries.Count());
        }

        [Test]
        public void GetLogEntries_SavedOne_ReturnsOne()
        {
            var logEntry = new LogEntry(
                LogType.Error,
                "Description",
                null,
                "test");
            _logRepository.Save(logEntry);

            IEnumerable<LogEntry> logEntries =
                _logRepository.GetLogEntries(DateTime.Now.AddHours(-1d));

            Assert.AreEqual(1, logEntries.Count());
        }

        [Test]
        public void Save_WhenGivenEntryWithHttpState_SavesWithoutErrors()
        {
            string url = "http://example.com/a/b?c=d&e=f";
            var request = new HttpRequest(string.Empty, url, string.Empty);
            var writer = new StringWriter();
            var response = new HttpResponse(writer);
            var httpContext = new HttpContext(request, response);
            HttpContext.Current = httpContext;

            var logEntry = new LogEntry(
                LogType.Error,
                "Description",
                null,
                "test");

            _logRepository.Save(logEntry);
        }

        [Test]
        public void Save_WhenGivenEntry_SavesWithoutErrors()
        {
            var logEntry = new LogEntry(
                LogType.Error,
                "Description",
                null,
                "test");

            _logRepository.Save(logEntry);
        }

        private void BeforeAndAfterEach()
        {
            SystemTime.Now = () => DateTime.Now;
            File.Delete(LogFileName);
        }
    }
}
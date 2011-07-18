namespace Uncas.Core.Tests.Logging
{
    using System.IO;
    using System.Web;
    using NUnit.Framework;
    using Uncas.Core.Logging;

    [TestFixture]
    public class LogRepositoryTests
    {
        private ILogRepository _logRepository;

        [SetUp]
        public void BeforeEach()
        {
            var logDbContext = new SQLiteLogDbContext();
            _logRepository = new LogRepository(logDbContext);
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
    }
}

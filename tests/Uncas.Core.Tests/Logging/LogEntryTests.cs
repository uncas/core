namespace Uncas.Core.Tests.Logging
{
    using System.IO;
    using System.Web;
    using NUnit.Framework;
    using Uncas.Core.Logging;

    [TestFixture]
    public class LogEntryTests
    {
        [Test]
        public void LogEntry_WithoutHttpContext_WithoutHttpState()
        {
            HttpContext.Current = null;

            var logEntry = new LogEntry(LogType.Error, "test", null, null);

            Assert.IsNull(logEntry.HttpState);
        }

        [Test]
        public void LogEntry_WithHttpContext_WithHttpState()
        {
            string url = "http://example.com/a/b?c=d&e=f";
            var request = new HttpRequest(string.Empty, url, string.Empty);
            var writer = new StringWriter();
            var response = new HttpResponse(writer);
            var httpContext = new HttpContext(request, response);
            HttpContext.Current = httpContext;

            var logEntry = new LogEntry(LogType.Error, "test", null, null);

            LogEntryHttpState httpState = logEntry.HttpState;
            Assert.IsNotNull(httpState);
            Assert.AreEqual(url, httpState.Url);
        }
    }
}

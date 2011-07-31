﻿namespace Uncas.Core.Tests.Logging
{
    using System;
    using System.IO;
    using System.Web;
    using NUnit.Framework;
    using Uncas.Core.Logging;

    [TestFixture]
    public class LogEntryTests
    {
        [SetUp]
        public void BeforeEach()
        {
            HttpContext.Current = null;
        }

        [Test]
        public void LogEntry_WithoutHttpContext_WithoutHttpState()
        {
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
            Assert.AreEqual(url, httpState.Url.AbsoluteUri);
        }

        [Test]
        public void LogEntry_WithException_FileNameFromException()
        {
            var exception = new Exception("Test exception.");
            var logEntry = new LogEntry(LogType.Error, "test", exception, null);

            Assert.That(logEntry.FileName.EndsWith("LogEntryTests.cs"));
            Assert.AreEqual(47, logEntry.LineNumber);
        }

        [Test]
        public void LogEntry_ThrownException_FileNameFromException()
        {
            try
            {
                throw new Exception("Test exception.");
            }
            catch (Exception exception)
            {
                var logEntry = new LogEntry(LogType.Error, "test", exception, null);
                Assert.That(logEntry.FileName.EndsWith("LogEntryTests.cs"));
            }
        }

        [Test]
        public void LogEntry_WithoutException_FileNameFromCaller()
        {
            var logEntry = new LogEntry(LogType.Warning, "test", null, null);

            Assert.That(logEntry.FileName.EndsWith("LogEntryTests.cs"));
        }
    }
}

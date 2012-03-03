namespace Uncas.Core.Tests.Logging
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Uncas.Core.Logging;

    [TestFixture]
    public class LoggerTests
    {
        private ILogger _logger;

        private Mock<ILogRepository> _logRepositoryMock;

        [SetUp]
        public void BeforeEach()
        {
            _logRepositoryMock = new Mock<ILogRepository>();
            _logger = new Logger(_logRepositoryMock.Object);
        }

        [Test]
        public void Log_WithDescription_SavesDescription()
        {
            string description = Guid.NewGuid().ToString();

            _logger.Log(LogType.Error, description);

            _logRepositoryMock.Verify(
                x => x.Save(It.Is<LogEntry>(y => y.Description == description)));
        }

        [Test]
        public void Log_WithoutException_FileNameFromCaller()
        {
            _logger.Log(LogType.Error, "test");

            _logRepositoryMock.Verify(
                x => x.Save(It.Is<LogEntry>(y => y.FileName.EndsWith("LoggerTests.cs"))));
        }
    }
}
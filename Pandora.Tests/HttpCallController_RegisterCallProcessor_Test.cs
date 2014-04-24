using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pandora.Http;
using Rhino.Mocks;

namespace Pandora.Tests
{
    [TestFixture]
    public class HttpCallController_RegisterCallProcessor_Test
    {
        private HttpCallController _controller;
        private IExceptionResponseHandler _mockResponseHandler;

        [SetUp]
        public void Init()
        {
            _mockResponseHandler = MockRepository.GenerateStub<IExceptionResponseHandler>();
            _controller = new HttpCallController(_mockResponseHandler);
        }

        [Test]
        public void RegisterCallProcessor_With_Null_Or_Null_Key_Throws()
        {
            // Arrange
            IHttpCallProcessor processor = MockRepository.GenerateStub<IHttpCallProcessor>();

            // Act
            // Assert
            Assert.Throws<NullReferenceException>(() => _controller.RegisterCallProcessor(null));
            Assert.Throws<ArgumentNullException>(() => _controller.RegisterCallProcessor(processor));
        }

        [Test]
        public void RegisterCallProcessor_With_A_Call_Processor_Adds_Processor()
        {
            // Arrange
            IHttpCallProcessor processor = MockRepository.GenerateStub<IHttpCallProcessor>();
            processor.Stub(s => s.Method).Return("TEST");
            var preRegisterProcessorCount = _controller.CallProcessors.Count();
            
            // Act
            _controller.RegisterCallProcessor(processor);
            var postRegisterProceessorCount = _controller.CallProcessors.Count();

            // Assert
            Assert.AreEqual(0, preRegisterProcessorCount);
            Assert.AreEqual(1, postRegisterProceessorCount);
            Assert.AreSame(processor, _controller.CallProcessors.First().Value);
        }

        [Test]
        public void RegisterCallProcessor_Adding_Same_Twice_Throws_ArgumentException()
        {
            // Arrange
            IHttpCallProcessor processor = MockRepository.GenerateStub<IHttpCallProcessor>();
            processor.Stub(s => s.Method).Return("TEST");

            // Act
            _controller.RegisterCallProcessor(processor);

            // Assert
            Assert.Throws<ArgumentException>(() => _controller.RegisterCallProcessor(processor));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pandora.Http;
using Rhino.Mocks;
using Pandora.Helpers;
using Pandora.Http.Exceptions;

namespace Pandora.Tests
{
    [TestFixture]
    public class HttpCallController_ProcessCall_Test
    {
        private HttpCallController _controller;
        private IExceptionResponseHandler _mockResponseHandler;
        private IHttpCallProcessor _mockProcessor;
        private INetworkStreamWrapper _mockWrapper;

        [SetUp]
        public void Init()
        {
            _mockResponseHandler = MockRepository.GenerateStub<IExceptionResponseHandler>();
            _controller = new HttpCallController(_mockResponseHandler);

            _mockProcessor = MockRepository.GenerateStub<IHttpCallProcessor>();
            _mockProcessor.Stub(s => s.Method).Return("TEST");
            _controller.RegisterCallProcessor(_mockProcessor);

            _mockWrapper = MockRepository.GenerateStub<INetworkStreamWrapper>();
        }

        [Test]
        public void ProcessCall_With_Null_Throws()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<NullReferenceException>(() => _controller.ProcessCall(null));
        }

        [Test]
        public void ProcessCall_With_No_Processors_Throws_BadRequestException()
        {
            // Arrange
            _mockWrapper.Stub(s => s.PeekToString(9)).Return("FAIL ");

            // Act
            // Assert
            var ex = Assert.Throws<BadRequestException>(() => _controller.ProcessCall(_mockWrapper));
            Assert.AreEqual("No IHttpCallProcessor for verb FAIL", ex.Message);
        }

        [Test]
        public void ProcessCall_With_Malformated_Request_Line_Throws_BadRequestException()
        {
            // Arrange
            _mockWrapper.Stub(s => s.PeekToString(9)).Return("MALFORMATED_LINE");

            // Act
            // Assert
            var ex = Assert.Throws<BadRequestException>(() => _controller.ProcessCall(_mockWrapper));
            Assert.AreEqual("Incoming call malformated", ex.Message);
        }

        [Test]
        public void ProcessCall_With_No_Matching_Processor_Throws_BadRequestException()
        {
            // Arrange
            _mockWrapper.Stub(s => s.PeekToString(9)).Return("FAIL ");
            _mockResponseHandler.Stub(s => s.HasResponse("NoSpecificException")).Return(true);

            // Act
            // Assert
            var ex = Assert.Throws<BadRequestException>(() => _controller.ProcessCall(_mockWrapper));
            Assert.AreEqual("No IHttpCallProcessor for verb FAIL", ex.Message);
        }

        [Test]
        public void ProcessCall_With_No_Matching_Processor_Throws_BadRequestException_And_Gets_Handled()
        {
            // Arrange
            var response = new HttpResponse();
            _mockWrapper.Stub(s => s.PeekToString(9)).Return("FAIL ");
            _mockResponseHandler.Stub(s => s.HasResponse("BadRequestException")).Return(true);
            _mockResponseHandler.Stub(s => s.GetResponseFromException(Arg<BadRequestException>.Is.Anything))
                                .Return(response);
            _mockWrapper.Stub(s => s.Send(Arg<byte[]>.Is.Anything)).Return(15);

            // Act
            _controller.ProcessCall(_mockWrapper);

            // Assert
            _mockWrapper.AssertWasCalled(s => s.Send(Arg<byte[]>.Is.Anything));
        }
    }
}

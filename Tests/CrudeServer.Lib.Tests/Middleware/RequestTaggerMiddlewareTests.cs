﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CrudeServer.Middleware;
using CrudeServer.Models.Contracts;

using Moq;

namespace CrudeServer.Lib.Tests.Middleware
{
    public class RequestTaggerMiddlewareTests
    {
        [Test]
        public async Task Process_AddGuidToResponseHeaders()
        {
            // Arrange
            Mock<IRequestContext> context = new Mock<IRequestContext>();
            Dictionary<string, string> headers = new Dictionary<string, string>();

            context.Setup(x => x.ResponseHeaders).Returns(headers);

            Func<Task> next = () => Task.CompletedTask;

            RequestTaggerMiddleware loggerMiddleware = new RequestTaggerMiddleware();

            // Act
            await loggerMiddleware.Process(context.Object, next);

            // Assert
            Assert.That(headers.ContainsKey("X-Request-Id"), Is.True);
        }
    }
}
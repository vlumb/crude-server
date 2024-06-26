﻿using System.Collections.Generic;
using System.Threading.Tasks;

using CrudeServer.Models;
using CrudeServer.Models.Contracts;
using CrudeServer.Providers;

using Moq;

namespace CrudeServer.Lib.Tests.Providers
{
    public class EmbeddedHandleBarsViewProviderTests
    {
        [Test]
        public async Task ViewSimpleCanBeRendered()
        {
            // Arrange
            EmbeddedHandleBarsViewProvider viewProvider = new EmbeddedHandleBarsViewProvider(
                GetType().Assembly,
                "files",
                new ServerConfig()
            );

            // Act
            string result = await viewProvider.GetTemplate(
                "simpleView.html",
                new Dictionary<string, object>
                {
                    { "viewModel", new { name = "John Doe" } }
                },
                Mock.Of<ICommandContext>()
            );

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Contains.Substring("Hello John Doe!"));
        }

        [Test]
        public async Task ViewWithLayoutCanBeRendered()
        {
            // Arrange
            EmbeddedHandleBarsViewProvider viewProvider = new EmbeddedHandleBarsViewProvider(
                GetType().Assembly,
                "files",
                new ServerConfig());

            // Act
            string result = await viewProvider.GetTemplate(
                "viewWithLayout.html",
                new Dictionary<string, object>
                {
                    { "viewModel", new { name = "John Doe" } }
                },
                Mock.Of<ICommandContext>()
            );

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Contains.Substring("Hello John Doe! This comes from a template."));
        }

        [Test]
        public async Task ViewWithPartialsCanBeRendered()
        {
            // Arrange
            EmbeddedHandleBarsViewProvider viewProvider = new EmbeddedHandleBarsViewProvider(
                GetType().Assembly,
                "files",
                new ServerConfig());

            // Act
            string result = await viewProvider.GetTemplate(
                "viewWithLayoutAndPartial.html",
                new
                {
                    viewModel = new
                    {
                        name = "John Doe"
                    }
                },
                Mock.Of<ICommandContext>()
            );

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Contains.Substring("Hello John Doe! This comes from a template."));
            Assert.That(result, Contains.Substring("HELLO I AM A PARTIAL"));
            Assert.That(result, Contains.Substring("AND I AM A PARTIAL AS WELL"));
        }
    }
}

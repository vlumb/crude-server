﻿using System.Security.Principal;
using System.Threading.Tasks;

using CrudeServer.Middleware;
using CrudeServer.Models.Contracts;
using CrudeServer.Providers.Contracts;

using Moq;

namespace CrudeServer.Lib.Tests.Middleware
{
    public class AuthenticatorMiddlewareTests
    {
        [Test]
        public async Task AuthenticationProviderReturnsIPrincipal_SetsUser()
        {
            // Arrange
            Mock<IAuthenticationProvider> authenticationProvider = new Mock<IAuthenticationProvider>();
            authenticationProvider
                .Setup(ap => ap.GetUser(It.IsAny<IRequestContext>()))
                .ReturnsAsync(new Mock<IPrincipal>().Object);

            Mock<IRequestContext> requestContext = new Mock<IRequestContext>();

            AuthenticatorMiddleware authenticator = new AuthenticatorMiddleware(authenticationProvider.Object);

            // Act
            await authenticator.Process(requestContext.Object, () => Task.CompletedTask);

            // Assert
            authenticationProvider.Verify(ap => ap.GetUser(It.IsAny<IRequestContext>()), Times.Once);
            requestContext.VerifySet(rc => rc.User = It.IsAny<IPrincipal>(), Times.Once);
        }

        [Test]
        public async Task AuthenticationProviderDoesNotHaveIPrincipal_DoesNotSetUser()
        {
            // Arrange
            Mock<IAuthenticationProvider> authenticationProvider = new Mock<IAuthenticationProvider>();
            authenticationProvider
                .Setup(ap => ap.GetUser(It.IsAny<IRequestContext>()))
                .ReturnsAsync((IPrincipal)null);

            Mock<IRequestContext> requestContext = new Mock<IRequestContext>();

            AuthenticatorMiddleware authenticator = new AuthenticatorMiddleware(authenticationProvider.Object);

            // Act
            await authenticator.Process(requestContext.Object, () => Task.CompletedTask);

            // Assert
            authenticationProvider.Verify(ap => ap.GetUser(It.IsAny<IRequestContext>()), Times.Once);
            requestContext.VerifySet(rc => rc.User = It.IsAny<IPrincipal>(), Times.Never);
        }
    }
}
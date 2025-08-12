using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.UseCases;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.User.Commands;

[TestClass]
public class AuthenticateUserCommandHandlerTests
{
    Mock<IStrongerDbContext> _context;
    Mock<ITokenService> _tokenService;
    Mock<IPasswordService> _passwordService;
    IRequestHandler<AuthenticateUserCommand, Response> _handler;
    public AuthenticateUserCommandHandlerTests()
    {
        _context = new Mock<IStrongerDbContext>();
        _tokenService = new Mock<ITokenService>();
        _passwordService = new Mock<IPasswordService>();

        IServiceCollection services = new ServiceCollection();
        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_context.Object);
        services.AddSingleton(_tokenService.Object);
        services.AddSingleton(_passwordService.Object);

        var provider = services.BuildServiceProvider();

        _handler = provider.GetRequiredService<IRequestHandler<AuthenticateUserCommand, Response>>();
    }

    [TestMethod]
    async Task TestHandle_Invalid()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await _handler.Handle(null!, CancellationToken.None);
        });
    }
}
using System.Collections.Generic;
using TelegramBotPrototype.Common.Commands;
using TelegramBotPrototype.SDK.Options;

namespace TelegramBotPrototype.SDK.Tests;

public class MessageCommandRouterTests
{
    private readonly Mock<ITelegramBotClient> _botClientMock = new();
    private readonly Mock<UserStateManager> _stateManagerMock = new();
    private readonly MessageCommandRouter _router;

    private readonly long _adminId = 1111;
    private readonly long _adminGroupChatId = 2222;

    public MessageCommandRouterTests()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new BotOptions()
        {
            AdminGroupChatId = _adminGroupChatId,
            AdminId = _adminId,
        });

        _router = new MessageCommandRouter(_stateManagerMock.Object, options);

        _router.RegisterCommand(new Test1Command());
        _router.RegisterCommand(new TestAdminCommand());

        _router.RegisterCommandFollowUp(new Test1CommandFollowUp());
    }

    [Fact]
    public async Task RouterExecutesCommandSuccessfully_Test()
    {
        var message = new Message() { Text = "/test_1", From = new() { Id = _adminId } };

        await _router.ExecuteAsync(_botClientMock.Object, message, new());

        Assert.True(true);
    }

    [Fact]
    public async Task RouterExecutesCommandFollowUpSuccessfully_Test()
    {
        var state = new UserCommandState() { Command = new Test1Command(), UserState = Enums.UserState.WaitingForReply };
        _stateManagerMock
            .Setup(x => x.GetUserSate(It.IsAny<long>()))
            .Returns(state);

        var message = new Message() { Text = "Hello world. Waiting for follow-up", From = new() { Id = _adminId } };

        await _router.ExecuteAsync(_botClientMock.Object, message, new());

        _stateManagerMock.Verify(x => x.GetUserSate(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task RouterExecutesAdminCommandSuccefully_Test()
    {
        var message = new Message() { Text = "/test_admin", From = new() { Id = _adminId } };

        await _router.ExecuteAsync(_botClientMock.Object, message, new());

        Assert.True(true);
    }

    [Fact]
    public async Task RouterExecutesAdminCommandUnsuccefully_Test()
    {
        var message = new Message() { Text = "/test_admin", From = new() { Id = 1010 } };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _router.ExecuteAsync(_botClientMock.Object, message, new()));
    }
}

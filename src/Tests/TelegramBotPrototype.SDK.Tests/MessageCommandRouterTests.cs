using TelegramBotPrototype.Common.Commands;

namespace TelegramBotPrototype.SDK.Tests;

public class MessageCommandRouterTests
{
    private readonly Mock<ITelegramBotClient> _botClientMock = new();
    private readonly Mock<UserStateManager> _stateManagerMock = new();
    private readonly MessageCommandRouter _router;

    public MessageCommandRouterTests()
    {
        _router = new MessageCommandRouter(_stateManagerMock.Object);
    }

    [Fact]
    public async Task RouterExecutesCommandSuccessfully_Test()
    {
        // Setup mocks
        var state = new UserCommandState() { Command = new Test1Command(), UserState = Enums.UserState.WaitingForReply };
        _stateManagerMock
            .Setup(x => x.GetUserSate(It.IsAny<long>()))
            .Returns(state);

        _router.RegisterCommand(new Test1Command());
        _router.RegisterCommandFollowUp(new Test1CommandFollowUp());

        var message = new Message() { Text = "Hello world!", From = new() { Id = 1111 } };

        await _router.ExecuteAsync(_botClientMock.Object, message, new());
    }
}

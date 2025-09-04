using Telegram.Bot;
using Telegram.Bot.Types;
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
        _router.RegisterCommand(new Test1Command());


        var message = new Message() { Text = "" };

        await _router.ExecuteAsync(_botClientMock.Object, message, new());
    }
}

using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.SDK.Commands;

namespace TelegramBotPrototype.SDK.Tests;

class TestCommand1 : BaseCommand
{
    public override string Path => "test_1";
    public override bool AdminOnly => false;
    public override bool RequiresState => true;

    public override Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}

class TestCommand2 : BaseCommand
{
    public override string Path => "test_2";
    public override bool AdminOnly => false;
    public override bool RequiresState => true;

    public override Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}


public class UserStateManagerTests
{
    private readonly UserStateManager _userStateManager;

    public UserStateManagerTests()
    {
        _userStateManager = new UserStateManager();
    }

    [Fact]
    public void UserManagerAddsUserSuccessfully_Test()
    {
        var userId = 1;

        var command1 = new TestCommand1();
        var command2 = new TestCommand2();

        _userStateManager.SetUserState(userId, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });

        var result = _userStateManager.GetUserSate(userId);

        Assert.Equal(Enums.UserState.WaitingForReply, result.UserState);
        Assert.Equal(typeof(TestCommand1), result.Command.GetType());
    }
}

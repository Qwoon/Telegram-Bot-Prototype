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

        _userStateManager.SetUserState(userId, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });

        var result = _userStateManager.GetUserSate(userId);

        Assert.Equal(Enums.UserState.WaitingForReply, result.UserState);
        Assert.Equal(typeof(TestCommand1), result.Command.GetType());
    }

    [Fact]
    public void UserManagerAddsAndDeletesUserSuccessfully_Test()
    {
        var userId = 1;

        var command1 = new TestCommand1();

        _userStateManager.SetUserState(userId, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });

        var result = _userStateManager.GetUserSate(userId);
        _userStateManager.RemoveUserState(userId);
        result = _userStateManager.GetUserSate(userId);

        Assert.Null(result);
    }

    [Fact]
    public void UserManagerAddsUsersSuccessfully_Test()
    {
        var userId1 = 1;
        var userId2 = 2;

        var command1 = new TestCommand1();
        var command2 = new TestCommand2();

        _userStateManager.SetUserState(userId1, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });
        _userStateManager.SetUserState(userId2, new UserCommandState { Command = command2, UserState = Enums.UserState.Default });

        var result1 = _userStateManager.GetUserSate(userId1);
        var result2 = _userStateManager.GetUserSate(userId2);

        // User1 Results
        Assert.Equal(Enums.UserState.WaitingForReply, result1.UserState);
        Assert.Equal(typeof(TestCommand1), result1.Command.GetType());

        // User2 Results
        Assert.Equal(Enums.UserState.Default, result2.UserState);
        Assert.Equal(typeof(TestCommand2), result2.Command.GetType());
    }

    [Fact]
    public void UserManagerAddsAndDeletesUsersSuccessfully_Test()
    {
        var userId1 = 1;
        var userId2 = 2;

        var command1 = new TestCommand1();
        var command2 = new TestCommand2();

        _userStateManager.SetUserState(userId1, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });
        _userStateManager.SetUserState(userId2, new UserCommandState { Command = command2, UserState = Enums.UserState.Default });

        var result1 = _userStateManager.GetUserSate(userId1);
        var result2 = _userStateManager.GetUserSate(userId2);
        _userStateManager.RemoveUserState(userId2);
        result2 = _userStateManager.GetUserSate(userId2);

        // User1 Results
        Assert.Equal(Enums.UserState.WaitingForReply, result1.UserState);
        Assert.Equal(typeof(TestCommand1), result1.Command.GetType());

        // User2 Results
        Assert.Null(result2);
    }
}

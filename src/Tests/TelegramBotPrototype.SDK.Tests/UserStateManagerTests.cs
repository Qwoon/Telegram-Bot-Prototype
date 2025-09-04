using TelegramBotPrototype.Common.Commands;

namespace TelegramBotPrototype.SDK.Tests;

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

        var command1 = new Test1Command();

        _userStateManager.SetUserState(userId, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });

        var result = _userStateManager.GetUserSate(userId);

        Assert.Equal(Enums.UserState.WaitingForReply, result.UserState);
        Assert.Equal(typeof(Test1Command), result.Command.GetType());
    }

    [Fact]
    public void UserManagerAddsAndDeletesUserSuccessfully_Test()
    {
        var userId = 1;

        var command1 = new Test1Command();

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

        var command1 = new Test1Command();
        var command2 = new Test2Command();

        _userStateManager.SetUserState(userId1, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });
        _userStateManager.SetUserState(userId2, new UserCommandState { Command = command2, UserState = Enums.UserState.Default });

        var result1 = _userStateManager.GetUserSate(userId1);
        var result2 = _userStateManager.GetUserSate(userId2);

        // User1 Results
        Assert.Equal(Enums.UserState.WaitingForReply, result1.UserState);
        Assert.Equal(typeof(Test1Command), result1.Command.GetType());

        // User2 Results
        Assert.Equal(Enums.UserState.Default, result2.UserState);
        Assert.Equal(typeof(Test2Command), result2.Command.GetType());
    }

    [Fact]
    public void UserManagerAddsAndDeletesUsersSuccessfully_Test()
    {
        var userId1 = 1;
        var userId2 = 2;

        var command1 = new Test1Command();
        var command2 = new Test2Command();

        _userStateManager.SetUserState(userId1, new UserCommandState { Command = command1, UserState = Enums.UserState.WaitingForReply });
        _userStateManager.SetUserState(userId2, new UserCommandState { Command = command2, UserState = Enums.UserState.Default });

        var result1 = _userStateManager.GetUserSate(userId1);
        var result2 = _userStateManager.GetUserSate(userId2);
        _userStateManager.RemoveUserState(userId2);
        result2 = _userStateManager.GetUserSate(userId2);

        // User1 Results
        Assert.Equal(Enums.UserState.WaitingForReply, result1.UserState);
        Assert.Equal(typeof(Test1Command), result1.Command.GetType());

        // User2 Results
        Assert.Null(result2);
    }
}

using System.Collections.Concurrent;
using TelegramBotPrototype.SDK.Commands;
using TelegramBotPrototype.SDK.Enums;

namespace TelegramBotPrototype.SDK;

public class UserCommandState
{
    public required BaseCommand Command { get; set; }
    public required UserState UserState { get; set; }
}

public class UserStateManager
{
    private readonly ConcurrentDictionary<long, UserCommandState> _userState = new();

    public virtual void SetUserState(long userId, UserCommandState userCommandState) => _userState.AddOrUpdate(userId, userCommandState, (key, oldValue) => userCommandState);
    public virtual UserCommandState GetUserSate(long userId) => _userState.GetValueOrDefault(userId);
    public virtual void RemoveUserState(long userId) => _userState.TryRemove(userId, out _);

}

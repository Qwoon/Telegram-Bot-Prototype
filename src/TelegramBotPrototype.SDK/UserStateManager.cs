using System.Collections.Concurrent;
using TelegramBotPrototype.SDK.Enums;

namespace TelegramBotPrototype.SDK;

public class UserStateManager
{
    private readonly ConcurrentDictionary<long, UserState> _userState = new();

    public void SetUserState(long userId, UserState userState) => _userState.AddOrUpdate(userId, userState, (key, oldValue) => userState);
    public UserState GetUserSate(long userId) => _userState.GetValueOrDefault(userId);
    public void RemoveUserState(long userId) => _userState.TryRemove(userId, out _);

}

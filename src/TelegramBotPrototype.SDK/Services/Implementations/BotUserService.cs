using Microsoft.EntityFrameworkCore;
using TelegramBotPrototype.Data.Entities;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK.Services.Implementations;

public sealed class BotUserService : ResourceService<BotChat, long>, IBotUserService
{
    public BotUserService(DbContext context, string key = null) : base(context, key) { }
}

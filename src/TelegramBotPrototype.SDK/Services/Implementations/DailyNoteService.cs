using Microsoft.EntityFrameworkCore;
using TelegramBotPrototype.Data.Entities;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK.Services.Implementations;

public class DailyNoteService : ResourceService<DailyNote, long>, IDailyNoteService
{
    public DailyNoteService(DbContext context, string key = null) : base(context, key) { }
}

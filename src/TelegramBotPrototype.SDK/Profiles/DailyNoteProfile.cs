using TelegramBotPrototype.Data.Entities;

namespace TelegramBotPrototype.SDK.Profiles;

public class DailyNoteProfile : Profile
{
    public DailyNoteProfile()
    {
        CreateMap<DailyNote, DailyNote>();
    }
}

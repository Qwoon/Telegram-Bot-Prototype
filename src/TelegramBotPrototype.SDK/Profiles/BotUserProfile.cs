using TelegramBotPrototype.Data.Entities;

namespace TelegramBotPrototype.SDK.Profiles;

public class BotUserProfile : Profile
{
    public BotUserProfile()
    {
        CreateMap<BotChat, BotChat>();
    }
}

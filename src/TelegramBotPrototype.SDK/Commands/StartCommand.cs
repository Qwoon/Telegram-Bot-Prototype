using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.Data.Entities;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK.Commands;

public class StartCommand(IBotUserService botUserService) : BaseCommand
{
    private readonly IBotUserService _botUserService = botUserService;

    public override string Path => "start";
    public override bool AdminOnly => false;
    public override bool RequiresState => false;

    public override async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        var chat = await _botUserService
            .Search()
            .Where(x => x.ChatId == message.Chat.Id)
            .FirstOrDefaultAsync();

        if (chat == null)
            await _botUserService.InsertAsync(new BotChat() { ChatId = message.Chat.Id });

        var text = """
        Welcome to Qwoon's Telegram Bot Prototype

        🌎 Github:

        https://github.com/Qwoon/

        💙 Feel free to test out the bot and it's commands
        """;

        await SendMessageAsync(client, message, text);
    }
}

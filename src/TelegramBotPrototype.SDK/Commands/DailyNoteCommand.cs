using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.Data.Entities;
using TelegramBotPrototype.SDK.Extensions;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK.Commands;

public class DailyNoteCommand : BaseCommand
{
    private readonly IDailyNoteService _service;
    private readonly IBotUserService _botUserService;
    private readonly IMapper _mapper;

    public DailyNoteCommand(
        IDailyNoteService service,
        IBotUserService botUserService,
        IMapper mapper)
    {
        _service = service;
        _botUserService = botUserService;
        _mapper = mapper;
    }

    public override string Path => "daily_note";
    public override bool AdminOnly => true;
    public override bool RequiresState => false;

    public override async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        var messageText = message.Text.TrimStart('/').Replace(Path, "").Trim();
        var entity = await _service.InsertAsync(new() { IsSent = false, Text = messageText }, ct);

        // TODO: It's better to move this out of the scope of this command
        // But we will do it here anyways >:)

        var users = _botUserService.Search().ToList();

        foreach (var user in users)
        {
            try
            {
                await client.SendMessage(chatId: user.ChatId, text: messageText, cancellationToken: ct);
            }
            catch
            {
                // Ideally we would need to check if each user got the message and change the note status IsSend to TRUE
            }
        }

        await _service.UpdateAsync(entity.Id, new DailyNote() { Text = messageText, IsSent = true }, _mapper.Patch, ct);
    }
}

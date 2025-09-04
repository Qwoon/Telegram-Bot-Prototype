using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.SDK.Commands;
using TelegramBotPrototype.SDK.Interfaces;
using TelegramBotPrototype.SDK.Options;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK;

public sealed class BotUpdateHandler : IBotUpdateHandler
{
    private readonly MessageCommandRouter _messageCommandRouter;
    private readonly UserStateManager _userStateManager;
    private readonly BotOptions _options;

    private readonly ITelegramBotClient _client;
    private readonly IServiceProvider _provider;
    private readonly ILogger<BotUpdateHandler> _logger;

    public BotUpdateHandler(
        MessageCommandRouter messageCommandRouter,
        UserStateManager userStateManager,
        ITelegramBotClient client,
        IServiceProvider provider,
        ILogger<BotUpdateHandler> logger,
        IOptions<BotOptions> options)
    {
        _messageCommandRouter = messageCommandRouter;
        _userStateManager = userStateManager;
        _client = client;
        _provider = provider;
        _logger = logger;
        _options = options.Value;

        RegisterCommands();
    }

    public async Task HandleAsync(Update update, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        await (update switch
        {
            { Message: { } message } => OnMessageAsync(message, ct),
            _ => Task.CompletedTask
        });
    }

    private async Task OnMessageAsync(Message message, CancellationToken ct)
    {
        _logger.LogInformation("Received message from chat #{chatId}. User {user}", message.Chat.Id, message.From.Username);

        if (_userStateManager.GetUserSate(message.From.Id) == Enums.UserState.WaitingForReply)
        {
            // TODO: #1 Save the reply

            // TODO: #2 Send follow-up to user

            await _client.SendMessage(
                chatId: message.Chat.Id,
                text: "Your question has been successfully delivered to the admins.");

            // TODO: #3 Format & Send the message to a group chat

            var groupMessage = $"""
                From: @{message.From.Username}
                Date: {Core.TimeProvider.ConvertToRigaTime(DateTime.UtcNow)}
                Message: {message.Text}
                """;

            await _client.SendMessage(
                chatId: _options.AdminGroupChatId,
                text: groupMessage);

            _userStateManager.RemoveUserState(message.From.Id);
        }
        else
        {
            await _messageCommandRouter.ExecuteAsync(_client, message, ct);
        }
    }

    private void RegisterCommands()
    {
        _messageCommandRouter.RegisterCommand(new DailyNoteCommand(
            _provider.GetRequiredService<IDailyNoteService>(),
            _provider.GetRequiredService<IBotUserService>(),
            _provider.GetRequiredService<IMapper>()));
        _messageCommandRouter.RegisterCommand(new StartCommand(_provider.GetRequiredService<IBotUserService>()));
        _messageCommandRouter.RegisterCommand(new StoryCommand(_userStateManager));
    }
}

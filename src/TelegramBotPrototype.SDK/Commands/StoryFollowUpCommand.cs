using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.SDK.Interfaces;
using TelegramBotPrototype.SDK.Options;

namespace TelegramBotPrototype.SDK.Commands;

public class StoryFollowUpCommand(IOptions<BotOptions> options) : IFollowUpCommand
{
    private readonly BotOptions _options = options.Value;

    public Type FollowAfterCommand => typeof(StoryCommand);

    public async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        // TODO: #1 Save the reply

        // TODO: #2 Send follow-up to user

        await client.SendMessage(
            chatId: message.Chat.Id,
            text: "Your question has been successfully delivered to the admins.");

        // TODO: #3 Format & Send the message to a group chat

        var groupMessage = $"""
                From: @{message.From.Username}
                Date: {Core.TimeProvider.ConvertToRigaTime(DateTime.UtcNow)}
                Message: {message.Text}
                """;

        await client.SendMessage(
            chatId: _options.AdminGroupChatId,
            text: groupMessage);

        // TODO: Pass as argument or 
        //userStateManager.RemoveUserState(message.From.Id);
    }
}

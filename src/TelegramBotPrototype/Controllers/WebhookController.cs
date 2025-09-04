using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotPrototype.SDK.Interfaces;
using TelegramBotPrototype.SDK.Options;

namespace TelegramBotPrototype.Controllers;

[ApiController]
[Route("[controller]")]
public class WebhookController(
    IBotUpdateHandler handler,
    ITelegramBotClient client,
    IOptions<TelegramApiOptions> options,
    ILogger<WebhookController> logger) : ControllerBase
{
    private readonly IBotUpdateHandler _handler = handler;
    private readonly ITelegramBotClient _client = client;
    private readonly ILogger<WebhookController> _logger = logger;

    private readonly TelegramApiOptions _options = options.Value;

    [HttpGet("setWebhook")]
    public async Task<IActionResult> SetWebhook(CancellationToken ct)
    {
        await _client.SetWebhook(_options.WebhookUrl, allowedUpdates: [], secretToken: _options.ApiSecret, cancellationToken: ct);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken ct)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != _options.ApiSecret)
            return BadRequest();

        await _handler.HandleAsync(update, ct);

        return Ok();
    }
}

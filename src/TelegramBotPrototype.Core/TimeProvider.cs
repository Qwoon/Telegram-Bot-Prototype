using System;
using System.Runtime.InteropServices;

namespace TelegramBotPrototype.Core;

public static class TimeProvider
{
    private static readonly TimeZoneInfo _timeZoneInRiga = TimeZoneInfo.FindSystemTimeZoneById(RigaTimeZoneId);

    public static string RigaTimeZoneId => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? "E. Europe Standard Time"
        : "Europe/Riga";

    public static DateTime GetTimeInRiga() => TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow, _timeZoneInRiga);
    public static DateTime ConvertToRigaTime(DateTime time) => TimeZoneInfo.ConvertTimeFromUtc(time, _timeZoneInRiga);
}

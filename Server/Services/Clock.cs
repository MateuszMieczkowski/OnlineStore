﻿namespace OnlineStore.Server.Services;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public class Clock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
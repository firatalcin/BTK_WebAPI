﻿using NLog;
using Services.Contracts;

namespace Services.Managers;

public class LoggerManager : ILoggerService
{
    private static ILogger _logger = LogManager.GetCurrentClassLogger();
    
    public void LogInfo(string message)
    {
        _logger.Info(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warn(message);
    }

    public void LogError(string message)
    {
        _logger.Error(message);
    }

    public void LogDebug(string message)
    {
        _logger.Debug(message);
    }
}
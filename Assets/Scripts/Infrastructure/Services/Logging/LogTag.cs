using System;

namespace Infrastructure.Services.Logging
{
    [Flags]
    public enum LogTag
    {
        GameLoopStateMachine = 1 << 0,
        RemoteSettings = 1 << 1,
        Analytics = 1 << 2,
        SceneLoader = 1 << 3,
        UI = 1 << 4,
        SaveService = 1 << 5,
        PlayerData = 1 << 6,
        ResourceService = 1 << 7
    }

    public static class LogTagExtensions
    {
        public static bool HasFlagFast(this LogTag value, LogTag flag)
        {
            return (value & flag) != 0;
        }
    }
}
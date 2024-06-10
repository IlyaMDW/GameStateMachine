using System;
using UnityEngine;

namespace Infrastructure.Services.Logging
{
    [Serializable]
    public class UnityConditionalLoggingService : IConditionalLoggingService
    {
        [SerializeField] private LogTag _enabledLogs;

        protected override void InternalLog(string text, LogTag tag)
        {
            if (_enabledLogs.HasFlagFast(tag))
            {
                Debug.LogFormat("[{0}] {1}", tag, text);
            }
        }

        protected override void InternalLogWarning(string text, LogTag tag)
        {
            if (_enabledLogs.HasFlagFast(tag))
            {
                Debug.LogWarningFormat("[{0}] {1}", tag, text);
            }
        }

        protected override void InternalLogError(string text, LogTag tag)
        {
            if (_enabledLogs.HasFlagFast(tag))
            {
                Debug.LogErrorFormat("[{0}] {1}", tag, text);
            }
        }
    }
}
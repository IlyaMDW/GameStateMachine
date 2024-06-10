using System;
using UnityEngine;

namespace Utils.MonoBehaviours
{
    public class UnityCallbacksRetranslator : MonoBehaviour
    {
        public static Action ApplicationQuit;
        public static Action<bool> ApplicationPause;
        public static Action<bool> ApplicationFocus;
        
        private void OnApplicationQuit()
        {
            ApplicationQuit?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            ApplicationPause?.Invoke(pauseStatus);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            ApplicationFocus?.Invoke(hasFocus);
        }
    }
}
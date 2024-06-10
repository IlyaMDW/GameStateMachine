using Infrastructure.Services.Saving;
using IngameDebugConsole;
using UnityEngine;
using Zenject;

namespace RuntimeDebug.ConsoleMethods
{
    public static class Tools
    {
        public static bool IsDebug
        {
            get { return Application.version[0] == 'd'; }
        }

        [ConsoleMethod("remove-saves", "removes save file"), UnityEngine.Scripting.Preserve]
        public static void RemoveSaves()
        {
            UnityEngine.Object.FindObjectOfType<ProjectContext>().Container.Resolve<ISaveService>().RemoveDefaultSaveFile();
        }
    }
}
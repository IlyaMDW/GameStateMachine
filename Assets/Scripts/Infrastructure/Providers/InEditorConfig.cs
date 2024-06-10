using System;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class InEditorConfig
    {
        public static InEditorConfig Instance { get; private set; }

        public void RegisterSingleton()
        {
            if (Instance != null) return;
            Instance = this;
        }

        [field: SerializeField] public bool SkipMenu { get; private set; }
    }
}
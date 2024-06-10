using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TriInspector;
using UnityEngine;

namespace UI.SpriteLibrary
{
    [CreateAssetMenu]
    public class SpriteLibrary : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<TextIconType, TextIconAssetsContainer> _textIcons;

        public IDictionary<TextIconType, TextIconAssetsContainer> TextIcons => _textIcons;

        [RuntimeInitializeOnLoadMethod]
        private static void Validate()
        {
            var spriteLibraries = Resources.FindObjectsOfTypeAll<SpriteLibrary>();

            foreach (var spriteLibrary in spriteLibraries)
            {
                spriteLibrary.OnValidate();
            }
        }

        public void OnValidate()
        {
            foreach (var value in Enum.GetValues(typeof(TextIconType)).Cast<TextIconType>())
            {
                if (!_textIcons.ContainsKey(value))
                {
                    Debug.LogError($"Fill containers with new resource types! ResourceType is missing: {value}");
                }
            }
        }

        [Button]
        private void FillWithNewTypes()
        {
            foreach (var value in Enum.GetValues(typeof(TextIconType)).Cast<TextIconType>())
            {
                _textIcons.TryAdd(value, default);
            }
        }
    }
}
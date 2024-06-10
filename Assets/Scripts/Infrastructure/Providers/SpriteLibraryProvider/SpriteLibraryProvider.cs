using System;
using UI.SpriteLibrary;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class SpriteLibraryProvider : ISpriteLibraryProvider
    {
        public string DiamondsIcon => SpriteLibrary.TextIcons[TextIconType.Diamonds].Icon;
        public string GoldIcon => SpriteLibrary.TextIcons[TextIconType.Gold].Icon;

        [field: SerializeField] public SpriteLibrary SpriteLibrary { get; private set; }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace UI.SpriteLibrary
{
    [Serializable]
    public struct TextIconAssetsContainer
    {
        [SerializeField] private TMP_SpriteAsset _spriteAsset;
        public string Icon => $"<sprite name={_spriteAsset.spriteCharacterTable[0].name}>";
        public Sprite Sprite => _spriteAsset.spriteGlyphTable[0].sprite;
    }
}
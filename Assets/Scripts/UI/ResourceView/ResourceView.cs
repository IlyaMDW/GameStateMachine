using System;
using Infrastructure;
using Infrastructure.Providers;
using Infrastructure.Services.ResourceService;
using TMPro;
using UI.SpriteLibrary;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using StringExtensions = Utils.Extensions.StringExtensions;

namespace UI.ResourceView
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] protected TextIconType _type;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _resourceSprite;
        private IResourceService _resourceService;
        private ISpriteLibraryProvider _spriteLibraryProvider;

        [Inject]
        private void Inject(IResourceService resourceService, ISpriteLibraryProvider spriteLibraryProvider)
        {
            _spriteLibraryProvider = spriteLibraryProvider;
            _resourceService = resourceService;
        }

        private void OnEnable()
        {
            if (!EntryPoint.IsAwakened) return;


            switch (_type)
            {
                case TextIconType.Gold:
                    OnResourceUpdated(_resourceService.Gold);
                    _resourceService.OnGoldUpdated += OnResourceUpdated;
                    break;
                case TextIconType.Diamonds:
                    OnResourceUpdated(_resourceService.Diamonds);
                    _resourceService.OnDiamondsUpdated += OnResourceUpdated;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            _resourceService.OnGoldUpdated -= OnResourceUpdated;
            _resourceService.OnDiamondsUpdated -= OnResourceUpdated;
        }

        protected virtual void OnResourceUpdated(long newValue)
        {
            _text.text = StringExtensions.GetAdaptedInt((ulong)newValue);
        }
    }
}
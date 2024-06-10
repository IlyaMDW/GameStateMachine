using Configs;
using UI.SpriteLibrary;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResourceView
{
    public class LimitedResourceView : ResourceView
    {
        [SerializeField] private Slider _slider;

        protected override void OnResourceUpdated(long newValue)
        {
            base.OnResourceUpdated(newValue);

            switch (_type)
            {
                case TextIconType.Gold:
                    _slider.maxValue = Remote.ResourceServiceConfig.DefaultMaxGold;
                    break;
            }

            _slider.value = newValue;
        }
    }
}
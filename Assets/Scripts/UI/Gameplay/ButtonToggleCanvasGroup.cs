using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Vector2 = UnityEngine.Vector2;

namespace UI.Gameplay
{
    public class ButtonToggleCanvasGroup : MonoBehaviour
    {
        public event Action<Button> OnButtonClicked;

        [SerializeField] private SerializedDictionary<Button, List<Canvas>> _buttonsAndRoutedCanvases;
        [HideInInspector] [SerializeField] private SerializedDictionary<Button, RectTransform> _cachedTransforms;
        [SerializeField] private Button _default;
        [SerializeField] private float _yPosOnClick = -185f;
        [SerializeField] private float _yPosByDefault = -125.5f;

        private int _resolveCallsCounter;
        private Button _cachedThisButton;

        private IEnumerator Start()
        {
            yield return null;
            
            foreach (var button in _buttonsAndRoutedCanvases.Keys)
            {
                _buttonsAndRoutedCanvases.Keys.Where(x => x != button)
                    .ForEach(otherButton => button.onClick.AddListener(() => ResolveClick(otherButton, false)));

                button.onClick.AddListener(() => ResolveClick(button, true));
            }

            _default.onClick.Invoke();
        }

        public void DisableAll()
        {
            //_buttonsAndRoutedCanvases.Keys.ForEach(x => x.interactable = false);
        }

        public void EnableAll()
        {
            _buttonsAndRoutedCanvases.Keys.ForEach(x => x.interactable = true);
        }

        private void ResolveClick(Button button, bool isThis)
        {
            _resolveCallsCounter++;

            if (isThis)
            {
                _cachedThisButton = button;
            }
            else
            {
                ProcessOtherButton(button);
            }

            if (_resolveCallsCounter == _buttonsAndRoutedCanvases.Count)
            {
                ProcessThisButton(_cachedThisButton);
                OnButtonClicked?.Invoke(_cachedThisButton);
                _resolveCallsCounter = 0;
                _cachedThisButton = null;
            }
        }

        private void ProcessThisButton(Button button)
        {
            _buttonsAndRoutedCanvases[button].ForEach(canvas => { canvas.gameObject.SetActive(true); });
            _cachedTransforms[button].anchoredPosition = new Vector2(_cachedTransforms[button].anchoredPosition.x, _yPosOnClick);
            //EnableAll();
            button.interactable = false;
        }

        private void ProcessOtherButton(Button button)
        {
            _cachedTransforms[button].anchoredPosition = new Vector2(_cachedTransforms[button].anchoredPosition.x, _yPosByDefault);
            _buttonsAndRoutedCanvases[button].ForEach(otherCanvas => { otherCanvas.gameObject.SetActive(false); });
            button.interactable = true;
        }


        [Button]
        private void AddButtonsToListFromChildrens()
        {
            var buttons = GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                _buttonsAndRoutedCanvases.TryAdd(button, null);
                if (!_cachedTransforms.TryAdd(button, button.GetComponent<RectTransform>()))
                {
                    _cachedTransforms[button] = button.GetComponent<RectTransform>();
                }
            }
        }
    }
}
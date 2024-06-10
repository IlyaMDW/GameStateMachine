using UnityEngine;
using UnityEngine.UI;

namespace Utils.MonoBehaviours
{
    /// <summary>
    ///     Marker component
    /// </summary>
    [DisallowMultipleComponent]
    public class GraphicRequester : MonoBehaviour
    {
        [field: SerializeField] public bool RaycastTarget;
        [field: SerializeField] public bool Maskable;
        [field: SerializeField] public bool OverrideAllChildrensMasksToThis;

        private void OnValidate()
        {
            if (!TryGetComponent<MaskableGraphic>(out var x))
            {
                return;
            }

            x.maskable = Maskable;
            x.raycastTarget = RaycastTarget;
        }
    }
}
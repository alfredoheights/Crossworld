using UnityEngine;

namespace Xyz.MomsSpaghettiCode.UI.ScriptableObjects
{
    [CreateAssetMenu(menuName = "UI/Popup", fileName = "Popup")]
    public class PopupScriptableObject : ScriptableObject
    {
        public bool xButtonEnabled = true;

        public string titleText;
        public float popupWidth = 128f;

        [HideInInspector]
        public bool dirty = true;

        public void CleanUpThatDirt()
        {
            dirty = false;
        }
        private void OnValidate()
        {
            dirty = true;
        }
    }
}
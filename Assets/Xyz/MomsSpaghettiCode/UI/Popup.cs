using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    /**
     * Reusable popup script and scriptable object.
     *
     * Create a prefab and add this to the script. Give it references to the sections and prefabs you
     * will use for the pieces of the window.
     */
    [ExecuteAlways]
    public class Popup : MonoBehaviour
    {
        #region Serialized UI Elements

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button xButton;
        [SerializeField] private RectTransform contentSection;

        [SerializeField] private float minimumHeight = 60f;

        [SerializeField] private PopupScriptableObject popupScriptableObject;

        // How should I do the children?
        [SerializeField] private GameObject childContainer;

        #endregion

        private void Awake()
        {
            UpdateUIValues();
        }

        private void Update()
        {
            if (popupScriptableObject is null) return;
            if (popupScriptableObject.dirty)
            {
                UpdateUIValues();
            }
        }

        private void UpdateUIValues()
        {
            if (popupScriptableObject is null) return;
            
            popupScriptableObject.CleanUpThatDirt();

            xButton.gameObject.SetActive(popupScriptableObject.xButtonEnabled);
            titleText.text = popupScriptableObject.titleText;

            RectTransform rectTransform = (RectTransform) transform;
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                popupScriptableObject.popupWidth
            );
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                minimumHeight + contentSection.sizeDelta.y
            );
        }
    }
}
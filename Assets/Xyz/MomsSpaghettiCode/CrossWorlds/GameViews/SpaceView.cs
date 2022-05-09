using UnityEngine;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class SpaceView : DroppableSpace
    {
        public Color highlightColor = Color.clear;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            highlightColor = Color.white;
            UpdateHighlightColor();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            highlightColor = Color.clear;
            UpdateHighlightColor();
        }

        private void UpdateHighlightColor()
        {
            // Make the actual change that makes the highlight color happen
        }

    }
}
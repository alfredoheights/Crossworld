using UnityEngine;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class SpaceView : DroppableSpace
    {
        public Color highlightColor = Color.clear;

        public new void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            highlightColor = Color.white;
            UpdateHighlightColor();
        }
        public new void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            highlightColor = Color.clear;
            UpdateHighlightColor();
        }

        private void UpdateHighlightColor()
        {
            // Make the actual change that makes the highlight color happen
        }


        private new void AnyPiecePickedUp(DraggablePiece piece)
        {
            // todo: make sure this actually gets called, idk how OOP works
            base.AnyPiecePickedUp(piece);
            Debug.Log("SpaceView: piece picked up");
        }
    }
}
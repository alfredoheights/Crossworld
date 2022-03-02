using UnityEngine;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.UI;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class HandView : DroppableSpace
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        private void UpdateHighlightColor()
        {
            // Make the actual change that makes the highlight color happen
        }


        protected override void AnyPiecePickedUp(DraggablePiece piece)
        {
            base.AnyPiecePickedUp(piece);
            Debug.Log("SpaceView: piece picked up");
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    public class DroppableSpace : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PiecePickupEventScriptableObject piecePickupEventScriptableObject;

        private void OnEnable()
        {
            // Subscribe to all piece pickup/drop events. The piece can change style
            // depending on the child
            piecePickupEventScriptableObject.piecePickupEvent.AddListener(AnyPiecePickedUp);
            piecePickupEventScriptableObject.pieceDropEvent.AddListener(AnyPieceDropped);
        }

        private void OnDisable()
        {
            piecePickupEventScriptableObject.piecePickupEvent.RemoveListener(AnyPiecePickedUp);
            piecePickupEventScriptableObject.pieceDropEvent.RemoveListener(AnyPieceDropped);
        }

        protected virtual void AnyPiecePickedUp(DraggablePiece piece)
        {
            // 
        }

        protected virtual void AnyPieceDropped(DraggablePiece piece)
        {
            //
        }
        

        public virtual void OnDrop(PointerEventData eventData)
        {
            // If the dropped object is a DroppablePiece
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            // If there is a DraggablePiece being dragged above this when it's hovered, interact
            if (piecePickupEventScriptableObject.pieceInMotion != null)
            {
                piecePickupEventScriptableObject.pieceInMotion.MeetYourFosterParents(transform);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            // If a DraggablePiece was dragged out, interact
            if (piecePickupEventScriptableObject.pieceInMotion != null)
            {
                piecePickupEventScriptableObject.pieceInMotion.AccidentallyKillYourFosterParentsDog();
            }
        }
    }
}
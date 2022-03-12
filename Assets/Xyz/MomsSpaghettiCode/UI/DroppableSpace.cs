using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    public class DroppableSpace : 
        MonoBehaviour,
        IDropHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [FormerlySerializedAs("piecePickupEventScriptableObject")] 
        [SerializeField] protected PieceDragEventScriptableObject pieceDragEventScriptableObject;

        // Use this id to tell the game state about changes within events.
        public int gameStateReferenceId;

        private void OnEnable()
        {
            // Subscribe to all piece pickup/drop events. The piece can change style
            // depending on the child
            pieceDragEventScriptableObject.piecePickupEvent.AddListener(PiecePickedUp);
            pieceDragEventScriptableObject.pieceDropEvent.AddListener(PieceDropped);
        }

        private void OnDisable()
        {
            pieceDragEventScriptableObject.piecePickupEvent.RemoveListener(PiecePickedUp);
            pieceDragEventScriptableObject.pieceDropEvent.RemoveListener(PieceDropped);
        }

        protected virtual void PiecePickedUp(DraggablePiece piece)
        {
            // 
        }

        protected virtual void PieceDropped(DraggablePiece piece)
        {
            //
        }
        

        public virtual void OnDrop(PointerEventData eventData)
        {
            // If the dropped object is a DroppablePiece
            Debug.Log("HI");
        }

        protected virtual void OnHoverEnter(PointerEventData eventData)
        {
            pieceDragEventScriptableObject.EnqueueParent(this);
        }

        protected virtual void OnHoverExit(PointerEventData eventData)
        {
            pieceDragEventScriptableObject.DequeueParent(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            // If there is a DraggablePiece being dragged above this when it's hovered, interact
            if (pieceDragEventScriptableObject.pieceInMotion != null)
            {
                OnHoverEnter(eventData);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            // If a DraggablePiece was dragged out, interact
            if (pieceDragEventScriptableObject.pieceInMotion != null)
            {
                OnHoverExit(eventData);
            }
        }
    }
}
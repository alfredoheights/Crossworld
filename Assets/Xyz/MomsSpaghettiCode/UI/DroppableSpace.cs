using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.UI.Model;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    public class DroppableSpace :
        MonoBehaviour,
        IDropHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [FormerlySerializedAs("piecePickupEventScriptableObject")] [SerializeField]
        protected PieceDragEventScriptableObject pieceDragEventScriptableObject;

        // Use this id to tell the game state about changes within events.
        public int gameStateReferenceId;

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        protected void SubscribeToEvents()
        {
            // Subscribe to all piece pickup/drop events. The space can highlight depending on if it's a valid
            // drop target.
            try
            {
                pieceDragEventScriptableObject.piecePickupEvent.AddListener(AnyPiecePickedUp);
                pieceDragEventScriptableObject.pieceDropEvent.AddListener(AnyPieceDropped);
                pieceDragEventScriptableObject.pieceMoveEvent.AddListener(AnyPieceMoved);
                pieceDragEventScriptableObject.pieceRemovedEvent.AddListener(AnyPieceRemoved);
            }
            catch (Exception)
            {
                Debug.Log("GOTTEM");
                throw;
            }
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        protected void UnsubscribeFromEvents()
        {
            pieceDragEventScriptableObject.piecePickupEvent.RemoveListener(AnyPiecePickedUp);
            pieceDragEventScriptableObject.pieceDropEvent.RemoveListener(AnyPieceDropped);
            pieceDragEventScriptableObject.pieceMoveEvent.RemoveListener(AnyPieceMoved);
            pieceDragEventScriptableObject.pieceRemovedEvent.RemoveListener(AnyPieceRemoved);
        }

        protected virtual void AnyPiecePickedUp(DraggablePiece piece)
        {
            // Triggered any time ANY piece is picked up
        }

        protected virtual void AnyPieceDropped(DraggablePiece piece)
        {
            // Triggered any time ANY piece is dropped
        }

        private void AnyPieceMoved(DraggablePiece piece, DroppableSpace space)
        {
            if (ReferenceEquals(space, this))
            {
                space.PieceMovedHere(piece);
            }
        }

        protected virtual void PieceMovedHere(DraggablePiece piece)
        {
            // Called by `AnyPieceMoved` when this is the target space
        }

        private void AnyPieceRemoved(DraggablePiece piece, DroppableSpace space)
        {
            if (ReferenceEquals(space, this))
            {
                space.PieceRemoved(piece);
            }
        }

        protected virtual void PieceRemoved(DraggablePiece piece)
        {
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            // If the dropped object is a DroppablePiece
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

        public virtual bool IsValidTarget(DraggablePiece piece)
        {
            return false;
        }
    }
}
using System;
using UnityEngine;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameViews;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI
{
    public class MovingPiecePlaceholder : MonoBehaviour
    {
        /*
         * When a user picks up a DraggablePiece, it becomes a child of this game object.
         *
         * This object is tagged as "MovingPiecePlaceholder" and can be accessed by tag.
         *
         * The events here alert when this adopts a piece.
         */
        [SerializeField] private PiecePickupEventScriptableObject piecePickupEventScriptableObject;

        private DraggablePiece _childPiece;

        public void OnEnable()
        {
            piecePickupEventScriptableObject.piecePickupEvent.AddListener(FosterPiece);
            piecePickupEventScriptableObject.pieceDropEvent.AddListener(TakePieceToTheOrphanage);
        }

        public void OnDisable()
        {
            piecePickupEventScriptableObject.piecePickupEvent.RemoveListener(FosterPiece);
        }

        private void FosterPiece(DraggablePiece piece)
        {
            piece.EnterTheFosterSystem(transform);
            _childPiece = piece;
        }

        private void TakePieceToTheOrphanage(DraggablePiece piece)
        {
            if (piece is null) return;
            piece.CryOnTheOrphanageDoorstep();
            _childPiece = null;
        }
    }
}

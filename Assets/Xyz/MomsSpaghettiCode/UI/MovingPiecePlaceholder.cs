using System;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("piecePickupEventScriptableObject")] [SerializeField] private PieceDragEventScriptableObject pieceDragEventScriptableObject;

        private DraggablePiece _childPiece;

        public void OnEnable()
        {
            pieceDragEventScriptableObject.piecePickupEvent.AddListener(FosterPiece);
        }

        public void OnDisable()
        {
            pieceDragEventScriptableObject.piecePickupEvent.RemoveListener(FosterPiece);
        }

        private void FosterPiece(DraggablePiece piece)
        {
            // TODO: Can I make this an event too?
            piece.UsePlaceholderParent(transform);
        }
    }
}

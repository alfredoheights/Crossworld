using UnityEngine;
using UnityEngine.Events;

namespace Xyz.MomsSpaghettiCode.UI.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Piece Drag Event",
        menuName = "Events / Piece Drag Event")]
    public class PieceDragEventScriptableObject : ScriptableObject
    {
        [SerializeField] public DraggablePiece pieceInMotion;

        /*
         * These events are for the actual drag/drop functions. They need to check for validity
         * before invoking the piece move events.
         */
        [System.NonSerialized] public UnityEvent<DraggablePiece> piecePickupEvent;
        [System.NonSerialized] public UnityEvent<DraggablePiece> pieceDropEvent;
        
        /*
         * When a piece move is valid, use this to broadcast that actual change so the game state
         * can update with it. This event can be invoked directly.
         */
        [System.NonSerialized] public UnityEvent<DraggablePiece, DroppableSpace> pieceMoveEvent;
        [System.NonSerialized] public UnityEvent<DraggablePiece, DroppableSpace> pieceRemovedEvent;

        /*
         * When a piece is dragged over a space, enqueue that space as a drop target.
         */
        [System.NonSerialized] public UnityEvent<DraggablePiece, DroppableSpace> parentEnqueueEvent;
        [System.NonSerialized] public UnityEvent<DraggablePiece, DroppableSpace> parentDequeueEvent;

        private void OnEnable()
        {
            piecePickupEvent ??= new UnityEvent<DraggablePiece>();
            pieceDropEvent ??= new UnityEvent<DraggablePiece>();
            pieceMoveEvent ??= new UnityEvent<DraggablePiece, DroppableSpace>();
            pieceRemovedEvent ??= new UnityEvent<DraggablePiece, DroppableSpace>();
            parentEnqueueEvent ??= new UnityEvent<DraggablePiece, DroppableSpace>();
            parentDequeueEvent ??= new UnityEvent<DraggablePiece, DroppableSpace>();
        }

        /*
         * We need public methods for these events because it needs to set/unset
         * pieceInMotion at the right time so pickup/drop event listeners can use them.
         */
        public void PickUpPiece(DraggablePiece piece)
        {
            pieceInMotion = piece;
            piecePickupEvent.Invoke(piece);
        }

        public void DropPiece()
        {
            pieceDropEvent.Invoke(pieceInMotion);
            pieceInMotion = null;
        }
    

        /*
         * The public methods for these call the action with the current piece in motion.
         * Invoking the actions directly would require a reference to the piece.
         */
        public void EnqueueParent(DroppableSpace droppableParent)
        {
            parentEnqueueEvent.Invoke(pieceInMotion, droppableParent);
        }
        public void DequeueParent(DroppableSpace droppableParent)
        {
            parentDequeueEvent.Invoke(pieceInMotion, droppableParent);
        }
    }
}
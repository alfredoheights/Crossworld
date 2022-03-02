using UnityEngine;
using UnityEngine.Events;

namespace Xyz.MomsSpaghettiCode.UI.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Piece Pickup Event",
        menuName = "Events / Piece Pickup Event")]
    public class PiecePickupEventScriptableObject : ScriptableObject
    {
        [SerializeField] public DraggablePiece pieceInMotion;
        [System.NonSerialized] public UnityEvent<DraggablePiece> piecePickupEvent;
        [System.NonSerialized] public UnityEvent<DraggablePiece> pieceDropEvent;

        private void OnEnable()
        {
            piecePickupEvent ??= new UnityEvent<DraggablePiece>();
            pieceDropEvent ??= new UnityEvent<DraggablePiece>();
        }

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
    }
}
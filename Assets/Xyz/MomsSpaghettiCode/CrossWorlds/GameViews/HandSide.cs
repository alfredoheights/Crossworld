using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.UI;
using Xyz.MomsSpaghettiCode.UI.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class HandSide : SpaceView
    {
        [System.NonSerialized] public UnityEvent<DraggablePiece> pieceDroppedOnHandSideEvent;

        public void Awake()
        {
            pieceDroppedOnHandSideEvent ??= new UnityEvent<DraggablePiece>();
            pieceDragEventScriptableObject.pieceMoveEvent.AddListener(DropPieceOnThisSide);
        }
        private void DropPieceOnThisSide(DraggablePiece piece, DroppableSpace space)
        {
            if (!ReferenceEquals(space, this))
            {
                return;
            }
            pieceDroppedOnHandSideEvent.Invoke(piece);
        }
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects;
using Xyz.MomsSpaghettiCode.UI;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public class BoardSpaceView : SpaceView
    {
        public int X;
        public int Y;

        public PlayerStateScriptableObject playerStateScriptableObject;
        
        public void OnEnable()
        {
            SubscribeToEvents();
        }
        
        public void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        protected override void PieceMovedHere(DraggablePiece piece)
        {
            base.PieceMovedHere(piece);
            GamePiece targetGamePiece = ((PieceView) piece).gamePiece;
            playerStateScriptableObject.board.SetAtIndex(X, Y, targetGamePiece);
        }

        public override bool IsValidTarget(DraggablePiece piece)
        {
            GamePiece? pieceAtIndex = playerStateScriptableObject.board.GetAtIndex(X, Y);
            return pieceAtIndex is null;
        }
    }
}
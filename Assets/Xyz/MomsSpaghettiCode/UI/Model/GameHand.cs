using System.Collections.Generic;
using UnityEngine;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI.Model
{
    public struct GameHand
    {
        public static GameHand CreateInstance()
        {
            return new GameHand
            {
                Pieces = new List<GamePiece>(),
            };
        }

        private List<GamePiece> Pieces { get; set; }

        [SerializeField] private PlayerStateScriptableObject playerStateRef;

        public GameHand(List<GamePiece> pieces, PlayerStateScriptableObject playerState)
        {
            if (pieces is null)
            {
                pieces = new List<GamePiece>();
            }
            Pieces = pieces;
            playerStateRef = playerState;
        }

        public void AddToHand(IEnumerable<GamePiece> newPieces)
        {
            Pieces.AddRange(newPieces);
        }

        public void RemoveFromHand(int index)
        {
            RemoveFromHand(Pieces[index]);
        }
        public void RemoveFromHand(GamePiece piece)
        {
            Pieces.Remove(piece);
        }

        // Function to trigger a player game state Hit event

        // Function to trigger a player game state Redraw event

        // Subscribe to play

    }
}
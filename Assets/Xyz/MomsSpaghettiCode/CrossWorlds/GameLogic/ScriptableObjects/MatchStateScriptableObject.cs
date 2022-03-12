using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game State / Match State", order = 1)]
    public class MatchStateScriptableObject : ScriptableObject
    {
        // Holds the global state for a game. Includes references to the player states for all players, and global
        // game state things like the bag.

        // Has events that are triggered by every change to the players' game state, as well as administrative
        // events like players joining, leaving, or being eliminated.

        // For each player state, subscribe to its events that other players should see.
        //  - Submit
        //  - Revoke
        //  - Hit / Stud
        //  - Spells (if, you know, we ever get to those)
        private List<PlayerStateScriptableObject> Players { get; set; }
        private GameBag _gameBag;

        public PieceDistributionScriptableObject pieceDistributionScriptableObject;

        [System.NonSerialized] public UnityEvent<PlayerStateScriptableObject> playerJoined;
        [System.NonSerialized] public UnityEvent<PlayerStateScriptableObject> playerLeft;
        [System.NonSerialized] public UnityEvent<PlayerStateScriptableObject> playerMadeMove;
        [System.NonSerialized] public UnityEvent matchStateChanged;
        
        // Specific match state change events
        [System.NonSerialized] public UnityEvent<int, List<GamePiece>> newHit;

        [System.NonSerialized] public UnityEvent<List<GamePiece>> hitPieceResponseEvent;

        private void OnEnable()
        {
            if (_gameBag.piecesRemaining() == 0)
            {
                _gameBag = GameBag.CreateInstance();
                _gameBag.FillBag(pieceDistributionScriptableObject);
            }

            playerJoined ??= new UnityEvent<PlayerStateScriptableObject>();
            playerLeft ??= new UnityEvent<PlayerStateScriptableObject>();
            playerMadeMove ??= new UnityEvent<PlayerStateScriptableObject>();
            matchStateChanged ??= new UnityEvent();

            newHit ??= new UnityEvent<int, List<GamePiece>>();

            hitPieceResponseEvent ??= new UnityEvent<List<GamePiece>>();

            // foreach (PlayerStateScriptableObject playerStateScriptableObject in Players)
            // {
            // }
        }

        private void OnDestroy()
        {
            foreach (PlayerStateScriptableObject playerStateScriptableObject in Players)
            {
                // playerStateScriptableObject.hitPieceRequestEvent.RemoveListener(PlayerRequestedHit);
            }
        }

        public List<GamePiece> RequestHit(int id, int count)
        {
            // Draw {count} pieces out of bag, return to player
            if (_gameBag.piecesRemaining() < count) return new List<GamePiece> { };

            return _gameBag.Hit(count);
        }

        // Whenever a player submits, if they are then out of pieces, we need to hit all players.
        
    }
}
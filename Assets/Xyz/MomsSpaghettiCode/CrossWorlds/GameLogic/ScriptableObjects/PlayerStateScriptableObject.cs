using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game State / Player State", order = 2)]
    public class PlayerStateScriptableObject : ScriptableObject
    {
        // Player game state held here for UI to subscribe to.
        // The board, hand, score, time remaining, etc are held here.
        // Also, there are events for each piece of this puzzle. If a UI element wants to subscribe to a player's
        // events, they reference something that has created this class.
        public GameHand hand;
        public GameBoard board;
        public int id;

        // Reference to the game state so we can call events to it
        public MatchStateScriptableObject matchState;

        // Will be created locally for one player, but generated programmatically for ai and network opponents
        // This will let us make tests that demonstrate rendering own/opponent boards based on different states.
        public UnityEvent<List<GamePiece>> newHitEvent;
        public UnityEvent<GamePiece> pieceMovedEvent;

        private void OnEnable()
        {
            newHitEvent = new UnityEvent<List<GamePiece>>();
            hand = GameHand.CreateInstance();
            board = GameBoard.CreateInstance();
        }

        public void Hit(int count = 3)
        {
            List<GamePiece> pieces = matchState.RequestHit(id, count);
            hand.AddToHand(pieces);
            newHitEvent.Invoke(pieces);
        }
    }
}
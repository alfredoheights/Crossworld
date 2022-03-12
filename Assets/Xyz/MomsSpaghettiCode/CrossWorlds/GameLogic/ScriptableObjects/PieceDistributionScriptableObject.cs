using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameViews;
using Xyz.MomsSpaghettiCode.UI.Model;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game Logic / Piece Distribution")]
    public class PieceDistributionScriptableObject : ScriptableObject
    {
        public List<LetterPiece> pieces;

        public LetterPiece GetPieceByLetter(char letter)
        {
            foreach (LetterPiece letterPiece in pieces)
            {
                if (letterPiece.letter == letter) return letterPiece;
            }

            throw new Exception("letter not found in alphabet");
        }
    }
    [System.Serializable]
    public class LetterPiece
    {
        public char letter;
        public int points;
        public int count;

        public override string ToString()
        {
            return $"LetterPiece - Letter: {letter}, Points: {points}";
        }
    }
}
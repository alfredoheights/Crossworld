using UnityEngine;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game Logic / Match Settings")]
    public class MatchSettingsScriptableObject : ScriptableObject
    {
        // Settings used for an individual match.
        // This includes piece distribution, time limits, player count limits, AI difficulty range
        
        // How many of each letter, and how much each is worth.
        // Default options are inspired by classic games of this genre.
        public PieceDistributionScriptableObject pieceDistribution;

        
        
        public enum GameType
        {
            Classic,
            Pressure,
        }
    }
}
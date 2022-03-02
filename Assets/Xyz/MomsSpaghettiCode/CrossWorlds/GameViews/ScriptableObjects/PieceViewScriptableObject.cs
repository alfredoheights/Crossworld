using UnityEngine;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews.ScriptableObjects
{
    /*
     * For visual style of piece views.
     */
    [CreateAssetMenu(menuName = "Game Views/Piece View", fileName = "PieceView")]
    public class PieceViewScriptableObject : ScriptableObject
    {
        // May add color, texture, etc here.

        public Vector3 dragScale = new Vector3(1.3f, 1.3f, 1f);
        public Vector3 dragOffset = new Vector3(0f, 10f, 0f);
        [Range(0f, 1f)] public float dragOpacity = .5f;
    }
}
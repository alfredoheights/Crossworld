using System.Collections.Generic;

namespace Xyz.MomsSpaghettiCode.UI.Model
{
    public struct GameBoard
    {
        public static GameBoard CreateInstance()
        {
            return new GameBoard
            {
                pieces = new Dictionary<(int, int), GamePiece>(),
                pieceCoordinateReference = new Dictionary<int, (int, int)>()
            };
        }

        public Dictionary<(int, int), GamePiece> pieces;
        public Dictionary<int, (int, int)> pieceCoordinateReference;
        public GamePiece? GetAtIndex(int x, int y)
        {
            return pieces.ContainsKey((x, y)) ? pieces[(x, y)] : (GamePiece?) null;
        }

        public void SetAtIndex(int x, int y, GamePiece piece)
        {
            if (pieceCoordinateReference.ContainsKey(piece.Id))
            {
                // If this piece has been set before, remove it from the list. This unlocks
                // spaces that had values but do not anymore.
                pieces.Remove(pieceCoordinateReference[piece.Id]);
            }
            pieces[(x, y)] = piece;
            pieceCoordinateReference[piece.Id] = (x, y);
        }
    }
}
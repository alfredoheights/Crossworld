using System;
using System.Collections.Generic;
using System.Linq;
using Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.ScriptableObjects;

namespace Xyz.MomsSpaghettiCode.UI.Model
{
    public struct GameBag
    {
        public override string ToString()
        {
            return _pieces.Aggregate(
                "",
                (current, gamePiece) =>
                    current + gamePiece.Letter
            );
        }

        public static GameBag CreateInstance()
        {
            return new GameBag
            {
                _pieces = new List<GamePiece>(),
                _rng = new Random(),
                _iteratedId = 0
            };
        }

        private List<GamePiece> _pieces;
        private Random _rng;
        private int _iteratedId;

        // public GameBag()
        // {
        //     _pieces = new List<GamePiece>();
        //     _rng = new Random();
        //     _iteratedId = 0;
        // }

        // public GameBag(List<GamePiece> pieces, Random rng)
        // {
        //     _pieces = pieces;
        //     _rng = rng ?? new Random();
        //     _iteratedId = 0;
        // }

        public void FillBag(PieceDistributionScriptableObject pieceDistribution)
        {
            foreach (LetterPiece piece in pieceDistribution.pieces)
            {
                for (int i = 0; i < piece.count; i++)
                {
                    _pieces.Add(new GamePiece(_iteratedId++, piece.points, piece.letter));
                }
            }

            _pieces = FisherYatesShuffle(_pieces);
        }

        public int piecesRemaining()
        {
            if (_pieces is null) return 0;
            return _pieces.Count;
        }

        private List<T> FisherYatesShuffle<T>(List<T> deck)
        {
            int n = deck.Count;
            for (int i = 0; i < (n - 1); i++)
            {
                int r = i + _rng.Next(n - i);
                T t = deck[r];
                deck[r] = deck[i];
                deck[i] = t;
            }

            return deck;
        }

        // Take tiles from the bag
        public List<GamePiece> Hit(int count)
        {
            List<GamePiece> returnedPieces = _pieces.GetRange(0, count);
            _pieces.RemoveRange(0, count);
            return returnedPieces;
        }

        public GamePiece Redraw(GamePiece piece)
        {
            _pieces.Add(piece);
            FisherYatesShuffle(_pieces);

            GamePiece hitPiece = _pieces[0];
            _pieces.RemoveAt(0);
            return hitPiece;
        }
    }
}
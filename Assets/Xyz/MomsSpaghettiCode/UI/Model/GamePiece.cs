namespace Xyz.MomsSpaghettiCode.UI.Model
{
    public struct GamePiece
    {
        internal int Id { get; }

        public int Points { get; }

        public char Letter { get; }

        public GamePiece(int id, int points, char letter)
        {
            Id = id;
            Points = points;
            Letter = letter;
        }

        public override string ToString()
        {
            return $"GamePiece - Letter: {Letter}, Points: {Points}";
        }
    }

    public struct GamePieceLocation
    {
        enum LocationType
        {
            bag,
            hand,
            board
        }

        private int playerId { get; }
        private LocationType locationType { get; set; }
        private int x { get; set; }
        private int y { get; set; }
    }
}
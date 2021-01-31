using Xunit;

namespace MyTextEditor.EditorBuffer
{
    public class TestPiece
    {
        [Fact]
        public void Piece_CheckStructure()
        {
            var piece = new Piece(
                PieceType.OriginalText, 0, 10);

            Assert.Equal(PieceType.OriginalText, piece.Type);
            Assert.Equal(0, piece.Offset);
            Assert.Equal(10, piece.Length);
        }

        [Fact]
        public void Piece_CanChackEquality()
        {
            var piece = new Piece(
                PieceType.OriginalText, 0, 5);

            var same = new Piece(
                PieceType.OriginalText, 0, 5);

            var diffType = new Piece(
                PieceType.AddedText, 0, 5);

            var diffOffset = new Piece(
                PieceType.OriginalText, 5, 5);

            var diffLength = new Piece(
                PieceType.OriginalText, 0, 10);

            Assert.True(piece.Equals(same));
            Assert.False(piece.Equals(diffType));
            Assert.False(piece.Equals(diffOffset));
            Assert.False(piece.Equals(diffLength));
        }

        [Fact]
        public void Piece_CanInitialize_WithPiece()
        {
            var piece = new Piece(
                PieceType.OriginalText, 0, 5);

            var another = new Piece(piece);

            Assert.IsType<Piece>(another);
            Assert.Equal(piece, another);
        }
    }
}

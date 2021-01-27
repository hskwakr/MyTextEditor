using System;
using System.Collections.Generic;
using Xunit;

namespace MyTextEditor.EditorBuffer
{
    public class TestPieceTable : TestEditorBufferStub
    {
        [Fact]
        public void PieceTable_CanInitialize()
        {
            var pieceTable = new PieceTable(TextSequence);

            Assert.Equal(TextSequence, pieceTable.ToString());
        }

        [Fact]
        public void PieceTable_PieceCollectionRepresentBuffer()
        {
            var table = new PieceTable(TextSequence);

            Assert.Equal(TextSequence, table.ToString());
        }

        [Fact]
        public void PieceTable_CanSubtractString()
        {
            var table = new PieceTable(TextSequence);
            var pieces = table.GetPieces();

            var minus = new Piece(
                PieceType.OriginalText, 7, 13);

            var actual = table.Subtract(pieces[0], minus);
            var expected = new List<Piece>
            {
                new Piece(PieceType.OriginalText, 0, 7),
                new Piece(PieceType.OriginalText, 20, 4)
            };
            Assert.Equal(expected[0], actual[0]);
            Assert.Equal(expected[1], actual[1]);
        }

        [Fact]
        public void PieceTable_SubtractCanThrow_WithInvalidArgument()
        {
            var table = new PieceTable(TextSequence);
            var pieces = table.GetPieces();

            var fake = new Piece(
                PieceType.OriginalText, 3, 2);

            var minus = new Piece(
                PieceType.OriginalText, 7, 13);

            fake = null;
            Assert.Throws<Exception>(() => table.Subtract(fake, minus));
        }

        [Fact]
        public void PieceTable_SubtractCanThrow_WithDifferentPieceType()
        {

            var table = new PieceTable(TextSequence);
            var pieces = table.GetPieces();

            var fake = new Piece(
                PieceType.AddedText, 0, 24);

            var minus = new Piece(
                PieceType.OriginalText, 7, 13);

            Assert.Throws<Exception>(() => table.Subtract(fake, minus));
        }

        [Fact]
        public void PieceTable_SubtractCanThrow_WithNull()
        {
            var table = new PieceTable(TextSequence);
            var pieces = table.GetPieces();

            var minus = new Piece(
                PieceType.OriginalText, 5, 10);

            Assert.Throws<Exception>(() => table.Subtract(pieces[0], null));
            Assert.Throws<Exception>(() => table.Subtract(null, minus));
        }

        [Fact]
        public void PieceTable_CanRemove_WithOffsetAndLength()
        {
            var table = new PieceTable(TextSequence);
            table.Remove(7, 5);
            Assert.Equal(TextSequence.Remove(7, 5), table.ToString());

            var add = " Hello, nice to meet you, too.";
            table = new PieceTable(TextSequence);
            table.Insert(add);
            table.Remove(20, 25);

            var expected = TextSequence + add;
            Assert.Equal(expected.Remove(20, 25), table.ToString());
        }

        [Fact]
        public void PieceTable_CanInsertString()
        {
            var add = " Hello, nice to meet you, too.";
            var table = new PieceTable(TextSequence);
            table.Insert(add);

            var expected = TextSequence + add;
            Assert.Equal(expected, table.ToString());
        }

        [Fact]
        public void PieceTable_CanAddPieceToTable()
        {
            var table = new PieceTable(TextSequence);
            var piece = new Piece(
                PieceType.OriginalText, 19, 5);

            table.Add(piece);

            var expected = TextSequence + " you.";
            Assert.Equal(expected, table.ToString());
        }

        [Fact]
        public void PieceTable_CanGetPiecesCollection()
        {
            var table = new PieceTable(TextSequence);

            Assert.IsType<List<Piece>>(table.GetPieces());
        }

        [Fact]
        public void PieceTable_CanDetermineWhetherHaveOrNot()
        {
            var table = new PieceTable(TextSequence);
            
            var same = new Piece(
                PieceType.OriginalText, 0, 24);

            var fake = new Piece(
                PieceType.OriginalText, 3, 2);
            Assert.False(table.Contains(fake));

            var pieces = table.GetPieces();
            Assert.True(table.Contains(pieces[0]));
            Assert.True(table.Contains(same));
        }

        [Fact]
        public void PieceTable_CanFindIndexInTable_WithPiece()
        {
            var table = new PieceTable(TextSequence);
            var piece = new Piece(
                PieceType.OriginalText, 0, 24);

            var index = table.FindIndex(piece);

            Assert.Equal(0, index);
        }

        [Fact]
        public void PieceTable_CanFindIndexInTable_WithPosition()
        {
            var table = new PieceTable(TextSequence);
            var position = 10;

            Assert.Equal(0, table.FindIndex(position));

            position = -1;
            Assert.Equal(-1, table.FindIndex(position));
            position = 100;
            Assert.Equal(-1, table.FindIndex(position));
        }
    }
}

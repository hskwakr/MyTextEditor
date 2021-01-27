using System;
using System.Collections.Generic;
using Xunit;

namespace MyTextEditor.EditorBuffer
{
    // TODO:
    // テキストエディターを作りたい。
    // ====================
    // - テキストエディタに求める操作
    //      - ファイルの読み書き、編集
    // ====================
    // - テキストエディタを構成するもの
    //      - インターフェース
    //      - テキスト情報を保持するバッファー
    //          - Piece table
    //      - ファイルIO
    // ====================
    // - Piece tableに関わる要素
    //      - Source            <- 読み込んだファイル
    //      - Original text     <- 読み込んだときの状態のデータ（読み取り専用）
    //      - Added text        <- 追加されたデータ（追加専用）
    //      - Piece             <- OriginalとAddedの文字列を始点と長さで追跡する。Pieceの集合でbufferを表現する
    //      - Piece table       <- pieceを管理する
    // ====================

    // TODO: for one day
    // - ToString部分を別のViewに分離できる。PieceTableはPieceのリストを返すメソッドがいい
    // - Original, Addedのファクトリがいるかどうか（まだわからない）
    // - PieceクラスのGetHashCodeの実装（必要になるまで待ってもいいかも）
    // - 読み取り専用のプロパティにはreadonlyをつけれるようにしたい（正解かどうかわからない）

    // TODO: for now
    // Piece tableを実装したい
    // - テストの構造を整理したほうがいいかも
    // - このpieceの探し方だと同じ形のpieceが複数tableの中に入っていたら違う場所のpieceを見つけてくるかも
    //      - 実際にかぶるかもしれないシチュエーション想像してみよう
    // - Insertにoffset(ポジション)いるかも
    // - （テスト書いてない）PieceからPieceを作れるようにしよう
    // - RemoveのstartIndexとlastIndexの境界チェック

    public class TestPieceTable
    {
        public readonly string TextSequence;
        public TestPieceTable()
        {
            TextSequence = 
                "Hello, nice to meet you.";
        }

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

        [Fact]
        public void OriginalText_CanSetString_OnlyInConstructor()
        {
            var original = new OriginalText(TextSequence);

            // compiler block original.TextSequece = "";
            Assert.Equal(TextSequence, original.Sequece);
        }

        [Fact]
        public void AddedText_CanChangeString()
        {
            var added = new AddedText(TextSequence);
            Assert.Equal(TextSequence, added.Sequece);

            added.Sequece = "";
            Assert.Equal("", added.Sequece);
        }

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
    }
}

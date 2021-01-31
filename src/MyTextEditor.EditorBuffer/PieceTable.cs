using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTextEditor.EditorBuffer
{
    public class PieceTable
    {
        private OriginalText _original;
        private AddedText _added;
        private List<Piece> _table;

        public PieceTable(string textSequence)
        {
            _original = new OriginalText(textSequence);
            _added = new AddedText("");
            _table = new List<Piece>();

            this.Add(new Piece(
                PieceType.OriginalText, 0, textSequence.Length));
        }

        public override string ToString()
        {
            string buffer = "";
            foreach (var piece in _table)
            {
                switch (piece.Type)
                {
                    case PieceType.OriginalText:
                        buffer += _original.Sequece.Substring(piece.Offset, piece.Length);
                        break;
                    case PieceType.AddedText:
                        buffer += _added.Sequece.Substring(piece.Offset, piece.Length);
                        break;
                }
            }

            return buffer;
        }

        /// <summary>
        /// Subtract pieces.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="minus"></param>
        /// <returns>List of Pieces that is sorted by Offset</returns>
        public List<Piece> Subtract(Piece from, Piece minus)
        {
            var result = new List<Piece>();

            if (from == null || minus == null)
            {
                throw new Exception("Invalid arguments");
            }

            if (from.Type != minus.Type)
            {
                throw new Exception("Invalid arguments");

            }

            var nextOffset = minus.Offset + minus.Length;

            result.Add(new Piece(
                minus.Type, nextOffset, from.Length - nextOffset
            ));

            result.Add(new Piece(
                from.Type, from.Offset, minus.Offset
            ));

            return result.OrderBy(p => p.Offset).ToList<Piece>();
        }

        public List<Piece> GetPieces()
        {
            return _table;
        }

        public bool Contains(Piece piece)
        {
            return _table.Contains(piece);
        }

        public int FindIndex(Piece piece)
        {
            return _table.IndexOf(piece);
        }

        public int FindIndex(int position)
        {
            if (position < 0)
            {
                return -1;
            }

            int sum = 0, index = 0;
            foreach (var piece in _table)
            {
                sum += piece.Length;

                if (sum >= position)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public void Insert(int startPosition, string add)
        {
            var startIndex = this.FindIndex(startPosition);

            var added = new Piece(
               PieceType.AddedText,
               _added.Sequece.Length,
               add.Length
            );
            
            if (startPosition == _table.Select(p => p.Length).Sum())
            {
                this.Add(added);
            }
            else if(startPosition == 0)
            {
                var affected = new List<Piece>();
                for (int i = startIndex; i < _table.Count; i++)
                {
                    affected.Add(_table[i]);
                }

                _table.RemoveRange(startIndex, _table.Count - startIndex);
                
                this.Add(added);
                _table.AddRange(affected);
            }
            else
            {
                int startOffset = this.GetOffset(startIndex);

                var before = new Piece(
                    _table[startIndex].Type,
                    _table[startIndex].Offset,
                    startPosition - startOffset);

                var after = new Piece(
                    _table[startIndex].Type,
                    before.Offset + before.Length,
                    _table[startIndex].Length - before.Length);

                if (added.Length > 0)
                {
                    _table.RemoveAt(startIndex);

                    if (before.Length > 0)
                    {
                        _table.Insert(startIndex, before);
                    }

                    _table.Insert(startIndex + 1, added);

                    if (after.Length > 0)
                    {
                        _table.Insert(startIndex + 2, after);
                    }
                }
            }

            _added.Sequece += add;
        }

        /// <summary>
        /// Calculate sum of length of piece in piece table until index. It doesn't contain a piece of index.
        /// </summary>
        /// <param name="index">A index of Piece table collection</param>
        /// <returns></returns>
        public int GetOffset(int index)
        {
            int result = 0, counter = 0;

            foreach (var piece in _table)
            {
                if (counter < index)
                {
                    result += piece.Length;
                }
                else
                {
                    break;
                }

                counter++;
            }

            return result;
        }

        public void Add(Piece piece)
        {
            _table.Add(piece);
        }

        /// <summary>
        /// Remove string in table with position of table and length.
        /// </summary>
        /// <param name="startPosition">a specific position in piece table.</param>
        /// <param name="length">a length of range which you want to remove</param>
        public void Remove(int startPosition, int length)
        {
            var lastPosition = startPosition + length - 1;
            var startIndex = this.FindIndex(startPosition);
            var lastIndex = this.FindIndex(lastPosition);

            int startOffset = this.GetOffset(startIndex);
            int lastOffset = this.GetOffset(lastIndex);

            var before = new Piece(
                _table[startIndex].Type,
                _table[startIndex].Offset,
                startPosition - startOffset
            );

            var after = new Piece(
                _table[lastIndex].Type,
                _table[lastIndex].Offset + lastPosition - lastOffset + 1,
                _table[lastIndex].Length - lastPosition + lastOffset - 1
            );

            var count = _table.Count(q =>{ 
                var index = this.FindIndex(q);
                return index >= startIndex && index <= lastIndex;
            });
            _table.RemoveRange(startIndex, count);

            if (before.Length > 0)
            {
                _table.Insert(startIndex, before);
            }

            if (after.Length > 0)
            {
                _table.Insert(startIndex + 1, after);
            }
        }
    }
}

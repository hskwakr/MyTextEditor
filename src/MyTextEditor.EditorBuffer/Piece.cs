using System;
using System.Diagnostics.CodeAnalysis;

namespace MyTextEditor.EditorBuffer
{
    /// <summary>
    /// This is a value object to represent a part of piece table
    /// </summary>
    public class Piece : IEquatable<Piece>
    {
        public PieceType Type { get; }
        public int Offset { get; }
        public int Length { get; }

        public Piece(PieceType type, int offset, int length)
        {
            Type = type;
            Offset = offset;
            Length = length;
        }

        public Piece(Piece piece)
        {
            Type = piece.Type;
            Offset = piece.Offset;
            Length = piece.Length;
        }

        public bool Equals([DisallowNull] Piece other)
        {
            if (this.Type != other.Type)
            {
                return false;
            }
            else if (this.Offset != other.Offset)
            {
                return false;
            }
            else if (this.Length != other.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

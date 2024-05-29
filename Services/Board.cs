using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Models;

namespace Checkers.Services
{
    internal class Board
    {
         public readonly EPiece[,] m_board;
         public int BlackPieces { get; set; }
         public int WhitePieces { get; set; }

         public Board()
         {
             m_board = new EPiece[8, 8];
             BlackPieces = 12;
             WhitePieces = 12;
             InitializeBoard();
         }
         public Board(EPiece[] ePieces , int BlackPieces, int WhitePieces)
        {
             m_board = new EPiece[8, 8];
             this.BlackPieces = BlackPieces;
             this.WhitePieces = WhitePieces;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    m_board[row, col] = ePieces[row * 8 + col];
                }
            }
         }
         public bool MovePiece(int sRow, int sCol, int dRow, int dCol)
         {
            if(!IsWithinBounds(sRow, sCol) || !IsWithinBounds(dRow, dCol))
            {
                throw new ArgumentException("Invalid move: Source or destination position is out of bounds.");
            
            }
            if (m_board[dRow, dCol] != EPiece.Empty)
            {
                throw new ArgumentException("Invalid move: Destination position is not empty.");
                //Console.WriteLine("Invalid move: Destination position is not empty.");
                //return;
            }
            if(!IsValidMoveDirection(sRow, sCol, dRow, dCol))
            {
                throw new ArgumentException("Invalid move: Invalid move direction.");
                //Console.WriteLine("Invalid move: Invalid move direction.");
                //return;
            }
            if (Math.Abs(dRow - sRow) == 1 && Math.Abs(dCol - sCol) == 1)
            {
                // Move the piece to the destination position
                m_board[dRow, dCol] = m_board[sRow, sCol];
                m_board[sRow, sCol] = EPiece.Empty;
                TurnToKing(dRow, dCol);
                return false;
             
            }
            else if (Math.Abs(dRow - sRow) == 2 && Math.Abs(dCol - sCol) == 2)
            {
                int capturedRow = (sRow + dRow) / 2;
                int capturedCol = (sCol + dCol) / 2;

                // Check if there is an opponent's piece to capture

                // Move the piece to the destination position and remove the captured piece

                m_board[dRow, dCol] = m_board[sRow, sCol];
                m_board[sRow, sCol] = EPiece.Empty;

                if (m_board[capturedRow, capturedCol] == EPiece.Black || m_board[capturedRow, capturedCol] == EPiece.BlackKing)
                {
                    BlackPieces--;
                }
                else if (m_board[capturedRow, capturedCol] == EPiece.White || m_board[capturedRow, capturedCol] == EPiece.WhiteKing)
                {
                    WhitePieces--;
                }

                m_board[capturedRow, capturedCol] = EPiece.Empty;

                TurnToKing(dRow, dCol);
                return true;
            }
            return false;
              
         }
         
         public void InitializeBoard()
         {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    m_board[row, col] = EPiece.Empty;
                }
            }

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        m_board[row, col] = EPiece.White;
                    }
                }
            }

            // Place white pieces
            for (int row = 5; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {

                    if ((row + col) % 2 == 1)
                    {
                        m_board[row, col] = EPiece.Black;
                    }
                }
            }
         }
        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }
        private bool IsValidMoveDirection(int sRow, int sCol, int dRow, int dCol)
        {
            int rowDifference = dRow - sRow;
            int colDifference = Math.Abs(dCol - sCol);

            // Check if the move is diagonal and within the board bounds
            if (Math.Abs(rowDifference) == Math.Abs(colDifference) && IsWithinBounds(dRow, dCol))
            {
                
                if (m_board[sRow, sCol] == EPiece.Black && rowDifference == -1)
                {
                    return true;
                }
                else if (m_board[sRow, sCol] == EPiece.White && rowDifference == 1)
                {
                    return true;
                }
                if (m_board[sRow, sCol] == EPiece.Black && rowDifference == -2 && (m_board[dRow, dCol] == EPiece.Empty && m_board[(sRow + dRow) / 2, (sCol + dCol) / 2] == EPiece.White || m_board[(sRow + dRow) / 2, (sCol + dCol) / 2] == EPiece.WhiteKing)) 
                {
                    return true;
                }
                else if (m_board[sRow,sCol] == EPiece.White && rowDifference == 2 && (m_board[dRow,dCol] == EPiece.Empty && m_board[(sRow + dRow) / 2, (sCol + dCol) / 2] == EPiece.Black || m_board[(sRow + dRow) / 2, (sCol + dCol) / 2] == EPiece.BlackKing))
                {
                    return true;
                }
                // King: can move both forward and backward
                else if (m_board[sRow, sCol] == EPiece.BlackKing || m_board[sRow, sCol] == EPiece.WhiteKing)
                {
                    return true;
                }
            }

            return false;
        }
        public void TurnToKing(int row, int col)
        {
            if (m_board[row, col] == EPiece.Black && row == 0)
            {
                m_board[row, col] = EPiece.BlackKing;
            }
            else if (m_board[row, col] == EPiece.White && row == 7)
            {
                m_board[row, col] = EPiece.WhiteKing;
            }
        }        
    }
}

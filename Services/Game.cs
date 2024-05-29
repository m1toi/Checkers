using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Checkers.Models;
namespace Checkers.Services
{
    internal class Game
    {
        public Board Board;
        public EPiece CurrentPlayer { get; set; }
        public EGameState GameState { get; set; }
        public bool PieceMoved { get; set; }
        public bool MultipleCaptures { get; set; }
       
        public Game()
        {
            Board = new Board();
            PieceMoved = false;
            CurrentPlayer = EPiece.Black;
            MultipleCaptures = false;
            GameState = EGameState.Playing;
        }

        public bool MovePiece(int sRow,int sCol,int dRow,int dCol)
        {
            bool CaptureWasMade;
            if (GameState != EGameState.Playing)
            {
                throw new Exception("The game is over!");
            }
            if (PieceMoved)
            {
                throw new Exception("You have already moved a piece!");
            }
            if (Board.m_board[sRow, sCol] == EPiece.Empty)
            {
                throw new Exception("There is no piece to move!");
            }
     
            if (Board.m_board[sRow, sCol] == CurrentPlayer || ((Board.m_board[sRow,sCol] == EPiece.BlackKing) && (CurrentPlayer == EPiece.Black))
                || ((Board.m_board[sRow, sCol] == EPiece.WhiteKing) && (CurrentPlayer == EPiece.White)))
            {

                PieceMoved = true;
                CaptureWasMade=Board.MovePiece(sRow, sCol, dRow, dCol);
                Console.WriteLine(Board.WhitePieces);
                Console.WriteLine(Board.BlackPieces);
                if(MultipleCaptures == false)
                {
                    return false;
                }
                if (CaptureWasMade == false)
                    return false;
                if (CheckForMoreCaptures(dRow, dCol) && MultipleCaptures == true && Math.Abs(sRow-dRow)==2 && Math.Abs(sCol - dCol)==2)
                {

                    PieceMoved = false;
                    return true;
                }
            }
            else
            {
                throw new Exception("It is not your turn!");
            }
            return false;
        }

        public bool CheckForMoreCaptures(int sRow, int sCol)
        {
            if (sRow<2 || sRow>5 || sCol<2 || sCol>5)
            {
                if  (
                    (Board.m_board[sRow, sCol] == EPiece.Black && sCol < 2) &&
                    ((Board.m_board[sRow - 1, sCol + 1] == EPiece.White && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty))
                    )
                    return true;
                if  (
                    (Board.m_board[sRow, sCol] == EPiece.Black && sCol > 5) &&
                    ((Board.m_board[sRow - 1, sCol - 1] == EPiece.White && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty))
                    )
                    return true;
                if  (
                    (Board.m_board[sRow, sCol] == EPiece.White && sCol > 5) && 
                    ((Board.m_board[sRow + 1, sCol - 1] == EPiece.Black && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty))
                    )
                    return true;
                if (
                    (Board.m_board[sRow, sCol] == EPiece.White && sCol < 2) 
                    &&(
                           (Board.m_board[sRow + 1, sCol + 1] == EPiece.Black && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                        || (Board.m_board[sRow + 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                       )
                    )
                    return true;

                if (
                    (Board.m_board[sRow, sCol] == EPiece.BlackKing && sCol < 2)
                    && (
                        (Board.m_board[sRow - 1, sCol + 1] == EPiece.White && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                        || (Board.m_board[sRow - 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                        || (Board.m_board[sRow + 1, sCol + 1] == EPiece.White && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                        || (Board.m_board[sRow + 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                       )
                     )
                { 
                    return true;                
                }
                if (
                    (Board.m_board[sRow, sCol] == EPiece.BlackKing && sCol > 5 && sRow > 2) &&
                    (
                       (Board.m_board[sRow - 1, sCol - 1] == EPiece.White && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol - 1] == EPiece.White && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    )
                    )
                {
                    return true;
                }

                if (Board.m_board[sRow, sCol] == EPiece.WhiteKing && sCol < 2 && sRow < 5)
                {
                    if ((Board.m_board[sRow - 1, sCol + 1] == EPiece.Black && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                  || (Board.m_board[sRow - 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                  || (Board.m_board[sRow + 1, sCol + 1] == EPiece.Black && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                  || (Board.m_board[sRow + 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                        )
                    return true;
                }
                if (
                    (Board.m_board[sRow, sCol] == EPiece.WhiteKing && sCol > 5 && sRow < 5)&&
                    (
                        (Board.m_board[sRow - 1, sCol - 1] == EPiece.Black && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                        || (Board.m_board[sRow - 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                        || (Board.m_board[sRow + 1, sCol - 1] == EPiece.Black && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                        || (Board.m_board[sRow + 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    )
                    )
                    return true;
                if (
                    (Board.m_board[sRow, sCol] == EPiece.BlackKing && sRow < 2 && sCol < 5) &&
                    (
                    (Board.m_board[sRow + 1, sCol - 1] == EPiece.White && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol + 1] == EPiece.White && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                    )
                    )
                    return true;
                if (
                    (Board.m_board[sRow, sCol] == EPiece.BlackKing && sRow > 5) &&
                    ((Board.m_board[sRow - 1, sCol - 1] == EPiece.White && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol + 1] == EPiece.White && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                    )
                    )
                    return true;
                if (
                    (Board.m_board[sRow, sCol] == EPiece.WhiteKing && sRow < 2)&&
                    (
                    (Board.m_board[sRow + 1, sCol - 1] == EPiece.Black && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol + 1] == EPiece.Black && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                    || (Board.m_board[sRow + 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                    )
                    )
                    return true;
                if (
                   ( Board.m_board[sRow, sCol] == EPiece.WhiteKing && sRow > 5 && sCol <5 ) &&
                    (
                    (Board.m_board[sRow - 1, sCol - 1] == EPiece.Black && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol + 1] == EPiece.Black && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                    || (Board.m_board[sRow - 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                    )
                    )
                    return true;

                return false;
            }
            if (
                (Board.m_board[sRow, sCol] == EPiece.Black) && 
                (  (Board.m_board[sRow-1,sCol-1] == EPiece.White && Board.m_board[sRow-2,sCol-2] == EPiece.Empty)
                || (Board.m_board[sRow-1,sCol+1] == EPiece.White && Board.m_board[sRow-2,sCol+2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol - 1] == EPiece.WhiteKing  && Board.m_board[sRow-2,sCol-2] == EPiece.Empty )
                || (Board.m_board[sRow - 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                )
                )
                return true;
            if (
                (Board.m_board[sRow, sCol] == EPiece.White) &&
                ((Board.m_board[sRow + 1, sCol - 1] == EPiece.Black && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.Black && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty))
                )
                return true;
            if (
                (Board.m_board[sRow, sCol] == EPiece.BlackKing) &&
                ((Board.m_board[sRow - 1, sCol - 1] == EPiece.White && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol + 1] == EPiece.White && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol - 1] == EPiece.White && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.White && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol - 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.WhiteKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty))
                )
                return true;

            if (
                (Board.m_board[sRow, sCol] == EPiece.WhiteKing) &&
                ((Board.m_board[sRow - 1, sCol - 1] == EPiece.Black && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol + 1] == EPiece.Black && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow - 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow - 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol - 1] == EPiece.Black && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.Black && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol - 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol - 2] == EPiece.Empty)
                || (Board.m_board[sRow + 1, sCol + 1] == EPiece.BlackKing && Board.m_board[sRow + 2, sCol + 2] == EPiece.Empty))
                )
                return true;

            return false;
        }
        public void SwitchTurn()
        {
            if (CurrentPlayer == EPiece.Black)
            {
                CurrentPlayer = EPiece.White;


                PieceMoved = false;
            }
            else
            {
                CurrentPlayer = EPiece.Black;
                PieceMoved = false;
            }
        }

        public void CheckGameOver()
        {
            if (Board.BlackPieces == 0)
            {
                GameState = EGameState.WhiteWon;
            }
            else if (Board.WhitePieces == 0)
            {
                GameState = EGameState.BlackWon;
            }
        }
        public void SaveGame(string filePath)
        {
            GameConfiguration gameConfiguration = new GameConfiguration
            {
                ePieces = Board.m_board.Cast<EPiece>().ToArray(),
                BlackPieces = Board.BlackPieces,
                WhitePieces = Board.WhitePieces,
                CurrentPlayer = CurrentPlayer,
                GameState = GameState,
                MultipleCaptures = MultipleCaptures
            };
            JsonUtilities.SerializeGameConfiguration(gameConfiguration, filePath);
        }

        public void LoadGame(string filePath)
        {
            GameConfiguration gameConfiguration = JsonUtilities.DeserializeGameConfiguration(filePath);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board.m_board[i, j] = gameConfiguration.ePieces[i * 8 + j];
                }
            }
            Board.BlackPieces = gameConfiguration.BlackPieces;
            Board.WhitePieces = gameConfiguration.WhitePieces;
            CurrentPlayer = gameConfiguration.CurrentPlayer;
            GameState = gameConfiguration.GameState;
            MultipleCaptures = gameConfiguration.MultipleCaptures;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Checkers.Models;
using Checkers.Services;
using ICommandDemoAgain.Commands;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Checkers.ViewModels
{
    
    public class GameViewModel : BaseViewModel
    {
        public int firstClickRow = -1;
        public int firstClickColumn = -1;
        public int secondClickRow = -1;
        public int secondClickColumn = -1;
        public bool checkForMoreCaptures { get; set; }
        private string _statsFilePath = "../../statistics.json";
        public int WhiteWins { get; set; }
        public int BlackWins { get; set; }
        public ObservableCollection<PieceImage> Pieces { get; set; }

        private Game _game = new Game();
        public ICommand PieceClickedCommand { get; private set; }
        public ICommand ChangeTurnCommand { get; private set; }
        public ICommand AllowMultipleJumpsCommand { get; private set; }
        public ICommand NewGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand LoadGameCommand { get; private set; }
        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ShowStatisticsCommand { get; private set; }


        private string _currentPlayer ="Black";
        public string CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }
        private string _currentGameState = "Game State: Playing";
        public string CurrentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;
                OnPropertyChanged(nameof(CurrentGameState));
            }
        }

        public GameViewModel()
        {
            checkForMoreCaptures = false;
            Pieces = new ObservableCollection<PieceImage>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (i < 3)
                        {
                            Pieces.Add(new PieceImage("/Assets/WhitePiece.png", i, j));
                        }
                        else if (i > 4)
                        {
                            Pieces.Add(new PieceImage("/Assets/BlackPiece.png", i, j));
                        }
                        else
                        {
                            Pieces.Add(new PieceImage("/Assets/EmptyPiece.png", i, j));
                        }
                    }
                    else
                    {
                        Pieces.Add(new PieceImage("/Assets/EmptyPiece.png", i, j));
                    }
                }
            }
            LoadStatistics();
            PieceClickedCommand = new RelayCommand(o => OnPieceSelected(o));
            AllowMultipleJumpsCommand = new RelayCommand(o => OnAllowMultipleJumps(o));
            ChangeTurnCommand = new RelayCommand(o => OnChangeTurn());
            NewGameCommand = new RelayCommand(o => OnNewGame());
            SaveGameCommand = new RelayCommand(o => SaveGame());
            LoadGameCommand = new RelayCommand(o => LoadGame());
            ShowHelpCommand = new RelayCommand(o => ShowHelp());
            ShowStatisticsCommand = new RelayCommand(o => ShowStatistics());
           
        }

        public void OnPieceSelected(object param)
        {
            var piece = param as PieceImage;
           
            if (firstClickRow == -1)
            {
                firstClickRow = piece.X;
                firstClickColumn = piece.Y;
            }
            else
            {
                secondClickRow = piece.X;
                secondClickColumn = piece.Y;
                try
                {
                    
                    checkForMoreCaptures = _game.MovePiece(firstClickRow, firstClickColumn, secondClickRow, secondClickColumn);
                    _game.CheckGameOver();
                    if (_game.GameState == EGameState.WhiteWon)
                    {
                        CurrentGameState = "White Won";
                        UpdateBoard();
                        WhiteWins++;
                        SaveStatistics();
                        return;
                    }
                    else if (_game.GameState == EGameState.BlackWon)
                    {
                        CurrentGameState = "Black Won";
                        UpdateBoard();
                        BlackWins++;
                        SaveStatistics();
                        return;
                    }
                    if (checkForMoreCaptures)
                    {
                        Console.WriteLine("More captures available");
                        firstClickRow = secondClickRow; 
                        firstClickColumn = secondClickColumn;
                    }
                    else
                    {
                        _game.SwitchTurn();
                        CurrentPlayer = _game.CurrentPlayer.ToString();
                        firstClickRow = -1;
                    }
                }
                catch (Exception e)
                {
                    _game.PieceMoved = false;
                    firstClickRow = -1;
                    Console.WriteLine(e.Message);
                }
                UpdateBoard();
                
            }   
        }
        public void ShowStatistics()
        {
            MessageBox.Show("Black Wins: " + BlackWins + "\nWhite Wins: " + WhiteWins, "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void LoadStatistics()
        {
            if (File.Exists(_statsFilePath))
            {
                string json = File.ReadAllText(_statsFilePath);
                var stats = JsonConvert.DeserializeObject<Statistics>(json);
                WhiteWins = stats.WhiteWins;
                BlackWins = stats.BlackWins;
            }
        }
        private void SaveStatistics()
        {
            var stats = new Statistics
            {
                BlackWins = BlackWins,
                WhiteWins = WhiteWins
            };
            string json = JsonConvert.SerializeObject(stats);
            File.WriteAllText(_statsFilePath, json);
        }
       
        public void OnAllowMultipleJumps(object param)
        {
            _game.MultipleCaptures = true;
        }   

        public void OnChangeTurn()
        {
            if (_game.PieceMoved == true || _game.PieceMoved== false && checkForMoreCaptures == true)
            {
                _game.SwitchTurn();
                CurrentPlayer = _game.CurrentPlayer.ToString();
            }
            else
            {
                Console.WriteLine(_game.CurrentPlayer + " has not moved a piece yet");
                return;
            }
        }

        public void OnNewGame()
        {
            _game = new Game();
            CurrentPlayer = "Black";
            CurrentGameState = "Game State: Playing";
            UpdateBoard();
        }
        public void ShowHelp()
        {
            string message = "Creatorul programului: Mițoi Alex Gabriel\n" +
                     "Adresa de email instituțională: alex.gabriel.mitoi@student.unitbv.ro\n" +
                     "Grupă: 10LF322\n\n" +
                     "Descriere a jocului:\n" +
                     "Acest joc este o implementare a jocului de dame (checkers) " +
                     "utilizând WPF și arhitectura MVVM. Jucătorii trebuie să " +
                     "mută piesele lor pe tabla de joc și să captureze piesele " +
                     "oponente pentru a câștiga.";

            // Show message using MessageBox
            MessageBox.Show(message, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void UpdateBoard()
        {

            Pieces.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (_game.Board.m_board[i, j] == EPiece.White)
                        {
                            Pieces.Add(new PieceImage("/Assets/WhitePiece.png", i, j));
                        }
                        else if (_game.Board.m_board[i, j] == EPiece.Black)
                        {
                            Pieces.Add(new PieceImage("/Assets/BlackPiece.png", i, j));
                        }
                        else if (_game.Board.m_board[i, j] == EPiece.WhiteKing)
                        {
                            Pieces.Add(new PieceImage("/Assets/WhiteKing.png", i, j));
                        }
                        else if (_game.Board.m_board[i, j] == EPiece.BlackKing)
                        {
                            Pieces.Add(new PieceImage("/Assets/BlackKing.png", i, j));
                        }
                        else
                        {
                            Pieces.Add(new PieceImage("/Assets/EmptyPiece.png", i, j));
                        }   
                       
                    }
                    else
                    {
                        Pieces.Add(new PieceImage("/Assets/EmptyPiece.png", i, j));
                    }
                }
            }
        }
        public void SaveGame()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                _game.SaveGame(dialog.FileName);
                MessageBox.Show("Game saved successfully!", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void LoadGame()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                _game.LoadGame(dialog.FileName);
                MessageBox.Show("Game loaded successfully!", "Load Game", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentPlayer = _game.CurrentPlayer.ToString();
                CurrentGameState = "Game State: Playing";
                UpdateBoard();
            }
        }   
    }

    public class PieceImage
    {
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public PieceImage(string image, int row, int column)
        {
            Image = image;
            X = row;
            Y = column;
        }
    }
}

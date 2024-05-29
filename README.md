# Checkers

Description
This is a Checkers game implemented in C# using WPF for the user interface, following the MVVM design pattern. The game features two sets of pieces: white and red. The board is an 8x8 grid. Players take turns moving their pieces, with red going first.

Features
Simple Move: Pieces can move one square diagonally forward. If a piece reaches the opponent's end, it becomes a "king" and gains the ability to move diagonally backward.
Capture Move: Players can jump over opponent's pieces to capture them. Multiple captures in one turn are allowed if available.
Multiple Jumps: Enabled with an option at the start of the game, players can make multiple jumps in one turn if possible.
Game End: The game ends when one player has no more pieces left. The opponent is declared the winner.
Save Game State: Players can save and load game states.
Statistics: Track the number of wins for white and red players, as well as the maximum number of pieces remaining on the board for the winner.
How to Play
Run the application.
Start a new game or load a saved game from the File menu.
Make moves by clicking on the pieces and selecting the destination square.
Capture opponent's pieces by jumping over them.
Continue until one player wins or the game ends in a draw.
Menus
File Menu:

New Game: Start a new game.
Save: Save the current game state.
Open: Load a saved game.
Allow Multiple Jump: Toggle the option for multiple jumps.
Statistics: View win statistics.
Help Menu:

About: Information about the creator of the program.
Getting Started
To get a local copy up and running, follow these simple steps:

Clone the repository: git clone https://github.com/your-username/checkers-game.git
Open the project in Visual Studio or any compatible IDE.
Build and run the application.

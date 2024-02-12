# Checkers Game

This is a classic Checkers game implemented in C# using WPF.

## Game Rules

1. **Moves**
   - The game starts with the player playing as white.
   - Regular pieces move forward by 1 square.
   - Regular pieces cannot move backward.
   - Ladies can move both forward and backward any number of squares.
   - When a regular piece reaches the end of the board, it is promoted to a Lady.

2. **Capturing**
   - Capturing is done by jumping over an opponent's piece to an empty square diagonally.
   - Backward capturing is allowed.
   - Capturing is mandatory.
   - When multiple capturing options are available, the most advantageous one must be chosen, considering the number of pieces captured. A Lady is considered equivalent to a regular piece in this regard.
   - If there are multiple ways to reach the same square after capturing the same number of pieces, one of them is chosen randomly. This is particularly relevant for Ladies.
   - If a piece passes over the end line during capturing but does not stop on it, it does not become a Lady (no promotion).

3. **End of the Game**
   - There are two options for ending the game:
     - The game ends when a player captures all of the opponent's pieces.
     - The game ends when a player cannot make any more moves.
     - The game ends in a draw if no pieces are captured by Ladies in 15 moves.

## How to Play

- To play the game, simply run the executable.
- Press 'Esc' to access the pause menu and decide whether to continue or quit.
- Click on any piece to see possible moves if the selected piece has any.
- Choose one of the possible moves by clicking on them.
- Normal moves are displayed in green color.
- Capturing moves are displayed in yellow color.
- When the game is over, you can see the game over menu and decide whether to restart the game or quit.

## How it Looks Like

**Game View**
![image](https://github.com/kamil-caly/Checkers/assets/66841315/5089ecbf-db83-4b0b-b07f-4f21a8c8bb2b)

**Pause Menu**
![image](https://github.com/kamil-caly/Checkers/assets/66841315/1ebfeb78-9b6e-4a55-9dc0-1d186e1889fd)

**Game Over Menu**
![image](https://github.com/kamil-caly/Checkers/assets/66841315/e6353ec8-1646-4dc2-a84a-d6fcba224c84)

## How it was Implemented

- The game was implemented in C# using the WPF framework.
- The project is divided into two main folders: 'project_GUI' and 'project_logic'.
- 'project_GUI' consists of a WPF project with the main application window and smaller windows (end game menu and pause menu).
- GUI utilizes 'project_logic', which contains the core game logic, including algorithms for determining possible moves and captures.
- Some game elements, such as displaying the pause menu, are inspired by a chess tutorial available at the following link: [Chess Tutorial](https://www.youtube.com/watch?v=GEkSE6eZMGc&list=PLFk1_lkqT8MahHPi40ON-jyo5wiqnyHsL)
- Images of chess pieces, the chessboard, and mouse cursors for players were also used from the aforementioned tutorial.

## Challenges in Implementation

- Basic game elements such as the pause menu, game end, move hints, and drawing the game board were not challenging tasks.
- The problem arose when creating an algorithm for generating possible capturing moves for regular pieces and Ladies.
- The algorithm for regular pieces works correctly (no errors were noticed during testing).
- The capturing algorithm for Ladies is much more complex than for regular pieces because Ladies can capture across any number of squares.
- Consequently, numerous errors emerged during testing (the most significant ones, in my opinion, were corrected).
- One such issue occurred when a Lady was positioned between one regular piece on one side and another regular piece on the other side. Initially, the Lady would capture one regular piece and then retreat to capture the previous one (an invalid move :))
- The above-mentioned error has been resolved.
- The highest chance of error arises when there are many regular pieces to capture and multiple paths to do so since, as described in the rules, one must select the best capturing move.
- Fortunately, such a scenario is so rare in the game itself that it is highly unlikely.
- In situations where it is possible to capture the same number of regular pieces and end the move on the same square using at least two different paths, the issue was addressed by randomly selecting one path.

## Development Opportunities

- Improvement of the algorithm for determining possible moves for checkers (if feasible).
- Creation of a bot for solo gameplay utilizing simple algorithms or machine learning.
- Implementation of online gameplay capabilities, for instance, using web sockets.
- Player logging (or similar functionality) and storing results in a database.
- Additional in-game menu featuring player rankings.

## License

This project is licensed under the GNU General Public License v3.0.

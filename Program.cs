using System.ComponentModel;
using System.Numerics;

namespace NoughtsAndCrosses
{
    internal class Program
    {
        static void Main()
        {
            var playerO = new Player("", "O");
            var playerX = new Player("", "X");


            var turn = 0;
            var players = new Player[] { playerO, playerX };

            Board board = new Board();

            board.SetCounter(0, 0, playerO);
            board.SetCounter(1, 2, playerO);
            board.SetCounter(2, 2, playerO);

            Console.WriteLine(board.Output());

            // find a better way to do this
            Func<int, string> c = position =>
            {
                var player = board.GetFromPosition(position);
                return player == null ? " " : player.letter;
            };

            string output = $"1─2─3─┐\n" +
                      $"│{c(1)}│{c(2)}│{c(3)}│\n" +
                      $"4─5─6─┤\n" +
                      $"│{c(4)}│{c(5)}│{c(6)}│\n" +
                      $"7─8─9─┤\n" +
                      $"│{c(7)}│{c(8)}│{c(9)}│\n" +
                      $"└─┴─┴─┘\n";
           

            Console.WriteLine(output);

            while (board.DetectWin() == null)
            {
                var activePlayer = players[turn];

                Console.Write($"{activePlayer.letter}> ");
                ConsoleKeyInfo character = Console.ReadKey();
                int input;


                while (!int.TryParse(character.KeyChar.ToString(), out input) || input == 0)
                {
                    // for arrow keys we will need to do something _different_ because it doesn't delete the character, but moves the cursor
                    if (character.Key != ConsoleKey.Backspace)
                    {
                        Console.Write("\b \b");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    character = Console.ReadKey();

                }
                (int x, int y) = ToIndicies(input);

                try {
                    board.SetCounter(x, y, activePlayer);
                }
                catch(InvalidOperationException) {
                    continue;
                }
                turn = (turn += 1) % 2;

                Console.WriteLine($"\nSuccessfully converted to coords {x}, {y}");
            }
        }

        static (int, int) ToIndicies(int input)
        {
            return Math.DivRem(input - 1, 3);
        }

        private void boardOutput()
        {
            Func<int, string> circledLetter = x => "①";
            string output = $"┌─┐\n│{4}│\n└─┘";
        }
    }


    class Board
    {
        public List<List<Player?>> gameState;


        public Board()
        {

            gameState = new List<List<Player?>>();

            // initialise empty 3x3 board
            for (int i = 0; i < 3; i += 1)
            {
                gameState.Add(new List<Player?>() { null, null, null });
            }
        }

        public void SetCounter(int x, int y, Player player)
        {
            if (x > 2 || y > 2)
            {
                throw new IndexOutOfRangeException("Both x and y must be in range 0-2");
            }
            Player? existingPlayer = GetPlayer(x, y);
            if (existingPlayer != null)
            {
                throw new InvalidOperationException($"Position ({x}, {y}) is already {existingPlayer.letter}");
            }
            gameState[y][x] = player;
        }

        public Player? GetPlayer(int x, int y)
        {
            if (x > 2 || y > 2)
            {
                throw new IndexOutOfRangeException("Both x and y must be in range 0-2");
            }
            return gameState[y][x];
        }

        public Player? GetFromPosition(int pos)
    {
        int y = Math.DivRem(pos - 1, 3, out int x);
        return GetPlayer(x, y);
    }

        public string Output()
        {
            string ret = "";
            foreach (var row in gameState)
            {
                foreach (var i in row)
                {

                    ret += i != null ? i.ToString() : " ";
                }
                ret += "\n";
            }
            return ret;
        }


        private bool MatchFound(List<Player?> arr)
        {
            // if there is only one colour counter, and that colour is not Empty
            // we should make this also return the positions of 
            return arr.Distinct().Count() == 1 && arr[0] != null;

        }

        public Player? DetectWin()
        {


            List<Player?> foundPlayers = new();
            for (int x = 0; x < 3; x++) // top to bottom
            {
                for (int y = 0; y < 3; y++)
                {
                    foundPlayers.Add(GetPlayer(x, y));
                }
                foundPlayers.Clear();
            }

            for (int y = 0; y < 3; y++) // top to bottom
            {
                for (int x = 0; x < 3; x++)
                {
                    foundPlayers.Add(GetPlayer(x, y));
                }
                if (MatchFound(foundPlayers)) return foundPlayers[0];
                foundPlayers.Clear();
            }

            for (int x = 0; x > 3; x++)
            {
                foundPlayers.Add(GetPlayer(x, x));
                if (MatchFound(foundPlayers)) return foundPlayers[0];
                foundPlayers.Clear();
            }

            for (int x = 2, y = 0; y > 3; x--, y++)
            {
                foundPlayers.Add(GetPlayer(x, y));
                if (MatchFound(foundPlayers)) return foundPlayers[0];
            }

            return null;
        }
    }


    class Player
    {
        public string colour;
        public string letter;

        public Player(string colour, string letter)
        {
            this.colour = colour;
            this.letter = letter;
        }
    }
}

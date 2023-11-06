using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            var board = new Board();

            Player? winner;

            while ((winner = board.DetectWin()) == null)
            {

                if (board.IsFull())
                {
                    winner = null;
                    break;
                }

                var activePlayer = players[turn];

                Console.Write($"{board.Output()}\n{activePlayer.letter}> ");
                ConsoleKeyInfo character = Console.ReadKey();

                int input;

                // check parsing keypress as an int that is not 0
                if (!int.TryParse(character.KeyChar.ToString(), out input) || input == 0) { Console.WriteLine("\nInput must be in range [1-9]"); continue; }

                (int x, int y) = ToIndices(input);

                if (x > 2 || y > 2) { Console.WriteLine("\nInput out of bounds"); continue; } // check if out of range (should never happen), convert to exception
                if (board.GetPlayer(x, y) != null) { Console.WriteLine("\nTile already in use"); continue; } // check if counter already exists

                board.SetCounter(x, y, activePlayer);

                turn = (turn += 1) % 2;
               Console.Clear();

            }

            Console.WriteLine(board.Output());

            Console.WriteLine(winner == null ? "Game draw" : winner.letter + " wins!");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        static (int, int) ToIndices(int input)
        {
            var (y, x) = Math.DivRem(input - 1, 3);
            return (x, y);
        }
    }
}

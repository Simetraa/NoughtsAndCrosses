namespace NoughtsAndCrosses
{
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
            gameState[y][x] = player;
        }

        public bool IsFull()
        {
            foreach (var row in gameState)
            {
                foreach (var slot in row)
                {
                    if (slot == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public Player? GetPlayer(int x, int y)
        {
            return gameState[y][x];
        }

        public Player? GetFromPosition(int pos)
        {
            int y = Math.DivRem(pos - 1, 3, out int x);
            return GetPlayer(x, y);
        }

        public string Output()
        {
            string c(int position) // format each entry of the board. we must use this instead of a method on Player as player can be null.
            {
                var player = GetFromPosition(position);
                return player == null ? " " : player.letter;
            }

            string output = $"1─2─3─┐\n" +
                            $"│{c(1)}│{c(2)}│{c(3)}│\n" +
                            $"4─5─6─┤\n" +
                            $"│{c(4)}│{c(5)}│{c(6)}│\n" +
                            $"7─8─9─┤\n" +
                            $"│{c(7)}│{c(8)}│{c(9)}│\n" +
                            $"└─┴─┴─┘\n";


            return output;
        }


        private bool MatchFound(List<Player?> arr)
        {
            // if there is only one colour counter, and that colour is not Empty
            // we should make this also return the positions of the list elements
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

                if (MatchFound(foundPlayers)) return foundPlayers[0];
                foundPlayers.Clear();
            }

            for (int y = 0; y < 3; y++) // left to right
            {

                for (int x = 0; x < 3; x++)
                {
                    foundPlayers.Add(GetPlayer(x, y));
                }

                if (MatchFound(foundPlayers)) return foundPlayers[0];
                foundPlayers.Clear();
            }

            for (int x = 0; x < 3; x++) // positive diagonal, use x for both as x = y
            {
                foundPlayers.Add(GetPlayer(x, x));
            }
            if (MatchFound(foundPlayers)) return foundPlayers[0];
            foundPlayers.Clear();


            for (int x = 2, y = 0; y < 3; x--, y++) // negative diagonal
            {
                foundPlayers.Add(GetPlayer(x, y));
            }
            if (MatchFound(foundPlayers)) return foundPlayers[0];
            foundPlayers.Clear();


            return null;
        }
    }
}

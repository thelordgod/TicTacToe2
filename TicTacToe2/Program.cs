using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static System.Int32;

namespace TicTacToe
{
    internal static class Program
    {
        private static readonly Board _board = new Board();
        private static Player _currentPlayer = Player.One;

        static void Main()
        {
            do
            {
                DrawBoard();
                ReadMove();
                Console.WriteLine();
                if (IsVictory())
                {
                    DrawBoard();
                    break;
                }

                _currentPlayer = _currentPlayer == Player.One ? Player.Two : Player.One;
            } while (_board.squareStatus.ContainsValue(Status.Empty));

            Console.WriteLine(IsVictory() ? GetWinnerMessage() : "The game has ended in a draw!");
            Console.ReadLine();
        }

        private static string GetWinnerMessage()
        {
            return "Congratulations, player "
                   + (_currentPlayer == Player.One ? "one" : "two")
                   + ", you are winner!";
        }

        private static bool IsVictory()
        {
            var status = _board.squareStatus;
            bool result;

            for (int i = 1; i <= 9; i += 3)
            {
                result = status[key: i] == status[key: i + 1]
                         && status[key: i] == status[key: i + 2]
                         && status[key: i] != Status.Empty;
                if (result)
                    return true;
            }

            for (int i = 1; i <= 3; i++)
            {
                result = status[key: i] == status[key: i + 3]
                         && status[key: i] == status[key: i + 6]
                         && status[key: i] != Status.Empty;
                if (result)
                    return true;
            }

            result = status[key: 5] != Status.Empty
                     && (status[key: 5] == status[key: 1]
                         && status[key: 5] == status[key: 9]
                         || status[key: 5] == status[key: 3]
                         && status[key: 5] == status[key: 7]);

            return result;
        }

        private static void ReadMove()
        {
            Console.WriteLine("Player " + (_currentPlayer == Player.One ? "one" : "two") + " moves!");
            var input = Console.ReadKey();

            ChangeField(input);
        }

        private static void ChangeField(ConsoleKeyInfo input)
        {
            int inputKey = Parse(Regex.Replace
                (input.Key.ToString(), "[^1-9]", "0"));
            if (inputKey == 0)
            {
                HandleInvalidInput();
                return;
            }

            ProcessField(inputKey);
        }

        private static void HandleInvalidInput(string message = "Invalid input. Try again.")
        {
            Console.WriteLine("\n" + message);
            ReadMove();
        }

        private static void ProcessField(int inputKey)
        {
            if (_board.squareStatus[key: inputKey] == Status.Empty)
            {
                _board.squareStatus[key: inputKey] =
                    _currentPlayer == Player.One
                        ? Status.PlayerOne
                        : Status.PlayerTwo;
                return;
            }

            HandleInvalidInput("Field already occupied!");
        }

        private static void DrawBoard()
        {
            var builder = new StringBuilder();

            for (int i = 1; i <= 9; i++)
            {
                builder.Append(GetFieldValue(i));
                if (i % 3 == 0 && i < 9)
                    builder.AppendLine();
            }

            Console.WriteLine(builder);
        }

        private static string GetFieldValue(int field)
        {
            return _board.squareStatus[key: field] switch
            {
                Status.Empty => field.ToString(),
                Status.PlayerOne => "X",
                Status.PlayerTwo => "O",
                _ => null
            };
        }
    }

    internal enum Player
    {
        One,
        Two
    }

    internal class Board
    {
        public readonly Dictionary<int, Status> squareStatus = new Dictionary<int, Status>()
        {
            {1, Status.Empty},
            {2, Status.Empty},
            {3, Status.Empty},
            {4, Status.Empty},
            {5, Status.Empty},
            {6, Status.Empty},
            {7, Status.Empty},
            {8, Status.Empty},
            {9, Status.Empty}
        };
    }

    internal enum Status
    {
        Empty,
        PlayerOne,
        PlayerTwo
    }
}
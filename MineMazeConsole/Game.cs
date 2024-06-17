using MineMaze;
using MineMaze.Extentions;
using MineMaze.Models.Enums;

namespace MineMazeConsole
{
	internal class Game
	{
		private MineMazeEngine? _game;
		private bool _running = true;
		private string _errorMessage = string.Empty;
		internal void MainLoop()
		{
			CreateGame();
			while (_running)
			{
				DrawBoard();
				if (_game.Board.Status == GameStatus.Pending)
				{
					ProcessUserString(Console.ReadLine());
				}
				else
				{
					ProcessKeyStroke(Console.ReadKey());
				}
			}
		}

		private void ProcessKeyStroke(ConsoleKeyInfo key)
		{
			switch (_game.Status)
			{
				case GameStatus.Running:
					switch (key.Key)
					{
						case ConsoleKey.E:
							_running = false;
							break;
						case ConsoleKey.UpArrow:
						case ConsoleKey.U:
							ProcessMoveStatus(_game.Move(Direction.Up));
							break;
						case ConsoleKey.DownArrow:
						case ConsoleKey.D:
							ProcessMoveStatus(_game.Move(Direction.Down));
							break;
						case ConsoleKey.LeftArrow:
						case ConsoleKey.L:
							ProcessMoveStatus(_game.Move(Direction.Left));
							break;
						case ConsoleKey.RightArrow:
						case ConsoleKey.R:
							ProcessMoveStatus(_game.Move(Direction.Right));
							break;
						default:
							_errorMessage = "Invalid choise!";
							break;
					}
					break;
				case GameStatus.Win:
				case GameStatus.Lose:
					switch (key.Key)
					{
						case ConsoleKey.E:
							_running = false;
							break;
						case ConsoleKey.N:
							_game.NewGame();
							break;
						default:
							_errorMessage = "Invalid choise!";
							break;
					}
					break;
				default:
					_errorMessage = "Invalid choise!";
					break;
			}
		}

		private void ProcessUserString(string? input)
		{
			if (input == null)
			{
				_errorMessage = "Invalid choise!";
			}

			if (input.ToUpper() == "E")
			{
				_running = false;
			}

			int row = -1;
			if (int.TryParse(input, out row))
			{
				ProcessMoveStatus(_game.FirstMove(row));
				return;
			}

			_errorMessage = "Invalid choise!";
		}

		private void ProcessMoveStatus(MoveStatus moveStatus)
		{
			switch (moveStatus)
			{
				case MoveStatus.LifeLost:
					_errorMessage = "Bang";
					break;
				case MoveStatus.Sucsess:
				case MoveStatus.GameOver:
					_errorMessage = string.Empty; 
					break;
				case MoveStatus.Invalid:
					_errorMessage = "Invalid choise!";
					break;
				case MoveStatus.InvalidGameInProgress:
				case MoveStatus.StartPointNotSet:				
				case MoveStatus.GameError:
				default:
					_errorMessage = "Something went wrong";
					break;				
			}
		}

		private void DrawBoard()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;

			Console.WriteLine();
			Console.WriteLine("Status:" + _game.Board.Status.Discription());
			Console.WriteLine(string.Format("Score:{0} Lives:{1}", _game.CurrentPlayerData.Score, _game.CurrentPlayerData.Lives));
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Blue;

			string movelist = string.Empty;
			_game.GetCurrentGameMoves().ForEach(m =>
			{
				movelist += movelist.Length == 0 ? "" : ", ";
				movelist += m;
			});
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Moves:" + movelist);
			Console.WriteLine();

			if (_errorMessage.Length > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(_errorMessage);
				Console.WriteLine();
			}

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("E to exit!");
			switch (_game.Board.Status)
			{
				case GameStatus.Error:
				case GameStatus.ErrorParamitersNotSet:
					Console.WriteLine("Something Went Wrong!");
					break;
				case GameStatus.Pending:
					Console.WriteLine(string.Format("To start game enter Row for first move(1-{0})", _game.Paramiters.Rows));
					break;
				case GameStatus.Running:
					Console.WriteLine("Next Move? U = up, D = Down, L = Left, R = right, Or arrow keys.");
					break;
				case GameStatus.Win:
					Console.WriteLine("You Win");
					Console.WriteLine("N = New game");
					break;
				case GameStatus.Lose:
					Console.WriteLine("You Lose");
					Console.WriteLine("N = New game");
					break;
			}
		}

		private void CreateGame()
		{
			_game = MineMazeEngine.CreateGame();
		}
	}
}

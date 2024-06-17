using MineMaze.Extentions;
using MineMaze.Models;
using MineMaze.Models.Enums;
using MineMaze.Models.Json;
using Newtonsoft.Json;

namespace MineMaze
{
	public class MineMazeEngine
	{
		private const int MaxBoardSize = 50;
		private const int DefaultBoardSize = 8;
		private const int DefaultMines = 10;
		private const int DefaultLives = 5;

		public GameParamiters Paramiters { get; set; }
		public GameBoard? Board { get; set; }
		public PlayerData? CurrentPlayerData { get; set; }

		private List<GameBoardCell> _gameMoves = new();

		public GameStatus Status
		{
			get
			{
				return Board?.Status ?? GameStatus.Error;
			}
		}

		public static MineMazeEngine CreateGame(int rows = DefaultBoardSize, int columns = DefaultBoardSize, int mines = DefaultMines, int lives = DefaultLives) => new MineMazeEngine(rows, columns, mines, lives);
		public static MineMazeEngine? CreateGame(string boardJson)
		{
			BoardJson? boardData = JsonConvert.DeserializeObject<BoardJson>(boardJson);
			return boardData == null ? throw new ArgumentException("Invalid game data.") : new MineMazeEngine(boardData);
		}

		private MineMazeEngine(BoardJson boardData)
		{
			if (boardData?.Paramiters == null || boardData?.Mines == null)
			{
				throw new ArgumentException("Invalid game data.");
			}

			Paramiters = boardData.Paramiters;
			
			GenerateBoard();
			if (Board != null)
			{
				boardData.Mines.ForEach(m => Board.GameBoardData[m.Column][m.Row].Status = CellStatus.Mine);
			}

			CurrentPlayerData = new PlayerData { Lives = Paramiters.Lives };
			_gameMoves = new();
		}

		public MineMazeEngine(int rows, int columns, int mines, int lives)
		{
			ValidateParamiters(rows, columns, mines, lives);
			Paramiters = new GameParamiters { Rows = rows, Columns = columns, Mines = mines, Lives = lives };
			NewGame();
		}

		public string GameString
		{
			get
			{
				return Board == null ? string.Empty : JsonConvert.SerializeObject(new BoardJson { Paramiters = Paramiters, Mines = Board.Mines });
			}
		}

		public static MineMazeEngine Load(string saveString)
		{
			GameSaveJson? saveData = JsonConvert.DeserializeObject<GameSaveJson>(saveString);
			if (saveData?.Board == null)
			{
				throw new ArgumentException("Invalid save data.");
			}

			try
			{
				MineMazeEngine engine = new MineMazeEngine(saveData.Board);
				if(engine == null || engine.Board == null)
				{
					throw new ArgumentException("Invalid save data.");
				}
				engine.Board.Status = saveData.Status;
				engine.CurrentPlayerData = saveData.CurrentPlayerData;
				engine._gameMoves = saveData.Moves ?? new();
				engine._gameMoves.ForEach(m => engine.Board.GameBoardData[m.Column][m.Row].Status = m.Status);
				return engine;
			}
			catch(ArgumentException ex)
			{
				throw new ArgumentException("Invalid save data.", ex);
			}
		}

		public string Save()
		{
			return Board == null ? string.Empty : JsonConvert.SerializeObject(new GameSaveJson
			{
				Board = new BoardJson
				{
					Paramiters = Paramiters,
					Mines = Board.Mines
				},
				Status = Board.Status,
				CurrentPlayerData = CurrentPlayerData,
				Moves = _gameMoves
			});
		}

		public GameStatus NewGame()
		{
			if (Paramiters == null)
			{
				return GameStatus.ErrorParamitersNotSet;
			}
			CurrentPlayerData = new PlayerData { Lives = Paramiters.Lives };
			_gameMoves = new();
			GenerateBoard();
			PoulateMines();

			return GameStatus.Pending;
		}

		private void GenerateBoard()
		{
			Board = new GameBoard
			{
				Status = GameStatus.Pending,
				Mines = new List<GameBoardCell>(),
				GameBoardData = Enumerable.Range(0, Paramiters.Columns)
					.Select(c => new KeyValuePair<int, List<GameBoardCell>>(c, Enumerable.Range(0, Paramiters.Rows)
						.Select(r => new GameBoardCell { Column = c, Row = r }).ToList()))
					.ToDictionary()
			};
		}
		private void PoulateMines()
		{
			int mineCount = 0;
			int cellIndex;
			GameBoardCell? cell;
			Random rand = new Random();
			while (mineCount < Paramiters.Mines)
			{
				cellIndex = rand.Next(1, Paramiters.Columns * Paramiters.Rows);
				cell = Board?.GameBoardData[cellIndex / Paramiters.Rows][cellIndex % Paramiters.Rows];
				if (cell?.Status == CellStatus.None)
				{
					cell.Status = CellStatus.Mine;
					mineCount++;
					Board?.Mines.Add(cell);
				}
			}
		}

		private void ValidateParamiters(int rows, int columns, int mines, int lives)
		{
			if (rows < 2 || rows > MaxBoardSize)
			{
				throw new ArgumentException(string.Format("Rows argument Value must be between 2 and MaxBoardSize({0}).", MaxBoardSize));
			}
			if (columns < 2 || columns > MaxBoardSize)
			{
				throw new ArgumentException(string.Format("Columns argument Value must be between 2 and MaxBoardSize({0}).", MaxBoardSize));
			}
			if (mines < 0 || mines > rows * columns)
			{
				throw new ArgumentException(string.Format("Mines argument Value must be between 0 and Number of cells({0}).", rows * columns));
			}
			if (lives <= 0)
			{
				throw new ArgumentException("Lives argument Value must be greater than 0.");
			}
		}

		public MoveStatus FirstMove(int row, bool isChessPosition = true)
		{
			if (Board == null)
			{
				return MoveStatus.GameError;
			}
			if (Board?.Status != GameStatus.Pending)
			{
				return MoveStatus.InvalidGameInProgress;
			}

			return Move(row - (isChessPosition ? 1 : 0), 0);
		}

		public MoveStatus Move(Direction direction)
		{
			if (Board == null)
			{
				return MoveStatus.GameError;
			}

			if (CurrentPlayerData?.Position == null)
			{
				return MoveStatus.StartPointNotSet;
			}

			if (Board.Status == GameStatus.Win || Board.Status == GameStatus.Lose)
			{
				return MoveStatus.GameOver;
			}

			switch (direction)
			{
				case Direction.Down:
					return Move(CurrentPlayerData.Position.Row - 1, CurrentPlayerData.Position.Column);
				case Direction.Up:
					return Move(CurrentPlayerData.Position.Row + 1, CurrentPlayerData.Position.Column);
				case Direction.Right:
					return Move(CurrentPlayerData.Position.Row, CurrentPlayerData.Position.Column + 1);
				case Direction.Left:
					return Move(CurrentPlayerData.Position.Row, CurrentPlayerData.Position.Column - 1);
				default:
					return MoveStatus.Invalid;
			}
		}

		public MoveStatus Move(int row, int column)
		{
			if (Board == null || CurrentPlayerData == null)
			{
				return MoveStatus.GameError;
			}

			if (row < 0 || row >= Paramiters.Rows || column < 0 || column >= Paramiters.Columns)
			{
				return MoveStatus.Invalid;
			}

			bool mineHit = false;
			CurrentPlayerData.Score++;
			GameBoardCell cell = Board.GameBoardData[column][row];
			switch (cell.Status)
			{
				case CellStatus.Mine:
					CurrentPlayerData.Lives--;
					mineHit = true;
					cell.Status = CellStatus.MineExploded;
					break;
				case CellStatus.None:
					cell.Status = CellStatus.Visited;
					break;
			}
			if (CurrentPlayerData.Position == null)
			{
				CurrentPlayerData.Position = new CellPosition();
			}

			CurrentPlayerData.Position.Column = column;
			CurrentPlayerData.Position.Row = row;
			_gameMoves.Add(new GameBoardCell(cell));

			if (CurrentPlayerData.Lives < 1)
			{
				Board.Status = GameStatus.Lose;
				return MoveStatus.GameOver;
			}

			if (CurrentPlayerData.Position.Column == Paramiters.Columns -1)
			{
				Board.Status = GameStatus.Win;
				return MoveStatus.GameOver;
			}

			Board.Status = GameStatus.Running;
			return mineHit ? MoveStatus.LifeLost : MoveStatus.Sucsess;
		}

		public List<string> GetCurrentGameMoves()
		{
			return _gameMoves?.Select(m => m.ToPositionString()).ToList() ?? new List<string>();
		}
	}
}

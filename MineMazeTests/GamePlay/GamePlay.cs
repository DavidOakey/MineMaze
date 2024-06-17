using MineMaze;
using MineMaze.Models;
using MineMaze.Models.Enums;
using MineMaze.Models.Json;
using Newtonsoft.Json;

namespace MineMazeTests.GamePlay
{
	public class GamePlay
	{
		[Fact]
		public void Win()
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				//Create a game with no mines
				engine = MineMazeEngine.CreateGame(5, 5, 0, 1);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			
			Assert.Equal(MoveStatus.Sucsess, engine.FirstMove(1));
			// Move forwards
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Right));
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Right));
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Right));
			Assert.Equal(MoveStatus.GameOver, engine.Move(Direction.Right));
			Assert.Equal(GameStatus.Win, engine?.Status);
		}

		[Fact]
		public void LoseLife()
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				//Create a game with no mines
				engine = MineMazeEngine.CreateGame(JsonConvert.SerializeObject(new BoardJson
				{
					// New game with mines from Column 2 -8 of row 2
					Paramiters = new GameParamiters { Columns = 8, Rows = 8, Mines = 7, Lives = 5 },
					Mines = new List<GameBoardCell>{
					new GameBoardCell{Row =1, Column =1},
					new GameBoardCell{Row =1, Column =2},
					new GameBoardCell{Row =1, Column =3},
					new GameBoardCell{Row =1, Column =4},
					new GameBoardCell{Row =1, Column =5},
					new GameBoardCell{Row =1, Column =6},
					new GameBoardCell{Row =1, Column =7}
				}
				}));
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			// Start in the middle
			Assert.Equal(MoveStatus.Sucsess, engine.FirstMove(2));

			// Move forwards
			Assert.Equal(MoveStatus.LifeLost, engine.Move(Direction.Right));
			Assert.Equal(4, engine?.CurrentPlayerData?.Lives);
		}

		[Fact]
		public void LoseGame()
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				//Create a game with no mines
				engine = MineMazeEngine.CreateGame(JsonConvert.SerializeObject(new BoardJson
				{
					// New game with mines from Column 2 -8 of row 2
					Paramiters = new GameParamiters { Columns = 8, Rows = 8, Mines = 7, Lives = 5 },
					Mines = new List<GameBoardCell>{
					new GameBoardCell{Row =1, Column =1},
					new GameBoardCell{Row =1, Column =2},
					new GameBoardCell{Row =1, Column =3},
					new GameBoardCell{Row =1, Column =4},
					new GameBoardCell{Row =1, Column =5},
					new GameBoardCell{Row =1, Column =6},
					new GameBoardCell{Row =1, Column =7}
				}
				}));
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			// Start in the middle
			Assert.Equal(MoveStatus.Sucsess, engine.FirstMove(2));

			// Move forwards
			Assert.Equal(MoveStatus.LifeLost, engine.Move(Direction.Right));
			Assert.Equal(4, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(MoveStatus.LifeLost, engine?.Move(Direction.Right));
			Assert.Equal(3, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(MoveStatus.LifeLost, engine?.Move(Direction.Right));
			Assert.Equal(2, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(MoveStatus.LifeLost, engine?.Move(Direction.Right));
			Assert.Equal(1, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(MoveStatus.GameOver, engine?.Move(Direction.Right));
			Assert.Equal(0, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(GameStatus.Lose, engine?.Status);
		}
	}
}

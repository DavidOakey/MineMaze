using MineMaze;
using MineMaze.Models;

namespace MineMazeTests.GameCreation
{
	public class GameCreationTests
	{
		[Fact]
		public void DefaltBoard()
		{
			MineMazeEngine? engine = null;
			try
			{
				engine = MineMazeEngine.CreateGame();
			}
			catch
			{
			}
			Assert.Equal(8, engine?.Paramiters.Columns);
			Assert.Equal(8, engine?.Paramiters.Rows);
			Assert.Equal(10, engine?.Paramiters.Mines);
			Assert.Equal(5, engine?.Paramiters.Lives);
			Assert.Equal(5, engine?.CurrentPlayerData?.Lives);
		}

		[Theory]
		// Invalid size
		[InlineData(-1, 8, 10, 5, false, true)]
		[InlineData(0, 8, 10, 5, false, true)]
		[InlineData(1, 8, 10, 5, false, true)]
		[InlineData(51, 8, 10, 5, false, true)]
		[InlineData(8, -1, 10, 5, false, true)]
		[InlineData(8, 0, 10, 5, false, true)]
		[InlineData(8, 1, 10, 5, false, true)]
		[InlineData(8, 51, 10, 5, false, true)]
		[InlineData(-1, 1, 10, 5, false, true)]
		[InlineData(0, 51, 10, 5, false, true)]
		[InlineData(1, 1, 10, 5, false, true)]
		[InlineData(51, 51, 10, 5, false, true)]

		// invalid number of mines
		[InlineData(2, 2, 10, 5, false, true)]

		// Valid boards
		[InlineData(2, 2, 1, 5, true, false)]
		[InlineData(2, 50, 10, 5, true, false)]
		[InlineData(50, 2, 10, 5, true, false)]
		[InlineData(50, 50, 10, 5, true, false)]

		// Invalid lives
		[InlineData(8, 8, 10, 0, false, true)]
		public void BoardSize(int rows, int columns, int mines, int lives, bool validEngine, bool exceptionThrown)
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				engine = MineMazeEngine.CreateGame(rows, columns, mines, lives);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			Assert.Equal(validEngine, engine != null);
			Assert.Equal(exceptionThrown, exception != null);
		}

		// Invalid mine numbers tested with board creation
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		public void MineCount(int mines)
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				engine = MineMazeEngine.CreateGame(50, 50, mines, 1);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			List<GameBoardCell>? mineList = engine?.Board?.GameBoardData.SelectMany(c => c.Value
				.Where(cell => cell.Status == MineMaze.Models.Enums.CellStatus.Mine)).ToList();
			Assert.Equal(mines, mineList?.Count);
		}
	}
}

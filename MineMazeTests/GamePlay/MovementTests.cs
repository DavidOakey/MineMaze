using MineMaze;
using MineMaze.Models.Enums;

namespace MineMazeTests.GamePlay
{
	public class MovementTests
	{
		[Fact]
		public void StandardMovement()
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				//Create a game with no mines
				engine = MineMazeEngine.CreateGame(7, 7, 0, 1);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			// Start in the middle
			Assert.Equal(MoveStatus.Sucsess, engine.FirstMove(3));

			// Move forwards
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Right));

			// Move back
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Left));

			// Move up
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Up));

			// Move down
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Down));
		}

		[Fact]		
		public void BoardBoundarys()
		{
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				//Create a game with no mines
				engine = MineMazeEngine.CreateGame(2, 2, 0, 1);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			// Start in the bottom
			Assert.Equal(MoveStatus.Sucsess, engine.FirstMove(1));

			// Move Back past start
			Assert.Equal(MoveStatus.Invalid, engine.Move(Direction.Left));

			// Move down passed bottom
			Assert.Equal(MoveStatus.Invalid, engine.Move(Direction.Down));

			// Move up passed top
			Assert.Equal(MoveStatus.Sucsess, engine.Move(Direction.Up));
			Assert.Equal(MoveStatus.Invalid, engine.Move(Direction.Up));

			// Moving passed the right of the game board is not posible as entring the last coloum triggers a win condition
		}
	}
}

using MineMaze;
using MineMaze.Models;
using MineMaze.Models.Enums;

namespace MineMazeTests.SaveLoad
{
	public class SaveLoadTests
    {
        [Fact]
        public void RetriveSaveString()
        {
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				engine = MineMazeEngine.CreateGame();
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			Assert.NotEmpty(engine.Save());
		}

        // Create a random game.
        // Make some moves.
        // Strore paramiters
        // Save
        // Create a new game with the save string
        // Compair paramiters
        [Fact]
        public void SaveAndReloadGame()
        {
			MineMazeEngine? engine = null;
			Exception? exception = null;
			try
			{
				engine = MineMazeEngine.CreateGame();
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			// Make Moves
			engine.FirstMove(2);
			engine.Move(Direction.Right);
			engine.Move(Direction.Right);
			engine.Move(Direction.Up);
			engine.Move(Direction.Left);

			// Save Data
			GameParamiters paramiters = engine.Paramiters;
			PlayerData? player = engine?.CurrentPlayerData;
			List<string>? moveList = engine?.GetCurrentGameMoves();
			GameStatus? gameStatus = engine?.Status;

			string saveString = engine?.Save() ?? string.Empty;
			Assert.NotEmpty(saveString);

			engine = null;
			exception = null;
			try
			{
				engine = MineMazeEngine.Load(saveString);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			Assert.NotNull(engine);
			Assert.Null(exception);

			Assert.Equal(paramiters.Columns, engine?.Paramiters.Columns);
			Assert.Equal(paramiters.Rows, engine?.Paramiters.Rows);
			Assert.Equal(paramiters.Mines, engine?.Paramiters.Mines);
			Assert.Equal(paramiters.Lives, engine?.Paramiters.Lives);

			Assert.Equal(player?.Lives, engine?.CurrentPlayerData?.Lives);
			Assert.Equal(player?.Score, engine?.CurrentPlayerData?.Score);
			Assert.Equal(player?.Position, engine?.CurrentPlayerData?.Position, (e,a)=>e?.Column == a?.Column && e?.Row == a?.Row);

			Assert.Equal(moveList, engine?.GetCurrentGameMoves());

			Assert.Equal(gameStatus, engine?.Status);
		}
	}
}

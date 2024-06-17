using MineMaze.Models.Enums;

namespace MineMaze.Models
{
	public class GameBoard
	{
		public GameStatus Status { get; set; }
		public required Dictionary<int, List<GameBoardCell>> GameBoardData { get; set; }
		public required List<GameBoardCell> Mines { get; set; }
	}
}

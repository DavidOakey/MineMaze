using MineMaze.Models.Enums;

namespace MineMaze.Models.Json
{
	public class GameSaveJson
	{
		public BoardJson? Board { get; set; }
		public GameStatus Status { get; set; }
		public PlayerData? CurrentPlayerData { get; set; }
		public List<GameBoardCell>? Moves { get; set; }
	}
}

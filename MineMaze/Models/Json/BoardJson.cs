namespace MineMaze.Models.Json
{
	public class BoardJson
	{
		public GameParamiters? Paramiters {  get; set; }
		public List<GameBoardCell>? Mines { get; set; }
	}
}

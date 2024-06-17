using MineMaze.Models.Enums;

namespace MineMaze.Models
{
	public class GameBoardCell
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public CellStatus Status {get;set;}

		public GameBoardCell()
		{

		}
		public GameBoardCell(GameBoardCell cell)
		{
			this.Row = cell.Row;
			this.Column = cell.Column;
			this.Status = cell.Status;
		}
	}
}

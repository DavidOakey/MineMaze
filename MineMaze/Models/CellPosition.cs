namespace MineMaze.Models
{
	public class CellPosition
	{
		public int Row { get; set; }
		public int Column { get; set; }

		public CellPosition()
		{

		}

		public CellPosition(CellPosition position)
		{
			Row = position.Row;
			Column = position.Column;
		}		
	}
}

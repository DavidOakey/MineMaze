using MineMaze.Models;

namespace MineMaze.Extentions
{
	public static class GameBoardCellExtensions
	{
		public static string ToPositionString(this GameBoardCell cell)
		{
			return string.Format("{0}{1}", IndexToString(cell.Column), cell.Row + 1);
		}

		private static string IndexToString(int index)
		{
			string returnString = string.Empty;
			int mod;

			while (index > 0 || returnString == string.Empty)
			{
				mod = index % 26;
				returnString = (char)('A' + mod) + returnString;
				index = (index - mod) / 26;
			}
			return returnString;
		}
	}
}

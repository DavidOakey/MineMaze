using MineMaze.Models.Enums;

namespace MineMaze.Extentions
{
	public static class GameStatusExtensions
	{
		public static string Discription(this GameStatus status)
		{
			switch (status)
			{
				case GameStatus.Error:
					return "Something went wrong";
				case GameStatus.Pending:
					return "Game ready!";
				case GameStatus.Running:
					return "In progress";
				case GameStatus.Win:
					return "Won";
				case GameStatus.Lose:
					return "Lost";
				case GameStatus.ErrorParamitersNotSet:
					return "Game Paramiters Not Set";
				default:
					return "Unknown";
			}
		}
	}
}

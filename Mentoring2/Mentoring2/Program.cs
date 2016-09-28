using System;

namespace Mentoring2
{
	class Program
	{
		static void Main(string[] args)
		{
			var authorInfo = GetAuthorInfo();

			Console.WriteLine(authorInfo);
			Console.ReadKey();
		}

		private static string GetAuthorInfo()
		{
			var author = new Author();
			var authorInfo = author.GetInformation();
			return authorInfo;
		}
	}
}

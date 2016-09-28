using System;

namespace Mentoring3
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.Write("Enter source string: ");
			var source = Console.ReadLine();

			try
			{
				var res = source.ToInt();
				Console.WriteLine("Res value: {0}", res);
				Console.WriteLine("Squared res value: {0}", res*res);
			}

			catch (ArgumentNullException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (FormatException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (OverflowException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadKey();
		}
	}
}

using System;

namespace Mentoring3
{
	public static class MySuperStringConverter
	{
		public static int ToInt(this string source)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (source.Length == 0) throw new FormatException("Source should not be empty");

			var result = 0;
			for (var i = 0; i < source.Length; i++)
			{
				var c = source[i];
				if (c >= '0' && c <= '9')
				{
					result = result*10 + c - '0';
				}
				else
				{
					throw new OverflowException();
				}
			}

			return result;
		}
	}
}

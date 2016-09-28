using System.Text;

namespace Mentoring2
{
	[Author("Andrei Kasak", "Andrei_Kasak@epam.com")]
	public class Author
	{
		public string GetInformation()
		{
			var type = typeof(Author);
			var attributes = type.GetCustomAttributes(false);
			var sb = new StringBuilder();
			foreach (AuthorAttribute item in attributes)
			{
				sb.AppendLine(item.ToString());
			}

			return sb.ToString();
		}
	}
}
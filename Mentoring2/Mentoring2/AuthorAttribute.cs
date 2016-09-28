using System;

namespace Mentoring2
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AuthorAttribute : Attribute
	{
		private readonly string _name;
		private readonly string _email;

		public AuthorAttribute()
		{
			
		}

		public AuthorAttribute(string name, string email)
		{
			_name = name;
			_email = email;
		}

		public override string ToString()
		{
			return string.Format("Author is: {0} ({1})", _name, _email);
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentoring1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			//var s = new Source();
			//var infoDatas = InfoDatas();

			//var sw = new Stopwatch();
			//sw.Start();
			//Console.WriteLine("Start");
			//s.CheckAndProceed(new User {Datas = infoDatas.ToArray()});
			//sw.Stop();
			//Console.WriteLine("Stop: {0}", sw.ElapsedMilliseconds);


			int a = 10;
			int? b = null;
			var c = a + b;

			Console.WriteLine(c);

			Console.ReadKey();
		}


		public sealed class MySecondClass { }

		private static List<IUserInfoData> InfoDatas()
		{
			var source = new List<IUserInfoData>();
			for (var i = 0; i < 1000000; i++)
			{
				source.Add(new UserInfoData
				{
					FirstName = "First " + i.ToString(),
					LastName = "Last " + i.ToString()
				});
			}
			return source;
		}
	}

	public interface IUserInfoData
	{
		string FirstName { get; set; }
		string LastName { get; set; }
	}

	internal struct UserInfoData : IUserInfoData
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class User : IUser
	{
		public IUserInfoData[] Datas { get; set; }
	}

	public interface IUser
	{
		IUserInfoData[] Datas { get; set; }
	}

	internal class Source
	{
		internal void CheckAndProceed(IUser data)
		{
			var dest = new Destination();

			dest.ProceedData(data);
		}
	}

	internal class Destination
	{
		internal void ProceedData(IUser data)
		{
			foreach (var item in data.Datas)
			{
				var temp = item.FirstName;
				item.FirstName = item.LastName;
				item.LastName = temp;
			}
		}
	}
}

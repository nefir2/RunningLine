using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace running_line
{
	internal class Program
	{
		static void Main()
		{
			RunLine.Run();
		}
		private static void Start()
		{
			while (true)
			{
				try
				{
					Console.WriteLine("введите текст для бегущей строки.");
					string text = Console.ReadLine();
					Console.Clear();
					new RunLine(text).Run();
				}
				catch (Exception ex)
				{
					Console.WriteLine("вы добились ошибки. поздравляю.\n" + ex.Message + "\n\n");
					continue;
				}
			}
		}
	}
}
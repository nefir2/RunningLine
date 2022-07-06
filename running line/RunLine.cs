using System;
using System.Text;
using System.Threading;
namespace running_line
{
	/// <summary>
	/// бегущая строка.
	/// </summary>
	public class RunLine
	{
		#region constants
		/// <summary>
		/// начальная строка программы.
		/// </summary>
		private const string textrunner = "это стандартная строка для теста программы. ";
		/// <summary>
		/// начальное время между перемещением букв.
		/// </summary>
		private const int timerunner = 70;
		/// <summary>
		/// начальное направление перемещения букв.
		/// </summary>
		private const bool TOlefter = true;
		/// <summary>
		/// начальная стартовая позиция, по горизонтали.
		/// </summary>
		private const int startlefter = 0;
		/// <summary>
		/// начальная стартовая позиция, по вертикали.
		/// </summary>
		private const int starttopper = 0;
		#endregion
		#region fields
		/// <summary>
		/// текст который будет бегать.
		/// </summary>
		private string text;
		/// <summary>
		/// время ожидания после очередного перебежавшего текста.
		/// </summary>
		private int afk;
		/// <summary>
		/// выбор в какую сторону должен бежать текст.
		/// </summary>
		private bool toleft;
		/// <summary>
		/// стартовая позиция текста по столбцам.
		/// </summary>
		private int leftpos;
		/// <summary>
		/// стартовая позиция текста по строкам.
		/// </summary>
		private int toppos;
		#endregion
		#region properties
		/// <summary>
		/// строка которая будет бегать.
		/// </summary>
		public string Text
		{
			get => text; 
			set => text = value; 
		}
		/// <summary>
		/// свойство для времени ожидания.
		/// </summary>
		/// <remarks>
		/// после последнего перемещения буквы, воспроизводится ожидание указанное здесь.
		/// </remarks>
		/// <value>
		/// значение не может быть бесконечным, или меньше 0.
		/// </value>
		/// <returns>
		/// время ожидания объекта.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public int Afk
		{
			get => afk; 
			set
			{
				if (value < 0 || value == Timeout.Infinite) throw new ArgumentOutOfRangeException(nameof(this.Afk), "время ожидания было указано отрицательным или бесконечным.");
				afk = value; 
			}
		}
		/// <summary>
		/// свойство для обозначения в какую сторону должен бежать текст.
		/// </summary>
		/// <value>
		/// если <see langword="value"/> равен <see langword="true"/>, то видимое направление перемещения текста - налево. <br/>
		/// иначе при <see langword="value"/> равном <see langword="false"/> видимое направление будет направо.
		/// </value>
		/// <returns>
		/// направление перемещения текста.
		/// </returns>
		public bool Toleft
		{
			get => toleft;
			set => toleft = value; 
		}
		/// <summary>
		/// стартовая позиция текста по столбцам.
		/// </summary>
		/// <value>
		/// если <see langword="value"/> и длина строки <see cref="Text"/> вместе будут больше <see cref="Console.BufferWidth"/>, то выбросится исключение.
		/// </value>
		/// <returns>
		/// значение начала строки в объекте.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public int Leftpos
		{
			get => leftpos;
			set
			{
				if (value + text.Length > Console.BufferWidth) throw new ArgumentOutOfRangeException(nameof(this.Leftpos), "значение вместе со строкой были больше буфферной зоны по ширине.");
				leftpos = value;
			}
		}
		/// <summary>
		/// стартовая позиция текста по строкам.
		/// </summary>
		/// <value>
		/// если <see langword="value"/> было больше <see cref="Console.BufferHeight"/>, то выбрасывается исключение.
		/// </value>
		/// <returns>
		/// номер строки в которой находится бегущая строка.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public int Toppos
		{
			get => toppos;
			set
			{
				if (value > Console.BufferHeight) throw new ArgumentOutOfRangeException(nameof(this.Toppos), "значение было больше буфферной зоны по высоте.");
				toppos = value;
			}
		}
		#endregion
		#region constructors
		/// <summary>
		/// конструктор объекта <see cref="RunLine"/>.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <param name="afk">время ожидания между перебежками буквы.</param>
		/// <param name="toleft">
		/// направление бега текста. <br/>
		/// если <see langword="true"/>, текст бежит справа налево. <br/>
		/// иначе при <see langword="false"/> текст бежит слева направо.
		/// </param>
		/// <param name="leftpos">позиция текста по столбцам.</param>
		/// <param name="toppos">позиция текста по строкам.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text = textrunner, int afk = timerunner, bool toleft = TOlefter, int leftpos = startlefter, int toppos = starttopper)
		{
			Text = text;
			Toleft = toleft;
			try
			{
				Afk = afk;
				Leftpos = leftpos;
				Toppos = toppos;
			}
			catch (ArgumentOutOfRangeException ex)
			{
				throw new ArgumentOutOfRangeException(ex.ParamName, "один, или несколько, из указанных параметров не попали в допустимые значения.");
			}
		}
		#endregion
		#region methods
		/// <summary>
		/// запуск бегущей строки.
		/// </summary>
		public void Run() 
		{
			Console.SetCursorPosition(leftpos, toppos);
			Console.CursorVisible = false;
			for (int i = 0; true; i++)
			{
				Console.Write(text);
				Thread.Sleep(afk);
				if (!toleft) EndToStart(ref text);
				else StartToEnd(ref text);
				Console.SetCursorPosition(leftpos, toppos);
				//ClearLine(toppos, leftpos, text.Length + 1);
			}
		}
		/// <summary>
		/// запуск бегущей строки.
		/// </summary>
		/// <remarks>
		/// создание бегущей строки через параметры.
		/// </remarks>
		/// <param name="text">исходная строка, которая должна бежать.</param>
		/// <param name="afk">
		/// скорость перемещения строки. в миллисекундах.<br/>
		/// задержка между сменой строки на изменённую позицию буквы.
		/// </param>
		/// <param name="toleft">
		/// направление бега строки. <br/>
		/// если <see langword="true"/>, то строка бежит справа налево. <br/>
		/// иначе при <see langword="false"/> бежит слева направо.
		/// </param>
		/// <param name="leftpos">начальная позиция по горизонтали.</param>
		/// <param name="toppos">начальная позиция по вертикали.</param>
		/// <returns>объект бегущей строки.</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public static RunLine Run(string text = textrunner, int afk = timerunner, bool toleft = TOlefter, int leftpos = startlefter, int toppos = starttopper) 
		{
			RunLine rl = new RunLine(text, afk, toleft, leftpos, toppos);
			rl.Run();
			return rl;
		}
		/// <summary>
		/// запуск бегущей строки.
		/// </summary>
		/// <remarks> 
		/// ввод строки для вывода через консоль. <br/>
		/// остальные поля объекта остаются стандартными.
		/// </remarks>
		/// <returns>объект бегущей строки запущенной в консоли.</returns>
		public static RunLine ConsoleRun(string inputMessage, string exceptionMessage)
		{
			while (true)
			{
				try
				{
					Console.WriteLine(inputMessage);
					string text = Console.ReadLine();
					Console.Clear();
					RunLine rl = new RunLine(text);
					rl.Run();
					return rl;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"{exceptionMessage}\n{ex.Message}\n\n");
					continue;
				}
			}
		}
		/// <summary>
		/// перемещение знака с начала в конец.
		/// </summary>
		/// <remarks>
		/// берётся первый символ строки, вставляется копия после последнего символа, и удаляется с начала.
		/// </remarks>
		/// <param name="text">исходная и возвращаемая строка.</param>
		/// <returns>строка с перемещённым символом с начала в конец.</returns>
		public static void StartToEnd(ref string text)
		{
			StringBuilder str = new(text);
			str.Insert(str.Length, str[0]);
			str.Remove(0, 1);
			text = str.ToString();
		}
		/// <summary>
		/// перемещение знака с конца в начало.
		/// </summary>
		/// <remarks>
		/// берётся последний символ строки, вставляется копия перед первым символом, и удаляется с конца.
		/// </remarks>
		/// <param name="text">исходная и возвращаемая строка.</param>
		/// <returns>строка с перемещённым символом с конца в начало.</returns>
		public static void EndToStart(ref string text)
		{
			StringBuilder str = new(text);
			str.Insert(0, str[^1]);
			str.Remove(str.Length - 1, 1);
			text = str.ToString();
		}
		/// <summary>
		/// очистка первой строки до указанной длины.
		/// </summary>
		/// <remarks>
		/// если стартовая точка в ширину <br/>
		/// и длина строки вместе превысят <see cref="Console.BufferWidth"/>, <br/>
		/// выбросится исключение.
		/// </remarks>
		/// <param name="top">стартовая позиция строк.</param>
		/// <param name="left">стартовая позиция столбцов.</param>
		/// <param name="length">число знаков которые нужно очистить.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static void ClearLine(int left, int top, int length)
		{
			if (left + length > Console.BufferWidth) throw new ArgumentOutOfRangeException(nameof(left));
			for (int i = left; i < length; i++)
			{
				Console.SetCursorPosition(i, top);
				Console.Write(' ');
			}
			Console.SetCursorPosition(left, top);
		}
		#endregion
	}
}
//метод static Run
//
//Console.SetCursorPosition(leftpos, toppos);
//Console.CursorVisible = false;
//for (int i = 0; true; i++)
//{
//	Console.Write(text);
//	Thread.Sleep(afk);
//	if (!toleft) EndToStart(ref text);
//	else StartToEnd(ref text);
//	Console.SetCursorPosition(leftpos, toppos);
//	//ClearLine(text.Length + 1, leftpos, toppos);
//}
//
//
//более ненужные конструкторы.
//
//
//
///// <summary>
///// конструктор без параметров.
///// </summary>
///// <remarks>
///// устанавливает стандартные значения.
///// </remarks>
//public RunLine()									 : this(textrunner, timerunner, TOlefter, startlefter, starttopper) { }
///// <summary>
///// конструктор с параметром для текста.
///// </summary>
///// <param name="text">текст который будет бегать.</param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(string text)							 : this(text, timerunner, TOlefter, startlefter, starttopper) { }
///// <summary>
///// конструктор с параметром для времени ожидания.
///// </summary>
///// <param name="afk">время ожидания между перебежками буквы.</param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(int afk)								 : this(textrunner, afk) { }
///// <summary>
///// конструктор с параметром для направления перемещения.
///// </summary>
///// <param name="toleft">
///// направление бега текста. <br/>
///// если <see langword="true"/>, текст бежит справа налево. <br/>
///// иначе при <see langword="false"/> текст бежит слева направо.
///// </param>
//public RunLine(bool toleft)							 : this(textrunner, toleft) { }
///// <summary>
///// конструктор с параметрами для начальной позиции текста. 
///// </summary>
///// <param name="leftpos">позиция текста по столбцам.</param>
///// <param name="toppos">позиция текста по строкам.</param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(int leftpos, int toppos)				 : this(textrunner, timerunner, TOlefter, leftpos, toppos) { }
///// <summary>
///// конструктор с параметрами для текста и времени ожидания.
///// </summary>
///// <param name="text">текст который будет бегать.</param>
///// <param name="afk">время ожидания между перебежками буквы.</param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(string text, int afk)				 : this(text, afk, TOlefter, startlefter, starttopper) { }
///// <summary>
///// конструктор с параметрами для текста и направления перемещения.
///// </summary>
///// <param name="text">текст который будет бегать.</param>
///// <param name="toleft">
///// направление бега текста. <br/>
///// если <see langword="true"/>, текст бежит справа налево. <br/>
///// иначе при <see langword="false"/> текст бежит слева направо.
///// </param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(string text, bool toleft)			 : this(text, timerunner, toleft, startlefter, starttopper) { }
///// <summary>
///// конструктор с параметрами для текста, времени ожидания и направления перемещения.
///// </summary>
///// <param name="text">текст который будет бегать.</param>
///// <param name="afk">время ожидания между перебежками буквы.</param>
///// <param name="toleft">
///// направление бега текста. <br/>
///// если <see langword="true"/>, текст бежит справа налево. <br/>
///// иначе при <see langword="false"/> текст бежит слева направо.
///// </param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(string text, int afk, bool toleft)    : this(text, afk, toleft, startlefter, starttopper) { }
///// <summary>
///// конструктор с параметром для текста и начальными позициями текста.
///// </summary>
///// <param name="text">текст который будет бегать.</param>
///// <param name="leftpos">позиция текста по столбцам.</param>
///// <param name="toppos">позиция текста по строкам.</param>
///// <exception cref="ArgumentOutOfRangeException"></exception>
//public RunLine(string text, int leftpos, int toppos) : this(text, timerunner, TOlefter, leftpos, toppos) { }
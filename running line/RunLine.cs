using System;
using System.Text;
using System.Threading;

namespace running_line
{
	public class RunLine
	{
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

		//стандартные значения для конструкторов.
		private const string textrunner = "это стандартная строка для теста программы. ";
		private const int timerunner = 70;
		private const bool TOlefter = true;
		private const int startlefter = 0;
		private const int starttopper = 0;
		#endregion
		#region properties
		/// <summary>
		/// свойство для значения бегущей строки.
		/// </summary>
		public string Text
		{
			get { return text; }
			set { text = value; }
		}
		/// <summary>
		/// свойство для времени ожидания.
		/// </summary>
		/// <remarks>
		/// после последнего перемещения буквы, воспроизводится ожидание указанное здесь.
		/// </remarks>
		public int Afk
		{
			get { return afk; }
			set { afk = value; }
		}
		/// <summary>
		/// свойство для обозначения в какую сторону должен бежать текст.
		/// </summary>
		public bool Toleft
		{
			get { return toleft; }
			set { toleft = value; }
		}
		/// <summary>
		/// стартовая позиция текста по столбцам.
		/// </summary>
		public int Leftpos
		{
			get { return leftpos; }
			set
			{
				if (value + text.Length > Console.BufferWidth) throw new ArgumentOutOfRangeException(nameof(value));
				leftpos = value;
			}
		}
		/// <summary>
		/// стартовая позиция текста по строкам.
		/// </summary>
		public int Toppos
		{
			get { return toppos; }
			set
			{
				if (value > Console.BufferHeight) throw new ArgumentOutOfRangeException(nameof(value));
				toppos = value;
			}
		}
		#endregion
		#region constructors
		/// <summary>
		/// конструктор без параметров.
		/// </summary>
		/// <remarks>
		/// устанавливает стандартные значения.
		/// </remarks>
		public RunLine()									 : this(textrunner, timerunner, TOlefter, startlefter, starttopper) { }
		/// <summary>
		/// конструктор с параметром для текста.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text)							 : this(text, timerunner, TOlefter, startlefter, starttopper) { }
		/// <summary>
		/// конструктор с параметром для времени ожидания.
		/// </summary>
		/// <param name="afk">время ожидания между перебежками буквы.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(int afk)								 : this(textrunner, afk) { }
		/// <summary>
		/// конструктор с параметром для направления перемещения.
		/// </summary>
		/// <param name="toleft">
		/// направление бега текста. <br/>
		/// если <see langword="true"/>, текст бежит справа налево. <br/>
		/// иначе при <see langword="false"/> текст бежит слева направо.
		/// </param>
		public RunLine(bool toleft)							 : this(textrunner, toleft) { }
		/// <summary>
		/// конструктор с параметрами для начальной позиции текста. 
		/// </summary>
		/// <param name="leftpos">позиция текста по столбцам.</param>
		/// <param name="toppos">позиция текста по строкам.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(int leftpos, int toppos)				 : this(textrunner, timerunner, TOlefter, leftpos, toppos) { }
		/// <summary>
		/// конструктор с параметрами для текста и времени ожидания.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <param name="afk">время ожидания между перебежками буквы.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text, int afk)				 : this(text, afk, TOlefter, startlefter, starttopper) { }
		/// <summary>
		/// конструктор с параметрами для текста и направления перемещения.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <param name="toleft">
		/// направление бега текста. <br/>
		/// если <see langword="true"/>, текст бежит справа налево. <br/>
		/// иначе при <see langword="false"/> текст бежит слева направо.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text, bool toleft)			 : this(text, timerunner, toleft, startlefter, starttopper) { }
		/// <summary>
		/// конструктор с параметрами для текста, времени ожидания и направления перемещения.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <param name="afk">время ожидания между перебежками буквы.</param>
		/// <param name="toleft">
		/// направление бега текста. <br/>
		/// если <see langword="true"/>, текст бежит справа налево. <br/>
		/// иначе при <see langword="false"/> текст бежит слева направо.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text, int afk, bool toleft)    : this(text, afk, toleft, startlefter, starttopper) { }
		/// <summary>
		/// конструктор с параметром для текста и начальными позициями текста.
		/// </summary>
		/// <param name="text">текст который будет бегать.</param>
		/// <param name="leftpos">позиция текста по столбцам.</param>
		/// <param name="toppos">позиция текста по строкам.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public RunLine(string text, int leftpos, int toppos) : this(text, timerunner, TOlefter, leftpos, toppos) { }
		/// <summary>
		/// конструктор всех полей.
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
		public RunLine(string text, int afk, bool toleft, int leftpos, int toppos)
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
		public static void Run(string text, int afk = timerunner, bool toleft = TOlefter, int leftpos = startlefter, int toppos = starttopper) 
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
				//ClearLine(text.Length + 1, leftpos, toppos);
			}
		}
		/// <summary>
		/// перемещает знак с начала в конец.
		/// </summary>
		/// <param name="text">исходная строка.</param>
		public static void StartToEnd(ref string text)
		{
			StringBuilder str = new(text);
			str.Insert(str.Length, str[0]);
			str.Remove(0, 1);
			text = str.ToString();
		}
		/// <summary>
		/// перемещает знак с конца в начало.
		/// </summary>
		/// <param name="text">исходная строка.</param>
		/// <returns>строка, где последний символ стал первым, а с конца он удалён.</returns>
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
			if (left + length > Console.BufferWidth) throw new ArgumentOutOfRangeException();
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
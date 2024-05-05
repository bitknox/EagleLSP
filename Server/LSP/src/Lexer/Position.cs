namespace LSP.Lexer;
public class Position(int line = 1, int column = 0)
{
	public int Line { get; set; } = line;
	public int Column { get; set; } = column;

	public int Idx { get; set; } = 0;

	public void Advance()
	{
		Idx++;
		Column++;
	}

	public void NewLine()
	{
		Idx++;
		Line++;
		Column = 0;
	}

	public override string ToString()
	{
		return $"Position(line: {Line}, column: {Column})";
	}

	public override bool Equals(object? obj)
	{
		if (obj is Position other)
		{
			return Line == other.Line && Column == other.Column;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Line, Column);
	}
}
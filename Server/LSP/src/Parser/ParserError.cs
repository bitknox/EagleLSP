using LSP.Lexer;

namespace LSP.Parser;

public class ParserError(string message, Position position, int length) 
{
	public string Message { get; set; } = message;
	public Position Position { get; set; } = position;
	public int Length { get; set; } = length;


	public override string ToString()
	{
		return $"ParserError({Message}, {Position})";
	}

	public override bool Equals(object? obj)
	{
		if (obj is ParserError other)
		{
			return Message == other.Message && Position == other.Position;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Message, Position);
	}
}
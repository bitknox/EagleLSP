namespace LSP.Lexer;

using System.Security.Cryptography;
using System.Text;

public class Lexer(string source)
{
	private readonly Position position = new();

	private readonly string source = source;

	private readonly List<Token> tokens = [];

	private static readonly Dictionary<string, TokenType> keywords = new()
	{
		{ "true", TokenType.BOOLEAN },
		{ "false", TokenType.BOOLEAN },
		{ "null", TokenType.NULL },
		{ "undefined", TokenType.UNDEFINED }
	};

	private Position startPosition = new();

	public List<Token> Lex()
	{
		while (!IsAtEnd())
		{
			LexToken();
		}

		return tokens;
	}

	private void LexToken()
	{
		char currentChar = Advance();
		startPosition = new Position(position.Line, position.Column);
		switch (currentChar)
		{
			case '{':
				AddToken(TokenType.LEFT_BRACKET);
				break;
			case '}':
				AddToken(TokenType.RIGHT_BRACKET);
				break;
			case '[':
				AddToken(TokenType.LEFT_BRACE);
				break;
			case ']':
				AddToken(TokenType.RIGHT_BRACE);
				break;
			case ',':
				AddToken(TokenType.COMMA);
				break;
			case ':':
				AddToken(TokenType.COLON);
				break;
			case ' ':
			case '\r':
			case '\t':
				break;
			case '\n':
				position.NewLine();
				break;
			case '"':
				LexString();
				break;
			default:
				if (char.IsDigit(currentChar))
				{
					LexNumber(currentChar);
				}
				else
				{
					LexLiteral(currentChar);
				}
				break;
		}
	}

	private void LexLiteral(char current)
	{
		StringBuilder value = new(current.ToString());

		while (char.IsLetter(Peek()))
		{
			value.Append(Advance());
		}

		string valueStr = value.ToString().ToLower();

		if (keywords.TryGetValue(valueStr, out TokenType v))
		{
			AddToken(v, valueStr);
		}
		else
		{
			throw new Exception($"Unexpected token: '{valueStr}'");
		}

	}

	private void LexNumber(char currentChar)
	{
		StringBuilder value = new(currentChar.ToString());

		while (char.IsDigit(Peek()))
		{
			value.Append(Advance());
		}

		if (Peek() == '.' && char.IsDigit(PeekNext()))
		{
			value.Append(Advance());

			while (char.IsDigit(Peek()))
			{
				value.Append(Advance());
			}
		}

		AddToken(TokenType.NUMBER, value.ToString());
	}

	private void LexString()
	{
		StringBuilder value = new();

		while (Peek() != '"' && !IsAtEnd())
		{
			char currentChar = Advance();

			if (currentChar == '\\')
			{
				char nextChar = Advance();

				switch (nextChar)
				{
					case 'n':
						position.Advance();
						value.Append('\n');
						break;
					case 't':
						value.Append('\t');
						break;
					case 'r':
						value.Append('\r');
						break;
					case 'b':
						value.Append('\b');
						break;
					case 'f':
						value.Append('\f');
						break;
					case '\\':
						value.Append('\\');
						break;
					case '"':
						value.Append('"');
						break;
					default:
						throw new Exception($"Invalid escape sequence: '\\{nextChar}'");
				}
			}
			else
			{
				value.Append(currentChar);
			}
		}

		if (IsAtEnd())
		{
			throw new Exception("Unterminated string");
		}

		AddToken(TokenType.STRING, value.ToString());
		Advance();
	}

	private char PeekNext()
	{
		if (position.Idx + 1 >= source.Length)
		{
			return '\0';
		}

		return source[position.Idx + 1];
	}

	private char Peek()
	{
		if (IsAtEnd())
		{
			return '\0';
		}

		return source[position.Idx];
	}

	private char Advance()
	{
		char currentChar = source[position.Idx];
		position.Advance();
		return currentChar;
	}

	private void AddToken(TokenType type, string? value = null)
	{
		tokens.Add(new Token(type, value, new Position(startPosition.Line, startPosition.Column)));
	}

	private bool IsAtEnd()
	{
		return position.Idx >= source.Length;
	}

}
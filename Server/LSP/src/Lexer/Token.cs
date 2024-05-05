namespace LSP.Lexer;
public class Token(TokenType type, string? value, Position position)
{
    public TokenType Type { get; } = type;
    public string? Value { get; } = value;
    public Position Position { get; } = position;

    public override string ToString()
    {
        return $"Token({Type}, {Value}, Line: {Position.Line}, Column: {Position.Column})";
    }
}
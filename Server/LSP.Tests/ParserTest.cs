namespace LSP.Tests;
using LSP.Lexer;
using LSP.Parser;

public class ParserTests
{
		[Fact]
		public void TestParse()
		{
				// Arrange
				var tokens = new List<Token>
				{
						new(TokenType.LEFT_BRACKET, "{", new Position()),
						new(TokenType.STRING, "name", new Position()),
						new(TokenType.COLON, ":", new Position()),
						new(TokenType.STRING, "benchmarks", new Position()),
						new(TokenType.COMMA, ",", new Position()),
						new(TokenType.STRING, "name", new Position()),
						new(TokenType.COLON, ":", new Position()),
						new(TokenType.NUMBER, "1.2", new Position()),
						new(TokenType.RIGHT_BRACKET, "}", new Position())
				};
				var parser = new Parser(tokens);
				
				// Act
				var errors = parser.Parse();
				
				// Assert
				Assert.Empty(errors);
		}
}
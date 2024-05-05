namespace LSP.Tests;
using LSP.Lexer;
public class LexerTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var lexer = new Lexer("{\"key\": \"value\", \"anotherKey\": 1.2}");
        // Act
        var tokens = lexer.Lex();
        // Assert
        Assert.Equal(9, tokens.Count);
        Assert.Equal(TokenType.LEFT_BRACKET, tokens[0].Type);
        Assert.Equal(TokenType.STRING, tokens[1].Type);
        Assert.Equal(TokenType.COLON, tokens[2].Type);
        Assert.Equal(TokenType.STRING, tokens[3].Type);
        Assert.Equal(TokenType.COMMA, tokens[4].Type);
        Assert.Equal(TokenType.STRING, tokens[5].Type);
        Assert.Equal(TokenType.COLON, tokens[6].Type);
        Assert.Equal(TokenType.NUMBER, tokens[7].Type);
        Assert.Equal("1.2", tokens[7].Value);
        Assert.Equal(TokenType.RIGHT_BRACKET, tokens[8].Type);
    }

    [Fact]
    public void TestLexLiteral() {
        // Arrange
        var lexer = new Lexer("{\"key\": true, \"anotherKey\": false, \"nullKey\": null}");
        // Act
        var tokens = lexer.Lex();
        // Assert
        Assert.Equal(13, tokens.Count);
        Assert.Equal(TokenType.LEFT_BRACKET, tokens[0].Type);
        Assert.Equal(TokenType.STRING, tokens[1].Type);
        Assert.Equal(TokenType.COLON, tokens[2].Type);
        Assert.Equal(TokenType.BOOLEAN, tokens[3].Type);
        Assert.Equal("true", tokens[3].Value);
        Assert.Equal(TokenType.COMMA, tokens[4].Type);
        Assert.Equal(TokenType.STRING, tokens[5].Type);
        Assert.Equal(TokenType.COLON, tokens[6].Type);
        Assert.Equal(TokenType.BOOLEAN, tokens[7].Type);
        Assert.Equal("false", tokens[7].Value);
        Assert.Equal(TokenType.COMMA, tokens[8].Type);
        Assert.Equal(TokenType.STRING, tokens[9].Type);
        Assert.Equal(TokenType.COLON, tokens[10].Type);
        Assert.Equal(TokenType.NULL, tokens[11].Type);
        Assert.Equal("null", tokens[11].Value);
        Assert.Equal(TokenType.RIGHT_BRACKET, tokens[12].Type);
    }

    [Fact]
    public void TestInvalidLiteralThrowsException() {
        // Arrange
        var lexer = new Lexer("{\"key\": tru}");
        // Act
        void act() => lexer.Lex();
        // Assert
        Assert.Throws<Exception>(act);
    }
}
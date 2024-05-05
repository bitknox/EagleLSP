namespace LSP.Lexer;
public enum TokenType {
		// Single-character tokens
		LEFT_BRACKET, RIGHT_BRACKET, LEFT_BRACE, RIGHT_BRACE,
		COMMA, COLON,
		// Literals
		NULL, UNDEFINED, STRING, NUMBER, BOOLEAN
}
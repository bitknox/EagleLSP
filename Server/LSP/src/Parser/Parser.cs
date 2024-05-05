using System.Diagnostics;
using LSP.Lexer;

namespace LSP.Parser;

public class Parser(List<Token> tokens) {

	private readonly List<Token> tokens = tokens;

	private readonly List<ParserError> errors = [];

	private int Current {get; set;} = 0;

	private int IndentLevel {get; set;} = 0;

	private Token LastToken => tokens[Current - 1];	

	private static readonly Dictionary<int, List<string>> allowedKeys = new()
	{
		{ 1, ["name", "benchmarks", "colours"] },
		{ 3, ["name", "iterations","group", "command", "environment_options" ] },
	};


	private void ParseUndefined() {
		Advance();
	}
	private void ParseBoolean() {
		Advance();
	}
	private void ParseNumber() {
		Advance();
	}
	private void ParseNull() {
		Advance();
	}

	private void ParseArray() {
		while (Current < tokens.Count) {
			Token token = Advance();
			if (token.Type == TokenType.RIGHT_BRACE) {
				IndentLevel--;
				return;
			}
			ParseToken();
		}
	}

	private void ParseString() {
		Token lToken = LastToken;
		Token token = Advance();
		if(lToken.Type == TokenType.COMMA) {

			bool keyIsValid = ValidateKey(token.Value);

			if (!keyIsValid) {
				errors.Add(new ParserError("Invalid key: " + token.Value ,token.Position, token?.Value?.Length ?? 0));
			}
		}
		// Check if the token is a valid string for the level of indentation
	}

	private void ParseObject() {
		//Parse the object, the first token should be a string
		Advance(); // Skip the left bracket
		// Check for a colon
		var t = Advance();
		
		if (t.Type != TokenType.STRING) {
			throw new Exception("Invalid JSON expected a string as first token");
		}


			bool keyIsValid = ValidateKey(t.Value);

			if (!keyIsValid) {
				errors.Add(new ParserError("Invalid key: " + t.Value , t.Position, t?.Value?.Length ?? 0));
			}
		


		while (Current < tokens.Count) {
			Token token = Advance();
			if (token.Type == TokenType.RIGHT_BRACKET) {
				IndentLevel--;
				return;
			}
			ParseToken();
		}
	}

	public List<ParserError> Parse() {
		//check if the first token is a left bracket
		var token = tokens[Current];
		if (token.Type != TokenType.LEFT_BRACKET) {
			throw new Exception("Invalid JSON expected '{' as first token");
		}


		while (!IsAtEnd()) {
			ParseToken();
		}
		return errors;
	}
	
    private void ParseToken() {
		Token token = tokens[Current];
		switch (token.Type) {
			case TokenType.LEFT_BRACKET:
				IndentLevel++;
				ParseObject();
				break;
			case TokenType.LEFT_BRACE:
				IndentLevel++;
				ParseArray();
				break;
			case TokenType.STRING:
				ParseString();
				break;
			case TokenType.NUMBER:
				ParseNumber();
				break;
			case TokenType.BOOLEAN:
				ParseBoolean();
				break;
			case TokenType.NULL:
				ParseNull();
				break;
			case TokenType.UNDEFINED:
				ParseUndefined();
				break;
			default:
				throw new Exception($"Unexpected token {token}");
		}
	}

	private bool ValidateKey(string? key) {
		if (key == null) {
			return false;
		}
		var allowed = allowedKeys.TryGetValue(IndentLevel, out var a);
		if(!allowed) {
			return true;
		}
		return a!.Contains(key);
	}

	private bool IsAtEnd() {
		return Current >= tokens.Count;
	}

	private Token Advance() {
		Current++;
		return tokens[Current - 1];
	}
}
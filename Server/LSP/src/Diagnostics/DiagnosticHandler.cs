namespace LSP.Diagnostics;
using LSP.Lexer;
using LSP.Parser;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using Position = OmniSharp.Extensions.LanguageServer.Protocol.Models.Position;

public class DiagnosticHandler(ILanguageServer router, BufferManager bufferManager)
{

		private readonly BufferManager _bufferManager = bufferManager;

		private readonly ILanguageServer _router = router;

		public void ParseDocument(Uri documentPath)
		{
			var buffer = _bufferManager.GetBuffer(documentPath.ToString());
			if (buffer == null)
			{
				return;
			}
			List<Diagnostic> _diagnostics = [];

			var source = buffer.ToString();
			var lexer = new Lexer(source);
			var tokens = lexer.Lex();

			var parser = new Parser(tokens);

			var errors = parser.Parse();

			foreach (var error in errors)
			{
				_diagnostics.Add(new Diagnostic()
				{
					Range = new Range()
					{
						Start = new Position()
						{
							Line = error.Position.Line-1,
							Character = error.Position.Column+1
						},
						End = new Position()
						{
							Line = error.Position.Line-1,
							Character = error.Position.Column+error.Length+1
						}
					},
					Severity = DiagnosticSeverity.Error,
					Message = error.Message
				});
			}

		
			_router.SendNotification("textDocument/publishDiagnostics", new PublishDiagnosticsParams()
			{
				Uri = documentPath,
				Diagnostics = new Container<Diagnostic>(_diagnostics)
			});
		}
		
}
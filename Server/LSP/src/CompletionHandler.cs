using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

class CompletionHandler(ILanguageServer router, BufferManager bufferManager) : ICompletionHandler
{

    private readonly ILanguageServer _router = router;
    private readonly BufferManager _bufferManager = bufferManager;

    private readonly DocumentSelector _documentSelector = new(
        new DocumentFilter()
        {
            Pattern = "**/*.eagle"
        }
    );

    public CompletionRegistrationOptions GetRegistrationOptions()
    {
        return new CompletionRegistrationOptions
        {
            DocumentSelector = _documentSelector,
            ResolveProvider = false
        };
    }

    public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        var documentPath = request.TextDocument.Uri.ToString();
        var buffer = _bufferManager.GetBuffer(documentPath);

        //TODO: Implement completion logic

        return Task.FromResult(new CompletionList(isIncomplete: false, items: [new CompletionItem()
				{
						Label = "nuget",
						Kind = CompletionItemKind.Keyword,
						Detail = "nuget",
						Documentation = "NuGet package reference",
						InsertText = "nuget",
						InsertTextFormat = InsertTextFormat.PlainText
				}]));
    }

    public void SetCapability(CompletionCapability capability)
    {
        // No need to do anything here
    }
}
using System.Collections.Concurrent;
using System.Text;

public class BufferManager
{
    private readonly ConcurrentDictionary<string, StringBuilder> _buffers = new();

    public void UpdateBuffer(string documentPath, StringBuilder buffer)
    {
        _buffers.AddOrUpdate(documentPath, buffer, (k, v) => buffer);
    }

    public StringBuilder? GetBuffer(string documentPath)
    {
        return _buffers.TryGetValue(documentPath, out var buffer) ? buffer : null;
    }
}
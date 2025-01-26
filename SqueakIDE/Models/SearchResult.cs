namespace SqueakIDE.Models;

public class SearchResult
{
    public int StartOffset { get; }
    public int Length { get; }

    public SearchResult(int startOffset, int length)
    {
        StartOffset = startOffset;
        Length = length;
    }
} 
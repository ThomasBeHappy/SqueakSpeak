using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using SqueakIDE.Models;
using SqueakIDE.Services;

public class SearchService
{
    private List<SearchResult> _currentResults = new();
    private Dictionary<string, List<SearchResult>> _searchResults = new();
    private int _currentIndex = -1;
    private SearchOptions _options = new();
    private readonly TextMarkerService _markerService;

    public SearchService(TextMarkerService markerService)
    {
        _markerService = markerService;
    }

    public void PerformSearch(string searchText, string currentFile, string content)
    {
        if (string.IsNullOrEmpty(searchText)) return;

        InitializeSearch();
        var results = FindMatches(content, searchText);
        if (results.Any())
        {
            _searchResults[currentFile] = results;
            _currentResults = results;
        }
    }

    private List<SearchResult> FindMatches(string content, string searchText)
    {
        var results = new List<SearchResult>();
        
        if (_options.UseRegex)
        {
            try
            {
                var regex = new Regex(searchText, 
                    _options.MatchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
                var matches = regex.Matches(content);
                foreach (Match match in matches)
                {
                    results.Add(new SearchResult(match.Index, match.Length));
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid regular expression pattern");
            }
        }
        else
        {
            var comparison = _options.MatchCase ? 
                StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            
            int index = 0;
            while ((index = content.IndexOf(searchText, index, comparison)) != -1)
            {
                if (_options.WholeWord)
                {
                    if (IsWholeWord(content, index, searchText.Length))
                    {
                        results.Add(new SearchResult(index, searchText.Length));
                    }
                }
                else
                {
                    results.Add(new SearchResult(index, searchText.Length));
                }
                index += searchText.Length;
            }
        }

        return results;
    }

    public void NavigateNext()
    {
        if (_currentResults?.Any() != true) return;
        _currentIndex = (_currentIndex + 1) % _currentResults.Count;
        HighlightCurrentResult();
    }

    public void NavigatePrevious()
    {
        if (_currentResults?.Any() != true) return;
        _currentIndex = (_currentIndex - 1 + _currentResults.Count) % _currentResults.Count;
        HighlightCurrentResult();
    }

    private void HighlightCurrentResult()
    {
        _markerService?.Clear();
        
        foreach (var result in _currentResults)
        {
            var marker = _markerService.Create(result.StartOffset, result.Length);
            marker.MarkerType = TextMarkerType.SquigglyUnderline;
            marker.MarkerColor = Colors.Yellow;
        }
    }

    private void InitializeSearch()
    {
        _currentResults.Clear();
        _currentIndex = -1;
        _markerService?.Clear();
    }

    public void UpdateSearchOptions(SearchOptions options)
    {
        _options = options;
    }

    private bool IsWholeWord(string content, int index, int length)
    {
        bool validStart = index == 0 || !char.IsLetterOrDigit(content[index - 1]);
        bool validEnd = index + length >= content.Length || 
                       !char.IsLetterOrDigit(content[index + length]);
        return validStart && validEnd;
    }

    public void Clear()
    {
        InitializeSearch();
    }
} 
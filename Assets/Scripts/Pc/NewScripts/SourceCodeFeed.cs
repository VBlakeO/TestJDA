using System.Text;

public class SourceCodeFeed
{
    private const char LineBreak = '\n';

    private readonly string _sample;
    private readonly int _charsPerStep;
    private readonly int _maxLines;
    private readonly int _maxCharacters;
    private readonly StringBuilder _visible = new();

    private int _cursor = 0;
    private int _lineCount = 0;

    public string VisibleText => _visible.ToString();

    public SourceCodeFeed(string sample, int charsPerStep, int maxLines, int maxCharacters)
    {
        _sample = sample ?? string.Empty;
        _charsPerStep = charsPerStep;
        _maxLines = maxLines;
        _maxCharacters = maxCharacters;
    }

    // Wraps around the sample so long projects never read past its end
    public void Advance()
    {
        if (_sample.Length == 0)
            return;

        for (int i = 0; i < _charsPerStep; i++)
        {
            char _next = _sample[_cursor];
            _visible.Append(_next);

            if (_next == LineBreak)
                _lineCount++;

            _cursor = (_cursor + 1) % _sample.Length;
        }

        TrimToLimits();
    }

    public void Reset()
    {
        _visible.Clear();
        _cursor = 0;
        _lineCount = 0;
    }

    // Only the tail fits on screen, so older text is dropped to keep the TextMeshPro rebuild cost constant
    private void TrimToLimits()
    {
        while (_lineCount > _maxLines)
            RemoveFirstLine();

        if (_visible.Length > _maxCharacters)
            RemoveLeadingCharacters(_visible.Length - _maxCharacters);
    }

    private void RemoveFirstLine()
    {
        for (int i = 0; i < _visible.Length; i++)
        {
            if (_visible[i] != LineBreak)
                continue;

            _visible.Remove(0, i + 1);
            _lineCount--;
            return;
        }

        _lineCount = 0;
    }

    private void RemoveLeadingCharacters(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_visible[i] == LineBreak)
                _lineCount--;
        }

        _visible.Remove(0, amount);
    }
}
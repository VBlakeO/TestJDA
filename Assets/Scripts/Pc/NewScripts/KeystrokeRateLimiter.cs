using System.Collections.Generic;

public class KeystrokeRateLimiter
{
    private const float WindowSeconds = 1f;

    private readonly int _maxPerWindow;
    private readonly Queue<float> _acceptedTimes = new();

    public KeystrokeRateLimiter(int maxPerWindow)
    {
        _maxPerWindow = maxPerWindow;
    }

    // Sliding window, so bursts from macros or several keys at once are capped without penalizing a steady rhythm
    public bool TryRegister(float time)
    {
        while (_acceptedTimes.Count > 0 && time - _acceptedTimes.Peek() >= WindowSeconds)
            _acceptedTimes.Dequeue();

        if (_acceptedTimes.Count >= _maxPerWindow)
            return false;

        _acceptedTimes.Enqueue(time);
        return true;
    }

    public void Reset()
    {
        _acceptedTimes.Clear();
    }
}
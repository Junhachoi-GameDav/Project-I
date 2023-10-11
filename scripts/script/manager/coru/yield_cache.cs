using System.Collections.Generic;
using UnityEngine;

class FloatComparer : IEqualityComparer<float>
{
    bool IEqualityComparer<float>.Equals(float x, float y)
    {
        return x == y;
    }
    int IEqualityComparer<float>.GetHashCode(float obj)
    {
        return obj.GetHashCode();
    }
}
public static class _yield_cache
{
    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_timeInterval.TryGetValue(seconds, out wfs))
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}

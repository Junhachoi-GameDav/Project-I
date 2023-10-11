using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

static class yield_cache
{
    // 박싱이 발생하지 않게 해주며 의도치 않게 가비지가 생성되는 것을 방지
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
    // wait 문들 캐싱
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> _time_interval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    private static readonly Dictionary<float, WaitForSecondsRealtime> _time_interval_real = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());


    // WaitForSeconds 를 가져와서 TryGetValue으로 넣어준다.
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_time_interval.TryGetValue(seconds, out wfs)) //없으면 넣어준다.
        {
            _time_interval.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        WaitForSecondsRealtime wfs_real;
        if (!_time_interval_real.TryGetValue(seconds, out wfs_real)) //없으면 넣어준다.
        {
            _time_interval_real.Add(seconds, wfs_real = new WaitForSecondsRealtime(seconds));
        }
        return wfs_real;
    }
}
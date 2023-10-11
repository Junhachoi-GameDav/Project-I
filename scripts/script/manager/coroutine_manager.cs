using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

static class yield_cache
{
    // �ڽ��� �߻����� �ʰ� ���ָ� �ǵ�ġ �ʰ� �������� �����Ǵ� ���� ����
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
    // wait ���� ĳ��
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> _time_interval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    private static readonly Dictionary<float, WaitForSecondsRealtime> _time_interval_real = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());


    // WaitForSeconds �� �����ͼ� TryGetValue���� �־��ش�.
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_time_interval.TryGetValue(seconds, out wfs)) //������ �־��ش�.
        {
            _time_interval.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        WaitForSecondsRealtime wfs_real;
        if (!_time_interval_real.TryGetValue(seconds, out wfs_real)) //������ �־��ش�.
        {
            _time_interval_real.Add(seconds, wfs_real = new WaitForSecondsRealtime(seconds));
        }
        return wfs_real;
    }
}
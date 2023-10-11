using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fade_manager : MonoBehaviour
{
    public GameObject fade_obj;
    public Image fade_image;

    float time = 0;
    float f_time = 1;

    public void Fade()
    {
        StartCoroutine(Fade_co_ro());
    }
    //public void Fade_in()
    //{
    //    StartCoroutine(Fade_in_co_ro());
    //}
    IEnumerator Fade_co_ro()
    {
        fade_obj.SetActive(true);
        time = 0;
        Color alpha = fade_image.color;
        while (alpha.a < 1f)
        {
            time += Time.unscaledDeltaTime / f_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            fade_image.color = alpha;
            yield return null;
        }
        time = 0;
        yield return yield_cache.WaitForSecondsRealtime(1.5f);
        
        while (alpha.a > 0f)
        {
            time += Time.unscaledDeltaTime / f_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            fade_image.color = alpha;
            yield return null;
        }
        fade_obj.SetActive(false);
        yield return null;
    }
    //}
    //IEnumerator Fade_in_co_ro()
    //{
    //    fade_obj.SetActive(true);
    //    Color alpha = fade_image.color;
    //    while (alpha.a > 0f)
    //    {
    //        time += Time.deltaTime / f_time;
    //        alpha.a = Mathf.Lerp(1, 0, time);
    //        fade_image.color = alpha;
    //        yield return null;
    //    }
    //    fade_obj.SetActive(false);
    //    time = 0;
    //    yield return null;
    //}
}

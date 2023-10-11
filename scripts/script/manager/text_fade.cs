using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class text_fade : MonoBehaviour
{
    public GameObject[] objs;
    public Image[] objs_image;
    public Text txt;

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
        for (int i = 0; i < objs.Length; i++) { objs[i].gameObject.SetActive(true); }
        
        time = 0;
        Color alpha1 = objs_image[0].color;
        Color alpha2 = objs_image[1].color;
        Color alpha3 = txt.color;
        while (alpha1.a < 1f)
        {
            time += Time.unscaledDeltaTime / f_time;
            alpha1.a = Mathf.Lerp(0, 1, time);
            alpha2.a = Mathf.Lerp(0, 1, time);
            alpha3.a = Mathf.Lerp(0, 1, time);
            objs_image[0].color = alpha1;
            objs_image[1].color = alpha2;
            txt.color = alpha3;
            yield return null;
        }
        time = 0;
        yield return yield_cache.WaitForSecondsRealtime(1.5f);

        while (alpha1.a > 0f)
        {
            time += Time.unscaledDeltaTime / f_time;
            alpha1.a = Mathf.Lerp(1, 0, time);
            alpha2.a = Mathf.Lerp(1, 0, time);
            alpha3.a = Mathf.Lerp(1, 0, time);
            objs_image[0].color = alpha1;
            objs_image[1].color = alpha2;
            txt.color = alpha3;
            yield return null;
        }
        for (int i = 0; i < objs.Length; i++) { objs[i].gameObject.SetActive(false); }
        yield return null;
    }
}

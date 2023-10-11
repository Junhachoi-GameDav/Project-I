using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class saving_icon : MonoBehaviour
{
    public Button save_button;

    public void save_icon()
    {
        StartCoroutine(co_save());
    }

    IEnumerator co_save()
    {
        yield return yield_cache.WaitForSecondsRealtime(1f);
        this.gameObject.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ending_credit : MonoBehaviour
{
    public void go_back_to_scene()
    {
        game_manager.Instance.fade_mng.Fade();
        StartCoroutine(co_load_scene(0));
    }
    IEnumerator co_load_scene(int num)
    {
        yield return yield_cache.WaitForSeconds(1f);
        SceneManager.LoadScene(num);
    }
}

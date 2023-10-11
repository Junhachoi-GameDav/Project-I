using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loading : MonoBehaviour
{
    public GameObject[] icon;
    public Slider progress_bar;
    bool is_key_press;
    

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += on_scene_loaded;
    }

    void on_scene_loaded(Scene scene, LoadSceneMode mode)
    {
        icon[0].SetActive(true); icon[1].SetActive(true); icon[2].SetActive(false);
        StartCoroutine(load_scene());
    }

    IEnumerator load_scene()
    {
        
        yield return yield_cache.WaitForSeconds(1.2f);
        AsyncOperation operation = SceneManager.LoadSceneAsync("GAME");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if(progress_bar.value < 0.9f)
            {
                progress_bar.value = Mathf.MoveTowards(operation.progress, 0.9f, Time.deltaTime);
            }
            else if(operation.progress >= 0.9f)
            {
                progress_bar.value = Mathf.MoveTowards(progress_bar.value, 1f, Time.deltaTime);
            }

            if(progress_bar.value >= 1)
            {
                icon[0].SetActive(false); icon[1].SetActive(false); icon[2].SetActive(true);
            }

            if (is_key_press && progress_bar.value >= 1 && operation.progress >=0.9f)
            {
                game_manager.Instance.fade_mng.Fade();
                game_manager.Instance.sound_mng.sm_normal_ef_sound_play("go_game_impact");
                yield return yield_cache.WaitForSeconds(1);
                operation.allowSceneActivation = true;
                is_key_press = false;
            }
        }
    }

    private void OnGUI()
    {
        Event _event = Event.current;
        if ((_event.isKey || _event.isMouse) && progress_bar.value >= 1)
        {
            is_key_press = true;
        }
    }

    //체인 풀기 게임이 종료되면 호출됨
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= on_scene_loaded;
    }
}

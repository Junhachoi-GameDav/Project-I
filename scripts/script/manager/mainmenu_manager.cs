using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class mainmenu_manager : MonoBehaviour
{
    //setting_manager setting_mng;
    //save_manager save_mng;
    public Button option_btn;
    public GameObject apply_window;

    public void connect_new_game_bool(bool value)
    {
        game_manager.Instance.save_mng.enable_new_game = value;
    }
    public void connect_load_game_bool(bool value)
    {
        game_manager.Instance.save_mng.enable_load_game = value;
    }
    public void connect_option()
    {
        game_manager.Instance.setting_mng.setting_obj.SetActive(true);
    }
    public void connect_new_game(int num)
    {
        
        game_manager.Instance.save_mng.delete_data(num);
        game_manager.Instance.save_mng.slot(num);
        game_manager.Instance.fade_mng.Fade();
    }

    public void connect_load_window()
    {
        game_manager.Instance.save_mng.load_obj.SetActive(true);
        Time.timeScale = 1f;
    }

    public void connect_game_start_btn()
    {
        game_manager.Instance.save_mng.game_start_btn();
    }

    public void connect_credit_btn()
    {
        game_manager.Instance.fade_mng.Fade();
        StartCoroutine(co_load_scene(3));
    }
    IEnumerator co_load_scene(int num)
    {
        yield return yield_cache.WaitForSeconds(1f);
        SceneManager.LoadScene(num);
    }

    private void Start()
    {
        if(!game_manager.Instance.is_apply_window_open)
        {
            Invoke("deley", 1f);
        }
    }
    void deley()
    {
        apply_window.SetActive(true);
        game_manager.Instance.is_apply_window_open = true;
    }

    public void exit_game()
    {
        Application.Quit();
    }


}

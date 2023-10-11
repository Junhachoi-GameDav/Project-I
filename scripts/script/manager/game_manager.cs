
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static game_manager instance = null;

    public sound_manager sound_mng;
    public save_manager save_mng;
    public setting_manager setting_mng;
    public fade_manager fade_mng;

    public bool is_apply_window_open;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        sound_mng = GetComponentInChildren<sound_manager>();
        save_mng = GetComponentInChildren<save_manager>();
        setting_mng = GetComponentInChildren<setting_manager>();
        fade_mng = GetComponentInChildren<fade_manager>();
    }
    public static game_manager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    //public sound_manager sound_mng;
    //public save_manager save_mng;
    //public setting_manager setting_mng;
    //public fade_manager fade_mng;

    //private void Start()
    //{
    //    sound_mng =GetComponentInChildren<sound_manager>();
    //    save_mng = GetComponentInChildren<save_manager>();
    //    setting_mng = GetComponentInChildren<setting_manager>();
    //    fade_mng = GetComponentInChildren<fade_manager>();
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            save_mng.load_obj.SetActive(false);
            setting_mng.setting_obj.SetActive(false);
        }
    }

    public void scene_load(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    public void exit_game()
    {
        Application.Quit();
    }
}

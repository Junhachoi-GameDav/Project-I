using UnityEngine;

public class tutorial_manager : MonoBehaviour
{
    public bool is_tutorial;

    ingame_manager ingame_mng;

    public int trigger_num;
    public GameObject[] tutorial_colliders;

    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }

    public void count_trigger_num()
    {
        trigger_num++;
        game_manager.Instance.save_mng.now_player.tutorial_count_num = trigger_num;
    }

    public void isnt_tutorial()
    {
        is_tutorial = false;
    }
    public void time_controlling()
    {
        if (is_tutorial)
        {
            ingame_mng.cams_mng.cams_lens_ef(1);
            Time.timeScale = 0;
        }
        else
        {
            ingame_mng.cams_mng.cams_lens_ef(0);
            Time.timeScale = 1;
        }
    }

}

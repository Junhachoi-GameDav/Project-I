using System.Collections;
using UnityEngine;
using System.IO;  //데이터를 넣거나 꺼낼때 쓴다.
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class playerData
{
    public float player_max_hp =120;
    public float player_cur_hp =120;
    public float player_max_stemina =130;
    public float player_attck_power =2.2f;
    public int player_data_chip =500;
    public int player_heal_pack_max_num =3;
    public int player_heal_power =40;
    public int player_pill_part_num =0;
    public int player_needed_datachip =0;

    public float player_pos_x = 0;
    public float player_pos_y = 0;
    public float player_pos_z = 0;

    public int capsule_index = 0;
    public int capsule_tank_fade_count = 0;
    public bool is_seconds_capsule;

    public int tutorial_count_num = 0;

    public bool area1_pill_get;
    public bool area2_pill_get;
    public bool area3_pill_get;
    public bool area4_pill_get;
    public bool area5_pill_get;
    public bool area6_pill_get;

    //public 


    public bool is_opened;
    public bool is_ending;

    public string date_time;

}

public class save_manager : MonoBehaviour
{
    
    public playerData now_player = new playerData();
    public Text[] slot_text;
    public GameObject[] new_game_windows;
    public GameObject load_obj;

    string path;
    int cur_slot;

    bool[] savefile = new bool[3];
    public bool enable_new_game;
    public bool enable_load_game;

    mainmenu_manager _mainmenu_mng;
    private void Awake()
    {
        _mainmenu_mng =FindObjectOfType<mainmenu_manager>();
        //C:/Users/user_name/AppData/LocalLow/DefaultCompany/project_name
        path = Application.persistentDataPath + "/save";
    }

    public void game_start_btn()
    {
        //파일 존재 확인
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(path + $"{i}"))
            {
                savefile[i] = true;
                cur_slot = i;
                load_data();
                slot_text[i].text = now_player.date_time;
            }
            else
            {
                slot_text[i].text = "NEW DATA";
                //slot_text[i].text = DateTime.Now.ToString();
            }
        }
        data_clear();
    }
    
    public void save_data()
    {
        now_player.date_time = DateTime.Now.ToString();

        string data = JsonUtility.ToJson(now_player);
        
        
        File.WriteAllText(path + cur_slot.ToString(), data);
    }
    public void load_data()
    {
        string data = File.ReadAllText(path + cur_slot.ToString());
        now_player = JsonUtility.FromJson<playerData>(data);
    }

    public void data_clear()
    {
        cur_slot = -1;
        now_player = new playerData();
    }

    public void slot(int num)
    {
        cur_slot = num;
        if (enable_new_game)
        {
            if (savefile[num])
            {
                new_game_windows[num].SetActive(true);
                game_manager.Instance.sound_mng.sm_ui_sound_play("button2_click");
            }
            else
            {
                save_data();
                //go_game();
                game_manager.Instance.fade_mng.Fade();
                game_manager.Instance.sound_mng.sm_ui_sound_play("button3_click");
                load_obj.SetActive(false);
                StartCoroutine(co_go_game());
            }
        }
        else if (enable_load_game)
        {
            if (savefile[num])
            {
                load_data();
                game_manager.Instance.fade_mng.Fade();
                game_manager.Instance.sound_mng.sm_ui_sound_play("button3_click");
                load_obj.SetActive(false);
                StartCoroutine(co_go_game());
            }
            else
            {
                save_data();
                //go_game();
                game_manager.Instance.fade_mng.Fade();
                game_manager.Instance.sound_mng.sm_ui_sound_play("button3_click");
                load_obj.SetActive(false);
                StartCoroutine(co_go_game());
            }
        }
    }
    public void mainmenu_start_all_false()
    {
        enable_new_game = false;
        enable_load_game = false;
    }

    public void go_game()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator co_go_game()
    {
        yield return yield_cache.WaitForSeconds(1.8f);
        go_game();
        mainmenu_start_all_false();
    }

    public void delete_data(int num)
    {
        File.Delete(path + num.ToString());
        if (!File.Exists(path + $"{num}"))
        {
            savefile[num] = false;
            slot_text[num].text = "NEW DATA";
        }
    }

}

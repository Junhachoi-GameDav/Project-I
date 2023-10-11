using System.Collections;
using UnityEngine;
using System.IO;  //데이터를 넣거나 꺼낼때 쓴다.
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class playerData
{
    public float player_max_hp =100;
    public float player_cur_hp =100;
    public float player_max_stemina =100;
    public float player_attck_power =2;
    public int player_data_chip =1000;
    public int player_heal_pack_max_num =5;
    public int player_heal_power =30;
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

    public bool is_ending;

    public string date_time;

}

public class save_manager : MonoBehaviour
{
    public GameObject load_obj;
    public playerData now_player = new playerData();

    string path;
    int cur_slot;

    public Text[] slot_text;

    bool[] savefile = new bool[3];

    private void Awake()
    {
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

        if (savefile[num])
        {
            load_data();
            StartCoroutine(co_go_game());
        }
        else
        {
            save_data();
            //go_game();
            StartCoroutine(co_go_game());
        }

    }

    public void go_game()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator co_go_game()
    {
        yield return yield_cache.WaitForSeconds(1.8f);
        go_game();
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

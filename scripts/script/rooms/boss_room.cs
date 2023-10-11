using System.Collections;
using UnityEngine;

public class boss_room : MonoBehaviour
{
    public GameObject pill_part;
    public GameObject[] door_collider;
    public BoxCollider this_box_collider;
    public doors _doors;
    public door_in_checker _door_in_checker;
    public GameObject[] en_kyle_objs;

    bool stop_while;

    ingame_manager ingame_mng;
    //public bg_sound _bg_sound;

    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            ingame_mng._enemy.battle_start = true;
            ingame_mng._bg_sound.play_sounds("boss_in_battle");
            _door_in_checker.is_player_in = false;
            _doors.close_doors();
            this_box_collider.enabled = false;
            for (int i = 0; i < door_collider.Length; i++)
            {
                door_collider[i].SetActive(false);
            }
            for (int i = 0;i < en_kyle_objs.Length; i++)
            {
                en_kyle_objs[i].SetActive(false);
            }
            StartCoroutine(co__end_fight());
        }
    }
    IEnumerator co__end_fight()
    {
        while (!stop_while)
        {
            yield return null;
            if (ingame_mng._enemy.is_dead == true)
            {
                for (int j = 0; j < door_collider.Length; j++)
                {
                    door_collider[j].SetActive(true);
                }
                pill_part.SetActive(true);
                stop_while = true;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class lock_doors : MonoBehaviour
{
    public GameObject pill_part;
    public GameObject[] door_collider;
    public Material[] door_materials; // 0 = white , 1 = black
    public BoxCollider this_box_collider;
    public doors[] _doors;
    public door_in_checker _door_in_checker;

    public GameObject[] en_Kyles_objs;

    bool stop_while;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _door_in_checker.is_player_in = false;

            for (int i = 0; i < _doors.Length; i++)
            {
                _doors[i].close_doors();
                _doors[i].door_left.GetComponent<MeshRenderer>().material = door_materials[1];
                _doors[i].door_right.GetComponent<MeshRenderer>().material = door_materials[1];
            }

            this_box_collider.enabled = false;

            for (int i = 0; i < door_collider.Length; i++)
            {
                door_collider[i].SetActive(false);
            }
            StartCoroutine(co_fight_last());
        }
    }

    IEnumerator co_fight_last()
    {
        while (!stop_while)
        {
            yield return null;
            for (int i = 0; i < en_Kyles_objs.Length; i++)
            {
                if (en_Kyles_objs[i].activeSelf == true)
                {
                    break;
                }
                else if( i >= en_Kyles_objs.Length-1 && en_Kyles_objs[i].activeSelf == false)
                {
                    for (int j = 0; j < door_collider.Length; j++)
                    {
                        door_collider[j].SetActive(true);
                    }

                    pill_part.SetActive(true);
                    for (int x = 0; x < _doors.Length; x++)
                    {
                        _doors[x].door_left.GetComponent<MeshRenderer>().material = door_materials[0];
                        _doors[x].door_right.GetComponent<MeshRenderer>().material = door_materials[0];
                    }
                    stop_while = true;
                }
            }
        }
    } 
}

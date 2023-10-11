using System.Collections;
using UnityEngine;

public class doors : MonoBehaviour
{
    door_in_checker _door_In_checker;

    public GameObject door_left;
    public GameObject door_right;
    public float speed;
    public float limit_door_open;
    float timer = 0;
    public float cool_time;
    public bool is_opend;

    private void Awake()
    {
        _door_In_checker = GetComponentInChildren<door_in_checker>();
    }
    private void Update()
    {
        if (is_opend && cool_time <=2f)
        {
            cool_time += Time.deltaTime;
        }
        else if(!_door_In_checker.is_player_in)
        {
            close_doors();
            cool_time = 0;
        }
    }

    public void open_doors()
    {
        if (!is_opend)
        {
            StartCoroutine(co_open_doors());
        }
    }
    public void close_doors()
    {
        if(is_opend)
        {
            StartCoroutine(co_close_doors());
        }
    }
    IEnumerator co_open_doors()
    {
        timer = 0;
        game_manager.Instance.sound_mng.sm_enviroment_sound_play("open_door");
        while(door_left.transform.localPosition.x > -(limit_door_open))
        {
            timer += Time.deltaTime * speed;
            door_left.transform.localPosition = new Vector3(Mathf.Lerp(0, -(limit_door_open), timer), 0, 0);
            door_right.transform.localPosition = new Vector3(Mathf.Lerp(0, limit_door_open, timer), 0, 0);
            yield return null;
        }
        timer = 0;
        is_opend = true;
    }
    IEnumerator co_close_doors()
    {
        timer = 0;
        game_manager.Instance.sound_mng.sm_enviroment_sound_play("open_door");
        while (door_left.transform.localPosition.x < 0)
        {
            timer += Time.deltaTime * speed;
            door_left.transform.localPosition = new Vector3(Mathf.Lerp(-(limit_door_open), 0, timer), 0, 0);
            door_right.transform.localPosition = new Vector3(Mathf.Lerp(limit_door_open, 0, timer), 0, 0);
            yield return null;
        }
        is_opend = false;
        timer = 0;
    }


}

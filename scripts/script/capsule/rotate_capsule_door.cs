using System.Collections;
using UnityEngine;

public class rotate_capsule_door : MonoBehaviour
{
    public GameObject door_obj;
    public float limit;
    public float speed;
    public bool is_opend;
    public bool is_player_in;
    float time;
    public float cool_time;

    private void Update()
    {
        if (is_opend && cool_time <= 2f)
        {
            cool_time += Time.deltaTime;
        }
        else if (!is_player_in)
        {
            close_doors();
            cool_time = 0;
        }
    }

    void open_door()
    {
        if (!is_opend)
        {
            StartCoroutine(co_open_doors());
        }
    }
    void close_doors()
    {
        if(is_opend)
        {
            StartCoroutine(co_close_doors());
        }
    }

    IEnumerator co_open_doors()
    {
        time = 0;
        while (door_obj.transform.localRotation.y > 0)
        {
            time += Time.deltaTime * speed;
            door_obj.transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(limit, 0, time), 0);
            yield return null;
        }
        time = 0;
        is_opend = true;
    }
    IEnumerator co_close_doors()
    {
        time = 0;
        while (door_obj.transform.localRotation.y < 0.8f)
        {
            time += Time.deltaTime * speed;
            door_obj.transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(0, limit, time), 0);
            yield return null;
        }
        is_opend = false;
        time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            open_door();
            is_player_in = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            is_player_in = false;
        }
    }

}

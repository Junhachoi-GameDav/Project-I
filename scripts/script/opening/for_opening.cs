using System.Collections;
using UnityEngine;

public class for_opening : MonoBehaviour
{
    public GameObject[] objs;
    public Transform cap_pos;
    public Transform cap_pos_org;
    public GameObject door_left;
    public GameObject door_right;
    public AudioSource bgm;
    public float speed;
    public float limit_door_open;
    public bool is_opened;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(game_manager.Instance.save_mng.now_player.is_opened) { is_opened = true; bgm.Play(); return; }
        StartCoroutine(co_open_doors());
    }
    
    IEnumerator co_open_doors()
    {
        yield return null;
        bgm.Stop();
        objs[0].transform.position = cap_pos.position;
        objs[0].transform.rotation = cap_pos.rotation;
        objs[1].SetActive(true);
        objs[3].SetActive(false); //ui

        yield return yield_cache.WaitForSeconds(2f);
        timer = 0;
        game_manager.Instance.sound_mng.sm_enviroment_sound_play("open_door");
        while (door_left.transform.localPosition.x > -(limit_door_open))
        {
            timer += Time.deltaTime * speed;
            door_left.transform.localPosition = new Vector3(Mathf.Lerp(0, -(limit_door_open), timer), 0, 0);
            door_right.transform.localPosition = new Vector3(Mathf.Lerp(0, limit_door_open, timer), 0, 0);
            yield return null;
        }
        timer = 0;
        game_manager.Instance.sound_mng.sm_enviroment_sound_play("down_capslue");
        while (objs[0].transform.localPosition.y > 0.01f)
        {
            timer += Time.deltaTime * 0.02f;
            objs[0].transform.position = new Vector3(-0.07f, Mathf.Lerp(objs[0].transform.localPosition.y, 0, timer), -3.74f);
            if(objs[0].transform.localPosition.y < 3.5f) { objs[2].SetActive(true); objs[1].SetActive(false); }
            yield return null;
        }
        timer = 0;
        yield return yield_cache.WaitForSeconds(1.5f);
        game_manager.Instance.fade_mng.Fade();
        yield return yield_cache.WaitForSeconds(1.5f);
        
        bgm.Play();
        objs[3].SetActive(true); //ui
        objs[2].SetActive(false);
        is_opened = true;
        yield return yield_cache.WaitForSeconds(5f);
        while (door_left.transform.localPosition.x < 0)
        {
            timer += Time.deltaTime * speed;
            door_left.transform.localPosition = new Vector3(Mathf.Lerp(-(limit_door_open), 0, timer), 0, 0);
            door_right.transform.localPosition = new Vector3(Mathf.Lerp(limit_door_open, 0, timer), 0, 0);
            yield return null;
        }
        timer = 0;
    }
    
}

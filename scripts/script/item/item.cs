
using UnityEngine;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    public GameObject parent_obj;
    public GameObject inter_icon_obj;
    public GameObject get_icon_obj;
    public Image interection_icon_img;
    public Transform interection_icon_pos;
    public ParticleSystem pill_get_ef;

    public float rot_spped;

    public bool is_player_in;
    ingame_manager ingame_mng;
    
    [Header("info")]
    public int pill_num;
    public bool got_the_pill;

    float timer;
    
    void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    private void Update()
    {
        check();
        when_pick_up();
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        this.transform.rotation = Quaternion.Euler(-33f, timer * rot_spped, 0f);

    }

    void check()
    {
        if(is_player_in)
        {
            inter_icon_obj.SetActive(true);
            interection_icon_img.rectTransform.position = Camera.main.WorldToScreenPoint(interection_icon_pos.position);
        }
    }

    void when_pick_up()
    {
        if(is_player_in && Input.GetKeyDown(key_setting.keys[key_action.INTERECTION]))
        {
            ingame_mng.player_mng.pill_part_num += 1;
            is_player_in = false;
            pill_get_ef.gameObject.transform.position = transform.position;
            pill_get_ef.Play();
            get_icon_obj.SetActive(true);
            inter_icon_obj.SetActive(false);
            game_manager.Instance.sound_mng.sm_ui_sound_play("get_pill");
            Invoke("set_false", 2.3f);
            got_the_pill = true;
            parent_obj.SetActive(false);
        }
    }
    
    void set_false()
    {
        get_icon_obj.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            is_player_in = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            is_player_in = false;
            inter_icon_obj.SetActive(false);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ingame_manager : MonoBehaviour
{
    public cams_manager cams_mng;
    public ui_manager ui_mng;
    public player_movement p_movement;
    public player_manager player_mng;
    weapon _weapon;

    mouse_cursor_manager mouse_cursor_mng;
    public talk_manager talk_mng;
    public upgrade_manager upgrade_mng;
    public text_fade _text_fade;
    public capsule _capsule;
    public tutorial_manager tutorial_mng;
    public enemy _enemy;
    lock_n_pill_manager lock_n_pill_mng;
    public anime_manager anime_mng;
    public bg_sound _bg_sound;

    [Header("ui")]
    public GameObject obj_ui;
    public GameObject obj_ui_mng;
    public GameObject obj_talk_ui;
    public GameObject obj_upgrade_ui;
    public Image pause_img;
    public Button option_btn;
    public Transform[] player_respawn_pos;

    public en_kyle[] _en_kyles; 

    [Header("bool")]
    public bool is_pause;
    public bool is_game_starting;

    int count;

    private void Awake()
    {
        option_btn = GetComponent<Button>();
        cams_mng = FindObjectOfType<cams_manager>();
        ui_mng = FindObjectOfType<ui_manager>();
        p_movement = FindObjectOfType<player_movement>();
        player_mng = FindObjectOfType<player_manager>();
        _weapon = player_mng.GetComponentInChildren<weapon>();
        mouse_cursor_mng = FindObjectOfType<mouse_cursor_manager>();
        talk_mng = GetComponentInChildren<talk_manager>();
        upgrade_mng = GetComponentInChildren<upgrade_manager>();
        _capsule = FindObjectOfType<capsule>();
        tutorial_mng = FindObjectOfType<tutorial_manager>();
        _enemy = FindObjectOfType<enemy>();
        lock_n_pill_mng = FindObjectOfType<lock_n_pill_manager>();
        anime_mng = GetComponentInChildren<anime_manager>();
        _bg_sound = GetComponentInChildren<bg_sound>();
    }
    
    void Start()
    {
      
        count = 0;

        load_data();
        pause_off();

        ui_mng.heal_pack_ui_count(player_mng.heal_pack_max_num.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        mouse_cursor();
        pause_cams_ef();
        pause_sys();
        game_over();
    }
    void mouse_cursor()
    {
        if (talk_mng.is_talk || is_pause || upgrade_mng.is_upgrading || _capsule.is_player_in || tutorial_mng.is_tutorial)
        {
            mouse_cursor_mng.mouse_cursor_on_off(true);
            cams_mng.mouse_sensitivity = game_manager.Instance.setting_mng.mouse_sensitivity_slider.value * 10f;
        }
        else
        {
            mouse_cursor_mng.mouse_cursor_on_off(false);
        }
    }
    public void pause_sys()
    {
        if (game_manager.Instance.setting_mng.is_setting || upgrade_mng.is_upgrading || p_movement._anime.GetBool("dead") 
            ||_capsule.is_player_in || tutorial_mng.is_tutorial) { return; }

        if (Input.GetKeyDown(KeyCode.Escape) && !is_pause)
        {
            Time.timeScale = 0f;
            is_pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && is_pause)
        {
            Time.timeScale = 1f;
            is_pause = false;

        }

        if (is_pause && pause_img.rectTransform.position.x < 0)
        {
            pause_img.rectTransform.position = new Vector3(Mathf.Lerp(pause_img.rectTransform.position.x, 0, Time.unscaledDeltaTime * 10f), 0, 0);
            //if(pause_img.rectTransform.position.x <= 0) { open = false; }
        }
        else if(!is_pause && pause_img.rectTransform.position.x > -1400)
        {
            pause_img.rectTransform.position = new Vector3(Mathf.Lerp(pause_img.rectTransform.position.x, -1400, Time.unscaledDeltaTime * 10f), 0, 0);
            //if (pause_img.rectTransform.position.x >= -1400) { open = true; }
        }
    }
    public void timezero_on()
    {
        //Time.timeScale = 0f;
        upgrade_mng.is_upgrading = true;
    }
    public void pause_off()
    {
        Time.timeScale = 1f;
        upgrade_mng.is_upgrading = false;
        cams_mng.cams_lens_ef(0);
        is_pause = false;
        
    }

    void pause_cams_ef()
    {
        if (is_pause || upgrade_mng.is_upgrading)
        {
            obj_ui_mng.SetActive(false);
            obj_ui.SetActive(false);
            obj_talk_ui.SetActive(false);
            
            cams_mng.cams_lens_ef(1);
        }
        else
        {
            obj_ui_mng.SetActive(true);
            obj_ui.SetActive(true);
            //cams_mng.cams_lens_ef(0);
        }
    }

    public void connect_option()
    {
        game_manager.Instance.setting_mng.setting_obj.SetActive(true);
    }
    
    public void load_scene(int num)
    {
        game_manager.Instance.fade_mng.Fade();
        StartCoroutine(co_load_scene(num));
    }
    IEnumerator co_load_scene(int num)
    {
        pause_off();
        yield return yield_cache.WaitForSeconds(1f);
        mouse_cursor_mng.mouse_cursor_on_off(true);
        SceneManager.LoadScene(num);
    }
    public void quit_game()
    {
        StartCoroutine(co_quit_and_save());
    }
    IEnumerator co_quit_and_save()
    {
        game_manager.Instance.fade_mng.Fade();
        yield return yield_cache.WaitForSecondsRealtime(1f);
        Application.Quit();
    }

    public void save_data()
    {
        game_manager.Instance.save_mng.now_player.player_max_hp = player_mng.player_max_hp;
        game_manager.Instance.save_mng.now_player.player_cur_hp = player_mng.player_hp;
        game_manager.Instance.save_mng.now_player.player_max_stemina = player_mng.player_max_stemina;
        game_manager.Instance.save_mng.now_player.player_attck_power = _weapon.damage;
        game_manager.Instance.save_mng.now_player.player_heal_power = player_mng.heal_power;
        game_manager.Instance.save_mng.now_player.player_heal_pack_max_num = player_mng.heal_pack_max_num;
        game_manager.Instance.save_mng.now_player.player_data_chip = player_mng.player_data_chip;
        game_manager.Instance.save_mng.now_player.player_needed_datachip = upgrade_mng.needed_datachip;
        game_manager.Instance.save_mng.now_player.capsule_index = _capsule.capsule_number;
        game_manager.Instance.save_mng.now_player.capsule_tank_fade_count = _capsule.capsule_fade_count;
        game_manager.Instance.save_mng.now_player.is_seconds_capsule = _capsule.is_seconds_in;
        game_manager.Instance.save_mng.now_player.player_pill_part_num = player_mng.pill_part_num;
        game_manager.Instance.save_mng.now_player.tutorial_count_num = tutorial_mng.trigger_num;
        
        //lock and pill
        //?. 연산자 = Null이 아니라면 참조하고, Null이라면 Null로 처리
        //for (int i = 0; i < lock_n_pill_mng.items.Length; i++)
        //{
        //    string propertyName = $"area{i + 1}_pill_get";
        //    game_manager.Instance.save_mng.now_player.GetType().GetProperty(propertyName)
        //        ?.SetValue(game_manager.Instance.save_mng.now_player, lock_n_pill_mng.items[i].got_the_pill);
        //}

        game_manager.Instance.save_mng.now_player.area1_pill_get = lock_n_pill_mng.items[0].got_the_pill;
        game_manager.Instance.save_mng.now_player.area2_pill_get = lock_n_pill_mng.items[1].got_the_pill;
        game_manager.Instance.save_mng.now_player.area3_pill_get = lock_n_pill_mng.items[2].got_the_pill;
        game_manager.Instance.save_mng.now_player.area4_pill_get = lock_n_pill_mng.items[3].got_the_pill;
        game_manager.Instance.save_mng.now_player.area5_pill_get = lock_n_pill_mng.items[4].got_the_pill;
        game_manager.Instance.save_mng.now_player.area6_pill_get = lock_n_pill_mng.items[5].got_the_pill;

        game_manager.Instance.save_mng.save_data();
    }
    
    void load_data()
    {
        is_game_starting = true;
        game_manager.Instance.save_mng.load_data();

        player_mng.player_max_hp = game_manager.Instance.save_mng.now_player.player_max_hp;
        player_mng.player_hp = game_manager.Instance.save_mng.now_player.player_max_hp;
        player_mng.player_max_stemina = game_manager.Instance.save_mng.now_player.player_max_stemina;
        _weapon.damage = game_manager.Instance.save_mng.now_player.player_attck_power;
        player_mng.heal_power = game_manager.Instance.save_mng.now_player.player_heal_power;
        player_mng.heal_pack_max_num = game_manager.Instance.save_mng.now_player.player_heal_pack_max_num;
        player_mng.heal_pack_num = player_mng.heal_pack_max_num;
        player_mng.player_data_chip = game_manager.Instance.save_mng.now_player.player_data_chip;
        upgrade_mng.needed_datachip = game_manager.Instance.save_mng.now_player.player_needed_datachip;
        _capsule.capsule_number = game_manager.Instance.save_mng.now_player.capsule_index;
        _capsule.capsule_fade_count = game_manager.Instance.save_mng.now_player.capsule_tank_fade_count;
        _capsule.is_seconds_in = game_manager.Instance.save_mng.now_player.is_seconds_capsule;
        player_mng.pill_part_num = game_manager.Instance.save_mng.now_player.player_pill_part_num;
        tutorial_mng.trigger_num = game_manager.Instance.save_mng.now_player.tutorial_count_num;

        //lock and pill
        lock_n_pill_mng.items[0].got_the_pill = game_manager.Instance.save_mng.now_player.area1_pill_get;
        lock_n_pill_mng.lock_door_objs[0].SetActive(!game_manager.Instance.save_mng.now_player.area1_pill_get); //얻었다(트루) 비활성화
        lock_n_pill_mng.items[1].got_the_pill = game_manager.Instance.save_mng.now_player.area2_pill_get;
        lock_n_pill_mng.lock_door_objs[1].SetActive(!game_manager.Instance.save_mng.now_player.area2_pill_get); //얻었다(트루) 비활성화
        lock_n_pill_mng.items[2].got_the_pill = game_manager.Instance.save_mng.now_player.area3_pill_get;
        lock_n_pill_mng.items[2].parent_obj.SetActive(!game_manager.Instance.save_mng.now_player.area3_pill_get);
        lock_n_pill_mng.items[3].got_the_pill = game_manager.Instance.save_mng.now_player.area4_pill_get;
        lock_n_pill_mng.items[3].parent_obj.SetActive(!game_manager.Instance.save_mng.now_player.area4_pill_get);
        lock_n_pill_mng.items[4].got_the_pill = game_manager.Instance.save_mng.now_player.area5_pill_get;
        lock_n_pill_mng.lock_door_objs[2].SetActive(!game_manager.Instance.save_mng.now_player.area5_pill_get); //얻었다(트루) 비활성화

        ui_mng.heal_pack_ui_count(player_mng.heal_pack_max_num.ToString());

        player_mng.transform.position = player_respawn_pos[_capsule.capsule_number].position;
        player_mng.transform.rotation = player_respawn_pos[_capsule.capsule_number].rotation;

        _capsule.en_guider_obj.transform.position = player_respawn_pos[_capsule.capsule_number + 2].position;
        _capsule.en_guider_obj.transform.rotation = player_respawn_pos[_capsule.capsule_number + 2].rotation;

        _capsule.transform.position = player_respawn_pos[_capsule.capsule_number + 4].position;
        _capsule.transform.rotation = player_respawn_pos[_capsule.capsule_number + 4].rotation;

        for (int i = 0; i < tutorial_mng.trigger_num; i++)
        {
            tutorial_mng.tutorial_colliders[i].SetActive(false);
        }


        //is_game_starting = false;
        StartCoroutine(co_deley());
    }
    IEnumerator co_deley()
    {
        yield return null;
        is_game_starting = false;
    }


    public void game_over()
    {
        if (p_movement._anime.GetBool("dead") && count <= 0)
        {
            StartCoroutine(co_game_over());
            count++;
        }
    }
    IEnumerator co_game_over()
    {
        if(player_mng.player_data_chip > 0) { player_mng.player_data_chip -= player_mng.player_data_chip / 2; }
        ui_mng.data_chip_bg_around.color = Color.magenta;
        ui_mng.chip_timer = 2f;
        yield return yield_cache.WaitForSeconds(1f);
        _text_fade.Fade();
        yield return yield_cache.WaitForSeconds(0.5f);
        game_manager.Instance.sound_mng.sm_normal_ef_sound_play("game_over_impact");
        yield return yield_cache.WaitForSeconds(2f);

        p_movement._anime.SetBool("dead", false);
        save_data();
        load_scene(1);
    }

    public void respawn_enemies()
    {
        for (int i = 0; i < _en_kyles.Length; i++)
        {
            //_en_kyles[i].gameObject.SetActive(true);
            _en_kyles[i].transform.position = _en_kyles[i].respawn_pos.position;
            _en_kyles[i].transform.rotation = _en_kyles[i].respawn_pos.rotation;
            _en_kyles[i].en_health = _en_kyles[i].max_en_health;
            _en_kyles[i].en_stemina = 0;
            _en_kyles[i].gameObject.layer = 6;

            _en_kyles[i].is_hit_break = false;
            _en_kyles[i].is_battle_start = false;
            _en_kyles[i].is_chase = false;
            _en_kyles[i].is_dead = false;
            _en_kyles[i].is_player_line_over = false;
            
            for (int j = 0; j < _en_kyles[i].skinnedMeshRenderer.Length; j++)
            {
                _en_kyles[i].skinnedMeshRenderer[j].enabled = true;
            }
            _en_kyles[i].meshRenderer.enabled = true;
            //_en_kyles[i].gameObject.SetActive(false);
        }
        player_mng.player_hp = player_mng.player_max_hp;
        player_mng.player_stemina = 0;
        player_mng.heal_pack_num = player_mng.heal_pack_max_num;
    }
}
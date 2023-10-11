using UnityEngine;

public class en_guider : enemy_manager
{

    [Header("transform")]
    public Transform start_pos;
    public Transform end_pos;
    public Transform talk_icon_pos;
    public AnimationCurve curve;
    public float lerp_timer;
    float cur_time = 0;

    [Header("bool")]
    public bool is_ready_talk;

    enemy_target_checker checker;
    ingame_manager ingame_mng;

    void Awake()
    {
        checker = GetComponent<enemy_target_checker>();
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    private void Start()
    {
        transform.position = start_pos.position;
    }

    void Update()
    {
        move_up_down();
        talk_with_player();
        talk();
    }

    void move_up_down()
    {
        cur_time += Time.deltaTime;

        if (cur_time >= lerp_timer)
        {
            cur_time -= cur_time; // 0ภฬตส ex) 3-3 =0;
        }

        float value = curve.Evaluate(cur_time);

        transform.position = Vector3.Lerp(start_pos.position, end_pos.position, value);
    }

    void talk_with_player()
    {
        if (checker.is_target_checked())
        {
            is_ready_talk = true;
            Vector3 dir = ingame_mng.p_movement.cam_pivot.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            talk();
        }
        else
        {
            is_ready_talk = false;
        }
    }

    void talk()
    {
        if (Input.GetKeyDown(key_setting.keys[key_action.INTERECTION]))
        {
            if(ingame_mng.talk_mng.count == 0)
            {
                ingame_mng.talk_mng.is_t_start_botton();
            }
        }
    }

    #region ui
    //void ui_hp_stemina()
    //{
    //    _en_hp_stemina_ui.set_max_health(max_en_health);
    //    _en_hp_stemina_ui.set_cur_health(en_health);
    //    _en_hp_stemina_ui.set_max_stemina(max_en_stemina);
    //    _en_hp_stemina_ui.set_cur_stemina(en_stemina);
    //}

    //void ui_show_up()
    //{
    //    if (_en_hp_stemina_ui.hp_slider.value >= _en_hp_stemina_ui.hp_slider.maxValue &&
    //        _en_hp_stemina_ui.st_slider.value <= 0f || is_dead)
    //    {
    //        _en_hp_stemina_ui.enemy_hp_ui_group.SetActive(false);
    //        _en_hp_stemina_ui.enemy_st_ui_group.SetActive(false);
    //    }
    //    else
    //    {
    //        _en_hp_stemina_ui.enemy_hp_ui_group.SetActive(true);
    //        _en_hp_stemina_ui.enemy_st_ui_group.SetActive(true);
    //    }
    //}
    #endregion


}

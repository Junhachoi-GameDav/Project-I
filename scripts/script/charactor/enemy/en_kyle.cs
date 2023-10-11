using UnityEngine;
using UnityEngine.AI;
using RootMotion.FinalIK;
using Unity.VisualScripting;

public class en_kyle : enemy_manager
{
    [Header("info")]
    public Transform respawn_pos;
    public Transform target;
    public float max_eyesight_distance;
    public SkinnedMeshRenderer[] skinnedMeshRenderer; //본체 매쉬
    public MeshRenderer meshRenderer; //매쉬

    [Header("bool")]
    public bool is_battle_start;
    public bool is_chase;
    public bool is_hit_break;
    public bool is_super_armor;
    public bool is_player_line_over;
    public bool is_no_atk;
    public bool drawGizmo;
    public bool is_dead;
    public bool is_stop;
    

    int hit_count;

    //time
    [Header("timer")]
    float cool_time=3f;
    public float max_cool_time=3f;
    public float min_cool_time=3f;
    int number;
    float only_chasing_timer;
    public float hit_break_timer;
    [Range(0,10)]public float smooth_look_at;

    [Header("effets")]
    public ParticleSystem when_dead_ef;
    public ParticleSystem when_dead_ef2;
    public ParticleSystem hit_ef;
    public ParticleSystem hit__break_ef;

    Rigidbody rigid;
    NavMeshAgent nav;
    public Animator animator;
    //player_movement p_movement;
    //player_manager p_manager;
    enemy_target_checker en_target_checker;
    public en_kyle_weapon _en_k_weapon;
    en_hp_stemina_ui _en_hp_stemina_ui;
    en_kyle_sound _en_kyle_sound;
    en_kyle_sound2 _en_kyle_sound2;
    //ui_manager ui_mng;
    ingame_manager ingame_mng;
    AimIK aimIK;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //p_movement = FindObjectOfType<player_movement>();
        //p_manager = FindObjectOfType<player_manager>();
        en_target_checker =GetComponentInChildren<enemy_target_checker>();
        _en_k_weapon = GetComponentInChildren<en_kyle_weapon>();
        _en_hp_stemina_ui = GetComponentInChildren<en_hp_stemina_ui>();
        _en_kyle_sound = GetComponentInChildren<en_kyle_sound>();
        _en_kyle_sound2 = GetComponentInChildren<en_kyle_sound2>();
        //ui_mng = FindObjectOfType<ui_manager>();
        ingame_mng = FindObjectOfType<ingame_manager>();
        aimIK = GetComponent<AimIK>();
        //aimIK.solver.target = p_movement.cam_pivot;
        //target = p_movement.transform;
    }
    private void Start()
    {
        aimIK.solver.target = ingame_mng.p_movement.cam_pivot;
        target = ingame_mng.p_movement.transform;
    }
    // Update is called once per frame
    void Update()
    {
        ui_hp_stemina();
        ui_show_up();
        dead_check();
        eyesight_check();
        if (!is_battle_start || ingame_mng.p_movement._anime.GetBool("dead") || is_dead || is_no_atk)
        {
            nav.velocity = Vector3.zero;
            animator.SetFloat("speed", 0f, 0.1f, Time.deltaTime);
            return;
        }
        if (animator.GetBool("cant_move")) { nav.velocity = Vector3.zero; }

        if (!is_hit_break && is_player_line_over)
        {
            nav.SetDestination(target.position);
        }
        else if(!is_hit_break && !is_player_line_over)
        {
            nav.SetDestination(respawn_pos.position);
        }

        hit_break();
        chase();
    }
    private void FixedUpdate()
    {
        freeze_nav_from_rigid();
    }
    void freeze_nav_from_rigid()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    #region ui
    void ui_hp_stemina()
    {
        _en_hp_stemina_ui.set_max_health(max_en_health);
        _en_hp_stemina_ui.set_cur_health(en_health);
        _en_hp_stemina_ui.set_max_stemina(max_en_stemina);
        _en_hp_stemina_ui.set_cur_stemina(en_stemina);
    }

    void ui_show_up()
    {
        if (_en_hp_stemina_ui.hp_slider.value >= _en_hp_stemina_ui.hp_slider.maxValue &&
            _en_hp_stemina_ui.st_slider.value <= 0f || is_dead)
        { 
            _en_hp_stemina_ui.enemy_hp_ui_group.SetActive(false);
            _en_hp_stemina_ui.enemy_st_ui_group.SetActive(false);
        }
        else
        {
            _en_hp_stemina_ui.enemy_hp_ui_group.SetActive(true);
            _en_hp_stemina_ui.enemy_st_ui_group.SetActive(true);
        }
    }
    #endregion

    void eyesight_check()
    {
        if (!is_dead)
        {
            if (Vector3.Distance(transform.position, ingame_mng.p_movement.transform.position) <= max_eyesight_distance + 3.5f)
            {
                if (aimIK.solver.IKPositionWeight < 1) { aimIK.solver.IKPositionWeight += Mathf.Lerp(0, 1, Time.deltaTime * smooth_look_at); }
            }
            else
            {
                if (aimIK.solver.IKPositionWeight > 0) { aimIK.solver.IKPositionWeight -= Mathf.Lerp(0, 1, Time.deltaTime * smooth_look_at); }
            }
        }
        else
        {
            aimIK.solver.IKPositionWeight = 0;
        }
        
        //플레이어 거리에 따라 체크
        if (Vector3.Distance(transform.position, ingame_mng.p_movement.transform.position) <= max_eyesight_distance && is_player_line_over)
        {
            is_battle_start = true;
        }
        else if(!is_player_line_over && Vector3.Distance(respawn_pos.position, transform.position) > nav.stoppingDistance)
        {
            is_battle_start = true;
        }
        else
        {
            is_battle_start = false;
            animator.SetFloat("speed", 0, 0.1f, Time.deltaTime);
        }
    }

    void chase()
    {
        
        if(is_stop)
        { 
            nav.velocity = Vector3.zero;
            animator.SetFloat("speed", 0, 0.1f, Time.deltaTime);
            if(number <= 0) { attecking(); number++; }
            stop_move_cool_time();
            return;
        }

        if(en_target_checker.is_grab_or_kick_skill_checked() || en_target_checker.is_target_checked())
        {
            animator.SetFloat("speed", 0, 0.1f, Time.deltaTime);

            transform.LookAt(target);
            nav.velocity = Vector3.zero;
            
            is_stop = true;
        }
        else
        {
            animator.SetFloat("speed", 1, 0.1f,Time.deltaTime);
            stemina_heal_sys();
        }
    }
    
    void stemina_heal_sys()
    {
        if(only_chasing_timer < 2f)
        {
            is_stemina_heal = false;
            only_chasing_timer += Time.deltaTime;
        }
        else
        {
            is_stemina_heal = true;
            if(is_stemina_heal && en_stemina > 0)
            {
                en_stemina -= Time.deltaTime * (max_en_stemina * 0.1f);
            }
            else
            {
                en_stemina = 0;
            }
        }
    }
    void stop_move_cool_time()
    {
        if (cool_time >= 0 && !is_hit_break && !is_dead)
        {
            cool_time -= Time.deltaTime;
        }
        else
        {
            int num = Random.Range(0, 2);
            only_chasing_timer = 0;
            number = 0;
            is_stop = false;
            if (num > 0) { cool_time = max_cool_time; } else { cool_time = min_cool_time; }
            
        }
    }

    void attecking()
    {
        if(!is_hit_break)
        {
            int num = Random.Range(0, 5);
            animator.SetTrigger("atk");
            animator.SetInteger("ran_num", num);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("melee") && !is_dead && hit_count<=0)
        {
            collider_off();
            trail_off();
            int num = Random.Range(0, 2);
            animator.SetInteger("ran_num", num);
            hit_ef.Play();
            kyle_hit_sound();
            if (!is_hit_break)
            {
                if (is_super_armor)
                {
                    en_stemina += ingame_mng.p_movement.p_weapon.damage;
                    en_health -= ingame_mng.p_movement.p_weapon.damage;
                }
                else
                {
                    animator.SetTrigger("hit");
                    en_stemina += ingame_mng.p_movement.p_weapon.damage;
                    en_health -= ingame_mng.p_movement.p_weapon.damage;
                }
            }
            else
            {
                animator.ResetTrigger("hit");
                collider_off();
                trail_off();
                en_health -= ingame_mng.p_movement.p_weapon.damage * 3f;
            }
            hit_count++;
            Invoke("reset_hit_count", 0.1f);
        }
    }
    void reset_hit_count()
    {
        hit_count = 0;
    }
    void dead_check()
    {
        if(en_health < 1 && !is_dead)
        {
            en_health = 0;
            en_stemina = 0;
            animator.ResetTrigger("hit");
            animator.ResetTrigger("atk");
            int num = Random.Range(0, 2);
            if(num > 0) { animator.Play("dead2"); } else { animator.Play("dead1"); }
            gameObject.layer = 12;
            animator.SetBool("dead", true);
            animator.SetBool("hit_break", false);
            ingame_mng.p_movement.target_lock_on = false;
            hit__break_ef.Stop();
            _en_kyle_sound2.play_sounds("dead_voice");
            nav.enabled = true;
            is_hit_break = false;
            is_stop = false;
            hit_break_timer = 4f;
            cool_time = min_cool_time;
            number = 0;
            only_chasing_timer = 0;
            is_dead = true;
        }
    }

    void hit_break()
    {
        if(en_stemina >= max_en_stemina)
        {
            animator.ResetTrigger("hit");
            en_stemina = max_en_stemina;
            hit_break_timer -= Time.deltaTime;
            if (!is_hit_break) { animator.Play("Hit To Body"); hit__break_ef.Play(); }
            nav.velocity = Vector3.zero;
            nav.enabled = false;
            only_chasing_timer = 0;
            cool_time = max_cool_time;
            is_hit_break = true;
            collider_off();
            trail_off();
            animator.SetBool("hit_break", true);

            if (hit_break_timer <= 0)
            {
                nav.enabled = true;
                is_hit_break = false;
                animator.SetBool("hit_break", false);
                hit__break_ef.Stop();
                is_stop = false;
                en_stemina = 0;
                hit_break_timer = 4f;
            }
        }
    }

    public void destroy_en_kyle()
    {
        when_dead_ef.Play(); when_dead_ef2.Play();
        ingame_mng.player_mng.player_data_chip += data_chip;
        ingame_mng.ui_mng.data_chip_bg_around.color = Color.magenta;
        ingame_mng.ui_mng.chip_timer = 2f;
        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].enabled = false;
        }
        //for (int i = 0; i < meshRenderer.Length; i++)
        //{
        //    meshRenderer[i].enabled = false;
        //}
        int num = Random.Range(0, 2);
        if (num > 0) { _en_kyle_sound.play_sounds("dead_sound"); }
        else { _en_kyle_sound.play_sounds("dead_sound2"); }

        Invoke("destroy_en_kyle2", 2f);
    }
    void destroy_en_kyle2()
    {
        animator.ResetTrigger("hit");
        animator.ResetTrigger("atk");
        animator.SetBool("hit_break", false);
        meshRenderer.enabled = false;
        animator.Play("move_tree");
        gameObject.SetActive(false);
    }

    #region animation event
    public void collider_on()
    {
        _en_k_weapon.melee_collider.enabled = true;
    }
    public void collider_off()
    {
        _en_k_weapon.melee_collider.enabled = false;
    }
    public void trail_on()
    {
        _en_k_weapon.trail[0].enabled = true;
        _en_k_weapon.trail[1].enabled = true;
    }

    public void trail_off()
    {
        _en_k_weapon.trail[0].enabled = false;
        _en_k_weapon.trail[1].enabled = false;
    }

    public void cant_move_true()
    {
        animator.SetBool("cant_move", true);
    }

    public void cant_move_flase()
    {
        animator.SetBool("cant_move", false);
    }
    #endregion

    #region sound

    public void kyle_footstep_sound()
    {
        if(animator.GetFloat("speed") > 0.3f) { _en_kyle_sound2.play_sounds("footstep_sound"); }
    }

    public void kyle_hit_sound()
    {
        int num = Random.Range(0, 2);
        if(num > 0)
        {
            _en_kyle_sound2.play_sounds("hit_voice");
            _en_kyle_sound.play_sounds("hit_sound");
        }
        else
        {
            _en_kyle_sound2.play_sounds("hit_voice2");
            _en_kyle_sound.play_sounds("hit_sound2");
        }
    }

    public void kyle_atk_sound()
    {
        int num =Random.Range(0, 2);
        if(num > 0) { _en_kyle_sound.play_sounds("swing_weapon"); }
        else { _en_kyle_sound.play_sounds("swing_weapon2"); }
    }

    public void kyle_hit_break_sound()
    {
        _en_kyle_sound.play_sounds("hit_break");
        _en_kyle_sound2.play_sounds("hit_break_voice");
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, max_eyesight_distance);
    }
}

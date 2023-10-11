using RootMotion.FinalIK;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemy : enemy_manager
{
    [Header("info")]
    public Transform target;
    public Transform grab_hit_pos;
    public BoxCollider kick_boxcollider;
    public BoxCollider grab_boxcollider;
    public ParticleSystem hit_effect;
    public ParticleSystem hit_break_effect;
    public GameObject[] life_objs;

    [Header("component")]
    public text_fade _text_fade;
    Rigidbody rigid;
    public Animator animator;
    public Animator weapon_animator;
    NavMeshAgent nav;
    enemy_target_checker en_target_checker;
    public en_weapon _en_weapon;
    enemy_sound _enemy_sound;
    ingame_manager ingame_mng;
    LookAtIK lookAt_ik;

    [Header("bool")]
    public bool battle_start;
    public bool is_run;
    public bool is_chase;
    public bool is_hit_break;
    public bool is_dead;

    float move_anime_time;
    float min_time;
    float chase_cool_time;
    float chase_walk_time;
    float hit_break_time;

    float sit_and_watch_timer;

    public int swhich_pettern_num;

    //이펙트 포지션 미리 캐싱
    Vector3 hit_pos0;
    Vector3 hit_pos1;
    Vector3 hit_pos2;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        en_target_checker = GetComponentInChildren<enemy_target_checker>();
        _enemy_sound = GetComponentInChildren<enemy_sound>();
        ingame_mng =FindObjectOfType<ingame_manager>();
        lookAt_ik = GetComponent<LookAtIK>();
    }
    private void Start()
    {
        cacheing_effects();
        hit_break_time = 5f;
    }
    void cacheing_effects()
    {
        hit_pos0 = new Vector3(0, 1.4f, 0);
        hit_pos1 = new Vector3(0.2f, 1.7f, 0);
        hit_pos2 = new Vector3(-0.2f, 1.7f, 0);
    }
    private void FixedUpdate()
    {
        freeze_nav_from_rigid();
    }

    void Update()
    {
        hit_break();
        if (!battle_start || ingame_mng.p_movement._anime.GetBool("dead") || is_hit_break || is_dead)
        {
            nav.velocity = Vector3.zero;

            if(en_target_checker.is_target_checked() && sit_and_watch_timer < 1 && is_dead || is_hit_break)
            {
                sit_and_watch_timer += Time.deltaTime;
                lookAt_ik.solver.IKPositionWeight = Mathf.Lerp(0, 1, sit_and_watch_timer);
            }
            return;
        }
        nav.SetDestination(target.position);

        boss_pettern();
        kick_and_grab_check();
        
    }
    void hit_break()
    {
        if (en_stemina >= max_en_stemina && !animator.GetBool("hit_break") && !is_dead)
        {
            is_hit_break = true;
            hit_break_effect.Play();
            animator.Play("Hit To Body_en");
            weapon_animator.Play("move");
            collider_off();
            
            animator.SetBool("atk", false); animator.SetBool("kick", false);
            weapon_animator.SetBool("atk", false); weapon_animator.SetBool("kick", false);
            weapon_animator.SetBool("hit_break", is_hit_break); animator.SetBool("hit_break", is_hit_break);
        }

        if(hit_break_time >= 0 && is_hit_break)
        {
            en_stemina = max_en_stemina;
            hit_break_time -=Time.deltaTime;
        }
        else if(hit_break_time <= 0)
        {
            en_stemina = 0;
            is_hit_break = false;
            weapon_animator.SetBool("hit_break", is_hit_break); animator.SetBool("hit_break", is_hit_break);
            hit_break_time = 5f;
        }

    }
    void freeze_nav_from_rigid()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void boss_pettern()
    {
        switch (swhich_pettern_num)
        {
            case 0:
                run();
                atk();
                chase();
                grab_timer1 = 0;
                grab_timer2 = 0;
                break;
            case 1:
                kick_or_grab();
                break;
            case 2:
                grab();
                break;
        }
    }

    #region chase_pettern
    void run()
    {
        if (is_run) { min_time = 1; nav.speed = 5f; } else { min_time = 0.5f; nav.speed = 2f; }

        if (!is_chase)
        {
            nav.velocity = Vector3.zero;
            move_anime_time = 0;
            animator.SetFloat("speed", move_anime_time);
        }
        else
        {
            if(move_anime_time < min_time) { move_anime_time += Time.deltaTime * 2; }
            animator.SetFloat("speed", move_anime_time);
        }
    }
    
    void atk()
    {
        if (en_target_checker.is_target_checked() && is_chase && !animator.GetBool("kick"))
        {
            int num = Random.Range(0, 2);
            animator.SetInteger("ran_num", num);
            weapon_animator.SetInteger("ran_num", num);
            animator.SetBool("atk", true);
            weapon_animator.SetBool("atk", true);
            is_chase = false;
        }
    }

    void chase()
    {
        if (animator.GetBool("atk"))
        {
            is_stemina_heal = false;
            lookAt_ik.solver.IKPositionWeight = 0;
            return;
        }
        if (!is_chase)
        {
            chase_cool_time += Time.deltaTime;
            if (chase_cool_time > 1f) // 쿨타임
            {
                int num = Random.Range(0, 2);
                if(num > 0) { is_run = true; } else { is_run = false; }
                is_chase=true;
                chase_cool_time = 0;
            }
        }
        else
        {
            if (!is_run)
            {
                chase_walk_time += Time.deltaTime;
                if(chase_walk_time > 3.5f)
                {
                    is_run = true;
                    chase_walk_time = 0;
                    is_stemina_heal = true;
                }
            }
            else
            {
                chase_walk_time += Time.deltaTime;
                if (chase_walk_time > 5f)
                {
                    chase_walk_time = 0;
                    is_stemina_heal = true;
                }
            }
            lookAt_ik.solver.IKPositionWeight = Mathf.Lerp(0, 1, chase_walk_time / 3.5f);
        }
    }

    #endregion

    #region kick_and_grab_pettern
    void kick_and_grab_check()
    {
        if (en_target_checker.is_grab_or_kick_skill_checked() && !animator.GetBool("atk"))
        {
            if (!ingame_mng.p_movement._anime.GetBool("grab_hit"))
            {
                swhich_pettern_num = 1;
            }
            else
            {
                swhich_pettern_num = 2;
            }
        }
        else if (ingame_mng.p_movement._anime.GetBool("grab_hit"))
        {
            swhich_pettern_num = 2;
        }
    }
    void kick_or_grab()
    {
        nav.velocity = Vector3.zero;
        is_chase = false;
        chase_cool_time = 0;
        int num = Random.Range(0, 3); //발차기를 할지 잡기를 할지 결정 // 0 1,발차기, 2 잡기
        animator.SetInteger("ran_num", num);
        animator.SetBool("kick", true);
        ingame_mng.p_movement._anime.SetBool("pushed_hit", true);
    }

    float grab_timer1;
    float grab_timer2;
    void grab()
    {
        grab_boxcollider.enabled = false;
        nav.velocity = Vector3.zero;
        is_chase = false;
        animator.SetBool("kick", false); // 애니메이션 끝내기 위함
        ingame_mng.p_movement._anime.SetBool("pushed_hit", false);
        
        //위치
        ingame_mng.p_movement._anime.applyRootMotion = false;
        ingame_mng.p_movement.transform.position = grab_hit_pos.position;
        ingame_mng.p_movement.transform.rotation = grab_hit_pos.rotation;
        
        //시간 지연
        if(grab_timer1 <= 0.28f)
        {
            grab_timer1 += Time.deltaTime;
        }
        else { grab_anime_deley(); }
        if(grab_timer2 <= 3.1f)
        {
            grab_timer2 += Time.deltaTime;
        }
        else { done_grab_or_kick_pettern(); }
    }

    // 애니메이션 이벤트 & 지연 후 함수
    void grab_anime_deley()
    {
        animator.Play("grab2");
        weapon_animator.Play("grab2");
    }
    public void dont_getbool_grab_hit()
    {
        ingame_mng.p_movement._anime.SetBool("grab_hit", false);
    }
    public void done_grab_or_kick_pettern()
    {
        swhich_pettern_num = 0;
        animator.SetBool("kick", false);
        ingame_mng.p_movement._anime.SetBool("pushed_hit", false);
    }
    public void kick_bool()
    {
        animator.SetBool("kick", false);
        ingame_mng.p_movement._anime.SetBool("pushed_hit", false); 
    }

    //킥 박스 콜라이더
    public void kick_collider_on()
    {
        kick_boxcollider.enabled = true;
    }
    public void kick_collider_off()
    {
        kick_boxcollider.enabled = false;
    }

    //잡기 박스 콜라이더
    public void grab_collider_on()
    {
        grab_boxcollider.enabled = true;
    }
    public void grab_collider_off()
    {
        grab_boxcollider.enabled = false;
    }
    public void player_up_anime()
    {
        ingame_mng.p_movement._anime.Play("bound_down_high");
    }
    public void player_push_anime()
    {
        ingame_mng.p_movement._anime.Play("knockdown_up02");
    }
    //이펙트 이벤트
    public void grab_warning_ef()
    {
        ingame_mng.player_mng.grab_particle.Play();
    }
    public void grab_hitting_ef()
    {
        ingame_mng.player_mng.push_hit_particle.Play();
        //p_manager.player_hp -= en_grab_damage;
        ingame_mng.player_mng.player_stemina += _en_weapon.damage * 0.8f;
    }
    #endregion

    #region collider and trail
    public void collider_on()
    {
        _en_weapon.colliders_on_off(true);
        _en_weapon.trail[0].enabled = true;
    }
    public void collider_off()
    {
        _en_weapon.colliders_on_off(false);
        _en_weapon.trail[0].enabled = false;
    }
    public void en_trail_on()
    {
        _en_weapon.trail[0].enabled = true;
    }
    public void en_trail_off()
    {
        _en_weapon.trail[0].enabled = false;
    }
    #endregion


    int count;
    private void OnTriggerEnter(Collider other)
    {
        if(is_dead) { return; }

        if (other.CompareTag("melee"))
        {
            int num = Random.Range(0, 3);
            switch (num)
            {
                case 0:
                    hit_effect.transform.localPosition = hit_pos0;
                    break;
                case 1:
                    hit_effect.transform.localPosition = hit_pos1;
                    break;
                case 2:
                    hit_effect.transform.localPosition = hit_pos2;
                    break;
                
            }
            hit_effect.Play();
            hit_katana_sound();
             
            if(en_health <= 1 && count < 1) { life_objs[count++].SetActive(false); en_health = max_en_health; en_stemina = max_en_stemina; return; }
            else if(en_health <= 1 && count == 1)
            { 
                life_objs[count].SetActive(false);
                battle_start = false;
                collider_off();
                animator.CrossFade("end_battle", 0.1f);
                ingame_mng._bg_sound.play_sounds("normal_bgm");
                weapon_animator.Play("move");
                animator.SetBool("atk", false); animator.SetBool("kick", false);
                weapon_animator.SetBool("atk", false); weapon_animator.SetBool("kick", false);
                weapon_animator.SetBool("hit_break", false); animator.SetBool("hit_break", false);
                StartCoroutine(co_deley());
                is_dead = true;
            }

            if (!is_hit_break) { en_stemina += ingame_mng.p_movement.p_weapon.damage; en_health -= ingame_mng.p_movement.p_weapon.damage; }
            else
            {
                en_health -= ingame_mng.p_movement.p_weapon.damage * 3;
            }
        }
    }
    IEnumerator co_deley()
    {
        yield return yield_cache.WaitForSeconds(0.5f);
        _text_fade.Fade();
        ingame_mng.player_mng.player_data_chip += data_chip;
        ingame_mng.ui_mng.data_chip_bg_around.color = Color.magenta;
        ingame_mng.ui_mng.chip_timer = 2f;
        ingame_mng.p_movement.target_lock_on = false;
        yield return yield_cache.WaitForSeconds(0.5f);
        game_manager.Instance.sound_mng.sm_ui_sound_play("beat_clone");
    }


    #region sounds
    public void en_swing_whip_sound()
    {
        int num =Random.Range(0, 3);
        if (num == 0)
        {
            _enemy_sound.enemy_sound_play("swing_whip");
        }
        else if (num == 1)
        {
            _enemy_sound.enemy_sound_play("swing_whip2");
        }
        else
        {
            _enemy_sound.enemy_sound_play("swing_whip3");
        }
    }
    public void hit_katana_sound()
    {
        int num = Random.Range(0, 3);
        if (num == 0)
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit_katana");
        }
        else if (num == 1)
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit3_katana");
        }
        else
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit2_katana");
        }
    }
    public void en_kick_sound()
    {
        _enemy_sound.enemy_sound_play("en_kick");
    }
    public void en_grab_sound()
    {
        game_manager.Instance.sound_mng.sm_normal_ef_sound_play("en_grab");
    }

    public void en_grab_whip_sound()
    {
        ingame_mng.player_mng.p_whip_hitted_sound();
    }

    public void en_guard_break_sound()
    {
        if(is_hit_break)
        {
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("break_sound");
        }
    }

    public void en_footstep_sound()
    {
        int num= Random.Range(0, 2);
        if(num == 0)
        {
            _enemy_sound.enemy_sound_play("en_footstep");
        }
        else
        {
            _enemy_sound.enemy_sound_play("en_footstep2");
        }
    }
    #endregion
}

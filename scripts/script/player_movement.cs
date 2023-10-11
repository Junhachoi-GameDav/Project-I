using UnityEngine;
using Random = UnityEngine.Random;
using RootMotion.FinalIK;

public class player_movement : MonoBehaviour
{
    public Transform cam_pivot;
    ground_check ground_c;
    public Animator _anime;
    Camera _camera;
    public CharacterController _characterController;
    public weapon p_weapon;
    player_manager p_manager;
    player_atk_sound _player_atk_sound;
    player_sound _player_sound;
    AimIK aimIK;
    
    ingame_manager ingame_mng;
    

    [Header("move and look")]
    public float smoothness = 50;
    public float forward;
    public float right;
    public float speed = 6;
    public float run_speed = 8;
    public float walk_speed = 2;
    public float rolling_forward;
    public float rolling_right;
    public float smooth_look_at;
    float apply_speed = 0;
    

    //이동 백터값
    public Vector3 move_dir;
    public Vector3 look_dir;
    Vector3 look_forward;
    Vector3 look_right;
    Vector3 player_rotate;

    [Header("gravity")]
    public float ray = 0.2f;
    public float gravity =6f;
    public float jump_force =3f;
    

    [Header("bool")]
    public bool run;
    public bool target_lock_on;
    public bool camera_target;
    public bool falling;
    public bool is_attacking;
    public bool cant_atkking;
    public bool guard;
    public bool guard_on_atking;
    public bool parry;
    public bool jump_flag;
    public bool jump_atk;
    public bool stemina_healing;
    public bool grab_hitted;
    public bool rolling;
    public bool atk_rolling;
    bool dont_rolling;

    [Header("weapon & atk")]
    public GameObject weapon_collider;
    public GameObject block_collider;

    float parry_timer;
    float guard_timer;
    float land_timer;
    float stemina_healing_timer;

    [Header("parry_block_time")]
    public float change_parrying_time;
    public float change_block_time;


    int num;
    int heal_pack_use_count;
    int rolling_deley;
    int feet_l_deley, feet_r_deley;
    
    void Awake()
    {
        ground_c =GetComponentInChildren<ground_check>();
        _anime = this.GetComponent<Animator>();
        _camera = Camera.main;
        _characterController = this.GetComponent<CharacterController>();
        p_weapon = GetComponentInChildren<weapon>();
        p_manager =GetComponent<player_manager>();
        _player_atk_sound = GetComponentInChildren<player_atk_sound>();
        _player_sound = GetComponentInChildren<player_sound>();
        aimIK = GetComponent<AimIK>();
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    
    void Update()
    {
        if (_anime.GetBool("dead") || ingame_mng.upgrade_mng.is_upgrading || ingame_mng.is_pause || ingame_mng._capsule.is_player_in)
        {
            target_lock_on = false;

            return;
        }
        #region inputs
        if (Input.GetKeyDown(key_setting.keys[key_action.TARGETTING]))
        {
            ingame_mng.cams_mng.clear_lock_on();
            target_lock_on = !target_lock_on;
            ingame_mng.cams_mng.handle_lock_on();
        }

        if (Input.GetKey(key_setting.keys[key_action.RUN]) && (forward !=0 || right !=0) &&!guard &&!falling && !is_attacking)
        {
            _anime.SetBool("run", true);
            p_manager.player_stemina += Time.deltaTime * p_manager.player_decrease_stemina;
            run =true;
        }
        else
        {
            _anime.SetBool("run", false);
            run = false;
        }


        #endregion
        
        p_movement();
        p_jump();
        p_rolling();
        p_attack();
        p_guard();
        drink_heal_pack();
        stemina_con();
        move_stop_for_rolling_and_etc();
        foot_step();
    }
    

    void p_movement()
    {
        if (!jump_flag && ground_c.is_ground())
        {
            if (guard || (ground_c.is_wall() && !target_lock_on) || _anime.GetBool("heal") || _anime.GetBool("guard_break")
                || (ground_c.is_enemy() && forward >= 0 &&right == 0))
            {
                apply_speed = walk_speed;
                p_manager.run_particle.gameObject.SetActive(false);
            }
            else
            {
                apply_speed = run ? run_speed : speed;
                if (run) { p_manager.run_particle.gameObject.SetActive(true); }
                else { p_manager.run_particle.gameObject.SetActive(false); }
            }
        }
        else
        {
            apply_speed = speed;
            p_manager.run_particle.gameObject.SetActive(false);
            if (ground_c.is_stair() && guard && !jump_flag || _anime.GetBool("heal"))
            {
                apply_speed = walk_speed;
            }
        }

        //정형화는 밑에서 처리
        
        //중력 적용
        if (ground_c.is_ground() && !falling &&!_anime.GetBool("grab_hit") && !grab_hitted )
        {
            if (Input.GetKey(key_setting.keys[key_action.UP])) { forward = 1f; }
            else if (Input.GetKey(key_setting.keys[key_action.DOWN])) { forward = -1f; }
            else { forward = 0f; }

            if (Input.GetKey(key_setting.keys[key_action.RIGHT])) { right = 1f; }
            else if (Input.GetKey(key_setting.keys[key_action.LEFT])) { right = -1f; }
            else { right = 0f; }

            //forward = Input.GetAxisRaw("Vertical");
            //right = Input.GetAxisRaw("Horizontal");

            look_forward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
            look_right = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z);

            move_dir = (look_forward * forward + look_right * right).normalized;
            look_dir = (look_forward * forward + look_right * right).normalized;
            
            if (jump_flag )
            {
                move_dir.y = jump_force;
            }
        }
        else
        {
            move_dir.y -= gravity * Time.deltaTime;
        }
        if (!is_attacking && !rolling && !ingame_mng.is_game_starting)
        {
            _characterController.Move(move_dir * apply_speed * Time.deltaTime);
        }
       
        //입력 받은 후
        // 락온 
        p_lock_on();
    }

    #region lock_on and turn
    void p_lock_on()
    {
        if (!target_lock_on)
        {
            p_trun();
            ingame_mng.cams_mng.clear_lock_on();
            if (aimIK.solver.IKPositionWeight > 0) { aimIK.solver.IKPositionWeight -= Mathf.Lerp(0, 1, Time.deltaTime * smooth_look_at); }
            
            num = 0;
        }
        else
        {
            //cams_mng.clear_lock_on();
            //cams_mng.handle_lock_on();
            ingame_mng.cams_mng.lock_on_view();
            
            if (ingame_mng.cams_mng.nearest_lock_on_target == null ||
                Vector3.Distance(ingame_mng.cams_mng.transform.position, ingame_mng.cams_mng.nearest_lock_on_target.position) >= ingame_mng.cams_mng.max_lock_on_distance +5f)
            {
                target_lock_on = false;
            }
            else
            {
                if (run || _anime.GetBool("guard_break"))
                {
                    p_trun();
                }
                float mouse_x = Input.GetAxis("Mouse X");

                aimIK.solver.target = ingame_mng.cams_mng.nearest_lock_on_target;
                if (!_anime.GetBool("guard_break"))
                {
                    if (run || jump_flag || grab_hitted) { aimIK.solver.IKPositionWeight = 0; }
                    else if (aimIK.solver.IKPositionWeight < 1 && !ground_c.is_enemy())
                    {
                        aimIK.solver.IKPositionWeight += Mathf.Lerp(0, 1, Time.deltaTime * smooth_look_at);
                    }
                    else if (ground_c.is_enemy() && aimIK.solver.IKPositionWeight > 0)
                    {
                        aimIK.solver.IKPositionWeight -= Mathf.Lerp(0, 1, Time.deltaTime * smooth_look_at);
                    }
                }
                else
                {
                    aimIK.solver.IKPositionWeight = 0;
                }
               

                if (mouse_x < -1.5f && num == 0)
                {
                    ingame_mng.cams_mng.clear_lock_on();
                    ingame_mng.cams_mng.handle_lock_on();
                    if (ingame_mng.cams_mng.lock_on_left_target != null)
                    {
                        ingame_mng.cams_mng.nearest_lock_on_target = ingame_mng.cams_mng.lock_on_left_target;
                        ingame_mng.cams_mng.lock_on_img = ingame_mng.cams_mng.lock_on_img_left;
                    }
                    num = 1;
                    Invoke("num_deley", 0.2f);
                }
                else if(mouse_x > 1.5f && num == 0)
                {
                    ingame_mng.cams_mng.clear_lock_on();
                    ingame_mng.cams_mng.handle_lock_on();
                    if (ingame_mng.cams_mng.lock_on_right_target != null)
                    {
                        ingame_mng.cams_mng.nearest_lock_on_target = ingame_mng.cams_mng.lock_on_right_target;
                        ingame_mng.cams_mng.lock_on_img = ingame_mng.cams_mng.lock_on_img_right;
                    }
                    num = 1;
                    Invoke("num_deley", 0.2f);
                }
            }

        }
    }
    
    void num_deley()
    {
        num = 0;
    }

    void p_trun()
    {
        if ((is_attacking && !cant_atkking) || rolling)
        {
            return;
        }
        if (forward != 0 || right != 0) //이걸로 회전값을 업데이트에서 막아줘야함
        {
            player_rotate = Vector3.Scale(look_dir, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(player_rotate),
            Time.deltaTime * 15);
        }
    }
    #endregion

    //점프는 버그때문에 없애기로 결정
    //중력은 받아야되니까 남긴다.

    int air_num;

    void p_jump()
    {
        if(grab_hitted || _anime.GetBool("guard_break")) { return; }

        if (ground_c.is_ground() || ground_c.is_stair())
        {
            if(land_timer > 0)
            {
                land_timer -=Time.deltaTime;
            }

            if(air_num <= 0) { gameObject.layer = 3; air_num++; }

            _anime.SetBool("ground", true);
            //if (Input.GetKeyDown(KeyCode.F) && land_timer <= 0 && !is_attacking)
            //{
            //    jump_flag = true;
            //    //sound_mng.sm_player_sound_play("p_jump");
            //}
        }
        else if(ground_c.is_stair())
        {
            jump_flag = false;
        }
        else
        {
            air_num = 0;
            land_timer = 0.2f;
            _anime.SetBool("ground", false);
            gameObject.layer = 14;
            jump_flag = false;
            //gameObject.layer = 14;
            //forward = 0;
            //right = 0;
            block_collider_off();
            if (_characterController.velocity.y < -25)
            {
                falling = true;
            }
            else
            {
                falling = false;
            }
        }
    }

    #region rolling
    void p_rolling()
    {
        if (ground_c.is_ground() || ground_c.is_stair())
        {
            if (target_lock_on)
            {
                if (rolling && atk_rolling)
                {
                    //롤링중에는 안쳐다보기
                    aimIK.solver.IKPositionWeight = 0;

                    //공격중에 원하는 방향으로 롤링 & 가만히 있으면 앞으로 롤링
                    if ((forward != 0 || right != 0) && rolling_deley <= 0)
                    {
                        player_rotate = Vector3.Scale(look_dir, new Vector3(1, 0, 1));
                        transform.rotation = Quaternion.LookRotation(player_rotate);
                        rolling_deley++;
                    }
                }
            }
            else
            {
                if(rolling && atk_rolling)
                {
                    //공격중에 원하는 방향으로 롤링 & 가만히 있으면 앞으로 롤링
                    if ((forward != 0 || right != 0) && rolling_deley <= 0)
                    {
                        player_rotate = Vector3.Scale(look_dir, new Vector3(1, 0, 1));
                        transform.rotation = Quaternion.LookRotation(player_rotate);
                        rolling_deley++;
                    }
                }
            }

            if (Input.GetKeyDown(key_setting.keys[key_action.ROLLING]) && !cant_atkking && !dont_rolling &&!_anime.GetBool("hit") && !_anime.GetBool("heal")
                && !_anime.GetBool("grab_hit") && !parry  && !falling && !jump_atk && !_anime.GetBool("guard_break")
                && !_anime.GetBool("rolling") && !_anime.GetBool("dead") 
                && p_manager.player_stemina < (p_manager.player_max_stemina - p_manager.player_decrease_rolling_stemina))
            {

                if (guard || is_attacking) { _anime.CrossFade("roll_front", guard ? 0 : 0.1f); }
                else { _anime.SetBool("rolling", true); }
                //_anime.CrossFade("roll_front", forward != 0 ? 0.1f : 0);
                p_manager.player_stemina += p_manager.player_decrease_rolling_stemina;
                p_manager.rolling_particle.transform.position = transform.position;
                p_manager.rolling_particle.Play();
                all_off();
            }
        }
    }
    public void rolling_true()
    {
        rolling = true;
        atk_rolling = true;
        gameObject.layer = 12;
    }
    public void atk_rolling_false()
    {
        atk_rolling = false;
    }
    public void rolling_false()
    {
        rolling = false;
        gameObject.layer = 3;
        rolling_deley = 0;
        p_manager.rolling_particle.transform.position = transform.position;
        p_manager.rolling_particle.Play();
    }

    public void do_not_rolling()
    {
        dont_rolling = true;
    }
    public void do_rolling()
    {
        dont_rolling = false;
    }
    #endregion

    #region attack 

    void p_attack()
    {
        is_attacking = _anime.GetBool("is_atk");
        cant_atkking = _anime.GetBool("is_atking");
        p_atk();
    }
    /*
    public void p_atk(InputAction.CallbackContext context)
    {
        if (talk_mng.count == 0)
        {
            if (context.performed)
            {
                if (context.interaction is HoldInteraction)
                {

                    _anime.SetTrigger("charge_start");
                }
                else if (context.interaction is PressInteraction)
                {
                    if (!cant_atkking)
                    {
                        _anime.SetTrigger("atk");
                    }

                }
            }
        }
    }
    */
    void p_atk()
    {
        if (Input.GetKeyDown(key_setting.keys[key_action.ATTACK]) && ingame_mng.talk_mng.count == 0 && !cant_atkking
            && ground_c.is_stair()
            && !ingame_mng.is_pause && !ingame_mng.upgrade_mng.is_upgrading && !ingame_mng.tutorial_mng.is_tutorial) 
        {
            _anime.SetTrigger("atk");
        }
        else { _anime.ResetTrigger("atk"); }
    }

    public void jum_atk_true()
    {
        jump_atk = true;  //시작할때 트루
    }
    public void is_atk_false()
    {
        _anime.SetBool("is_atk", false);
    }
    public void is_atking_false()
    {
        _anime.SetBool("is_atking", false);
    }
    //콜라이더
    public void weapon_coll_on()
    {
        weapon_collider.SetActive(true);
    }
    public void weapon_coll_off()
    {
        weapon_collider.SetActive(false);
    }
    //이펙트
    public void on_trail()
    {
        p_weapon.trail[0].enabled = true;
        p_weapon.trail[1].enabled = true;
    }
    public void off_trail()
    {
        p_weapon.trail[0].enabled = false;
        p_weapon.trail[1].enabled = false;
    }
    #endregion

    #region block_guard_parring
    void p_guard()
    {
        guard_on_atking = _anime.GetBool("guard_atk");

        if(guard_timer < change_block_time)
        {
            guard_timer += Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(key_setting.keys[key_action.BLOCK]) && land_timer <=0 && !cant_atkking 
                && !_anime.GetBool("hit") && !_anime.GetBool("heal") && !_anime.GetBool("rolling") && !_anime.GetBool("grab_hit")
                && !_anime.GetBool("dead") && !_anime.GetBool("guard_break"))
            {
                parrying();
                guard = true;
                _anime.SetBool("guard", true);
                if (jump_flag)
                {
                    _anime.CrossFade("Fall A Loop", 0.1f);
                }
            }
            else
            {
                guard = false;
                block_collider.SetActive(false);
                _anime.SetBool("guard", false);
                guard_timer = 0;
                parry_timer = 0;
                if (!grab_hitted) { gameObject.layer = 3; }
            }
        }
    }
    public void guard_atk_true()
    {
        _anime.SetBool("guard_atk", true);
    }
    public void guard_atk_false()
    {
        _anime.SetBool("guard_atk", false);
    }
    public void block_collider_on()
    {
        block_collider.SetActive(true);
    }
    public void block_collider_off()
    {
        block_collider.SetActive(false);
    }
    
    void parrying()
    {
        
        //parry_int = Random.Range(0, 3);
        if (parry_timer < change_parrying_time && guard) // 패링 가능한 시간
        {
            parry_timer += Time.deltaTime;
            block_collider.SetActive(true);
            gameObject.layer = 12;
        }
        else
        {
            if (!grab_hitted) { gameObject.layer = 3; }
            block_collider.SetActive(false);
        }

    }
    #endregion

    #region drink heal pack
    void drink_heal_pack()
    {
        if(is_attacking || !ground_c.is_ground() || guard || p_manager.heal_pack_num <=0
            ||_anime.GetBool("guard_break") || _anime.GetBool("hit")) 
        { return; }
        if (Input.GetKeyDown(key_setting.keys[key_action.HEAL]) && heal_pack_use_count <=0 && ingame_mng.ui_mng.heal_timer <= 0)
        {
            _anime.SetBool("heal", true);
            p_manager.heal_pack_num--;
            p_manager.heal_pill_obj.SetActive(true);
            p_manager.heal_pill_ef.SetActive(true);
            ingame_mng.ui_mng.heal_pack_ui_count(p_manager.heal_pack_num.ToString());
            heal_pack_use_count = 1; p_eatting_heal_pack();
            ingame_mng.ui_mng.heal_timer = 1;
        }
    }
    public void _heal()
    {
        p_manager.heal();
        game_manager.Instance.sound_mng.sm_normal_ef_sound_play("heal_impact");
    }
    public void heal_bool_false()
    {
        _anime.SetBool("heal", false);
        heal_pack_use_count = 0;
    }
    #endregion

    #region stemina
    void stemina_con()
    {
        if(is_attacking || cant_atkking || _anime.GetBool("hit") || 
            _anime.GetBool("guard_atk") || _anime.GetBool("guard") || _anime.GetBool("grab_hit") ||
            falling || run || jump_atk || parry || rolling)
        {
            stemina_healing = false;
            stemina_healing_timer = 0;
            return;
        }
        if (stemina_healing_timer < 1.5f)
        {
            stemina_healing_timer += Time.deltaTime;
        }
        else
        {
            stemina_healing = true;
        }
    }
    #endregion

    void move_stop_for_rolling_and_etc()
    {
        //롤링
        if ((rolling && ground_c.is_enemy()) || (rolling && ground_c.is_wall()))
        {
            _anime.applyRootMotion = false;
            if (!_anime.applyRootMotion && !rolling)
            {
                _anime.applyRootMotion = true;
            }
        }
        else if (!_anime.GetBool("rolling") && !ingame_mng.is_game_starting && !_anime.GetBool("grab_hit"))
        {
            _anime.applyRootMotion = true;
        }
    }
    
    void all_off()
    {
        _anime.SetBool("guard", false);
        _anime.SetBool("guard_atk", false);
        _anime.ResetTrigger("guard_hit");
        _anime.ResetTrigger("hit_trigger");
        _anime.SetBool("hit", false);
    }

    void foot_step()
    {
        if (forward == 0 && right == 0) { return; }
        
        if (feet_l_deley <= 0 && ground_c.is_l_foot_on_floor())
        {
            _player_sound.player_sound_play("p_footstep");
            feet_l_deley++;
        }

        if (feet_r_deley <= 0 && ground_c.is_r_foot_on_floor())
        {
            _player_sound.player_sound_play("p_footstep");
            feet_r_deley++;
        }
    }

    #region sounds
    public void p_rolling_sound()
    {
        int num = Random.Range(0, 2);
        if(num == 0)
        {
            _player_sound.player_sound_play("p_rolling");
        }
        else
        {
            _player_atk_sound.player_atk_sound_play("p_rolling2");
        }
    }
    public void p_atk_voice()
    {
        _player_atk_sound.player_atk_sound_play("p_rolling");    
    }
    public void p_landing_sound()
    {
        _player_sound.player_sound_play("p_landing");
    }

    public void p_eatting_heal_pack()
    {
        _player_atk_sound.player_atk_sound_play("p_eatting_pill");
    }

    //애니메이션에서 반대로 놓자
    public void p_footstep_l_deley()
    {
        feet_l_deley = 0;
    }
    public void p_footstep_r_deley()
    {
        feet_r_deley = 0;
    }
    #endregion
}

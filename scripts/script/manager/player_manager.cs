using RootMotion.FinalIK;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class player_manager : MonoBehaviour
{
    [Header("state")]
    public float player_hp;
    public float player_max_hp;
    public float player_stemina;
    public float player_max_stemina;
    public float player_decrease_stemina;
    public float player_decrease_rolling_stemina;
    public int player_data_chip;

    [Header("heal")]
    public int heal_pack_num;
    public int heal_pack_max_num;
    public int heal_power;
    public int pill_part_num;
    public GameObject heal_pill_obj;
    public GameObject heal_pill_ef;

    public bool parring;

    [Header("Effects")]
    public ParticleSystem guard_particle;
    public ParticleSystem parring_particle;
    public ParticleSystem hit_particle;
    public ParticleSystem push_hit_particle;
    public ParticleSystem push_down_particle;
    public ParticleSystem grab_particle;
    public ParticleSystem heal_particle;
    public ParticleSystem heal_particle2;
    public ParticleSystem debufe_particle;
    public ParticleSystem rolling_particle;
    public ParticleSystem run_particle;

    public float guard_partical_scale;
    public float partical_scale;

    public FullBodyBipedIK grounderFBBIK;
    player_movement player_move;
    ingame_manager in_game_mng;
    player_atk_sound _player_atk_sound;
    player_sound _player_sound;
    enemy_sound _enemy_sound;
    weapon en_weapon;

    Vector3 hit_ef_pos0;
    Vector3 hit_ef_pos1;
    Vector3 hit_ef_pos2;
    Vector3 hit_ef_pos3;
    Vector3 hit_ef_pos4;

    float g_b_timer;
    public float max_g_b_timer;
    int p_g_b_s_num = 0;

    private void Awake()
    {
        player_move = GetComponent<player_movement>();
        grounderFBBIK = GetComponent<FullBodyBipedIK>();
        in_game_mng = FindObjectOfType<ingame_manager>();
        _player_atk_sound = GetComponentInChildren<player_atk_sound>();
        _player_sound = GetComponentInChildren<player_sound>();
        _enemy_sound = FindObjectOfType<enemy_sound>();
    }
    private void Start()
    {
        hit_ef_pos_caching();
        g_b_timer = max_g_b_timer;
    }

    private void Update()
    {
        guard_break_and_dead();
    }

    void hit_ef_pos_caching()
    {
        hit_ef_pos0 = new Vector3(0, 1.4f, -0.2f);
        hit_ef_pos1 = new Vector3(0.18f, 1.7f, -0.2f);
        hit_ef_pos2 = new Vector3(-0.18f, 1.7f, -0.2f);
        hit_ef_pos3 = new Vector3(0.18f, 1, -0.2f);
        hit_ef_pos4 = new Vector3(-0.18f, 1, -0.2f);
    }
    public void hit_ef_swich_case()
    {
        int num = Random.Range(0, 5);
        switch (num)
        {
            case 0:
                hit_particle.transform.localPosition = hit_ef_pos0;
                break;
            case 1:
                hit_particle.transform.localPosition = hit_ef_pos1;
                break;
            case 2:
                hit_particle.transform.localPosition = hit_ef_pos2;
                break;
            case 3:
                hit_particle.transform.localPosition = hit_ef_pos3;
                break;
            case 4:
                hit_particle.transform.localPosition = hit_ef_pos4;
                break;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (player_move.parry || player_move._anime.GetBool("dead") || player_move.rolling) { return; } //패링, dead 이면 리턴
        
        if (other.CompareTag("en_melee"))
        {
            all_off();
            en_weapon = other.GetComponent<weapon>();
            if (player_move.guard)
            {
                player_move._anime.SetTrigger("guard_hit");
                guard_particle.Play();
                player_stemina += en_weapon.damage;
                block_katana_sound();
            }
            else
            {
                player_move._anime.SetBool("hit", true); player_move._anime.SetTrigger("hit_trigger");
                player_move._anime.SetBool("guard", false); player_move._anime.SetBool("guard_atk", false);
                in_game_mng.ui_mng.heal_timer = 1;

                //weapon의 타입에 따라 사운드 바뀜
                switch (en_weapon._type.ToString())
                {
                    case "whip":
                        p_whip_hitted_sound();
                        break;
                    case "melee":
                        p_melee_hitted_sound();
                        break;
                    case "axe":
                        p_axe_hitted_sound();
                        break;
                    case "kick":
                        p_hitted_kick_sound();
                        break;
                }
                if (in_game_mng._enemy.animator.GetBool("kick"))
                {
                    push_hit_particle.transform.localPosition = hit_ef_pos0;
                    push_hit_particle.Play();
                    player_hp -= en_weapon.damage;
                    player_stemina += en_weapon.damage * 1.2f;
                    //if (player_move._anime.GetBool("guard_break")) { p_whip_hitted_sound(); }
                    //else { p_hitted_kick_sound(); }
                }
                else
                {
                    hit_ef_swich_case();
                    hit_particle.Play();
                    //p_whip_hitted_sound();
                    player_hp -= en_weapon.damage;
                    player_stemina += en_weapon.damage;
                }
            }
            all_off();
        }

        if (other.CompareTag("en_grab"))
        {
            all_off();
            en_weapon = other.GetComponent<weapon>();
            gameObject.layer = 12;
            player_move._anime.SetTrigger("grab_hit_trigger");
            player_move._anime.SetBool("grab_hit", true);
            p_whip_hitted_sound();
            player_move.grab_hitted = true;
            push_hit_particle.transform.localPosition = Vector3.zero;
            grounderFBBIK_off();
            all_off();
        }
    }

    //날라갈때 다리 자연스럽게
    public void grounderFBBIK_off()
    {
        grounderFBBIK.enabled = false;
    }
    public void grounderFBBIK_on()
    {
        grounderFBBIK.enabled = true;
    }
    void all_off()
    {
        player_move.off_trail();
        player_move.weapon_coll_off();
        player_move.stemina_healing=false;
        player_move._anime.SetBool("heal", false);
        player_move._anime.SetBool("rolling", false);
        player_move.rolling = false;
        player_move.do_rolling();
        heal_pill_obj.SetActive(false);
        heal_pill_ef.SetActive(false);
    }
    
    public void grab_hit_false()
    {
        player_move.grab_hitted = false;
    }
    public void dead_check()
    {
        player_hp -= en_weapon.damage * 3f;
        if(player_hp <= 0)
        {
            player_move._anime.SetBool("dead", true);
            gameObject.layer = 12;
            in_game_mng.cams_mng.clear_lock_on();
        }
        else
        {
            gameObject.layer = 3;
        }
    }
    public void reset_pushed_hit_ef_pos()
    {
        push_hit_particle.transform.localPosition = hit_ef_pos0;
    }

    public void heal()
    {
        heal_particle.Play(); heal_particle2.Play();
        heal_pill_obj.SetActive(false);
        heal_pill_ef.SetActive(false);
        if(player_hp < player_max_hp && !(heal_power + player_hp > player_max_hp))
        {
            player_hp += heal_power;
        }
        else
        {
            player_hp = player_max_hp;
        }
    }

    void guard_break_and_dead()
    {
        if(in_game_mng.upgrade_mng.is_upgrading || in_game_mng.tutorial_mng.is_tutorial) { return; }
        if (player_move._anime.GetBool("guard_break"))
        {
            in_game_mng.cams_mng.cams_lens_ef(0.5f);
            in_game_mng.cams_mng.cams_lens_ef3(1);
        }
        else if(!in_game_mng.is_pause)
        {
            in_game_mng.cams_mng.cams_lens_ef(0);
            in_game_mng.cams_mng.cams_lens_ef3(0);
        }

        //가드 파괴시 (잡기후 및 롤링후)

        if (player_stemina >= player_max_stemina && !player_move.grab_hitted
            && !player_move._anime.GetBool("guard_break"))
        {
            player_stemina = player_max_stemina;
            if(p_g_b_s_num <= 0)
            {
                player_move._anime.Play("guard_b_begin");
                debufe_particle.Play();
                p_guard_break_sound();
                p_g_b_s_num++;
            }
            player_move._anime.SetBool("guard_break", true);
        }
        


        //죽을시
        if (player_hp <= 0 && player_move.grab_hitted == false)
        {
            player_move._anime.SetBool("dead", true);
            player_move._anime.Play("dead1");
            gameObject.layer = 12; //parry_ 무적
            all_off();
            in_game_mng.cams_mng.clear_lock_on();
        }
        else if (player_move._anime.GetBool("guard_break")) //가드 파괴시
        {
            if(g_b_timer > 0)
            {
                g_b_timer -= Time.deltaTime;
                player_move._anime.ResetTrigger("guard_hit");
                player_move._anime.ResetTrigger("hit_trigger");
                player_move._anime.SetBool("hit", false);
                player_move._anime.SetBool("guard", false);
                player_move.rolling = false;
                player_stemina = player_max_stemina;
            }
            else
            {
                player_stemina = 0;
                p_g_b_s_num = 0;
                g_b_timer = max_g_b_timer;
                player_move._anime.SetBool("guard_break", false);
            }
        }
    }

    public void ground_hit_eff()
    {
        push_down_particle.Play();
    }

    /*
    #region slash_ef_for_anime_event
    public void slash_ef()
    {
        slash_particles[0].Play();
    }
    public void slash_ef1()
    {
        slash_particles[1].Play();
    }
    public void slash_ef2()
    {
        slash_particles[2].Play();
    }
    public void slash_ef3()
    {
        slash_particles[3].Play();
    }
    public void slash_ef4()
    {
        slash_particles[4].Play();
    }
    public void slash_ef5()
    {
        slash_particles[5].Play();
    }
    public void slash_ef6()
    {
        slash_particles[6].Play();
    }
    #endregion
    */
    #region sounds
    
    public void swing_katana_sound()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            _player_atk_sound.player_atk_sound_play("swing_katana");
        }
        else
        {
            _player_atk_sound.player_atk_sound_play("swing_katana2");
        }
    }
    public void block_katana_sound()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            _player_atk_sound.player_atk_sound_play("block_katana");
        }
        else
        {
            _player_atk_sound.player_atk_sound_play("block2_katana");
        }
    }
    public void parry_katana_sound()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            _player_atk_sound.player_atk_sound_play("parry_katana");
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("parry_impact");
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("parry_impact2");
        }
        else
        {
            _player_atk_sound.player_atk_sound_play("parry2_katana");
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("parry_impact");
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("parry_impact2");
        }
    }
    public void p_hitted_sound()
    {
        int num = Random.Range(0, 3);
        if (player_move._anime.GetBool("dead"))
        {
            _player_sound.player_sound_play("p_hitted"); //죽는소리임
            return;
        }
        if (num == 0)
        {
            _player_sound.player_sound_play("p_hitted2");
        }
        else if(num == 1)
        {
            _player_sound.player_sound_play("p_hitted3");
        }
    }



    public void p_whip_hitted_sound()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit_whip");
        }
        else
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit_whip2");
        }
    }
    public void p_melee_hitted_sound()
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
        else if (num == 2)
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit2_katana");
        }
    }
    public void p_axe_hitted_sound()
    {
        int num = Random.Range(0, 2);
        if (num == 0)
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit_axe");
        }
        else
        {
            game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit2_axe");
        }
    }
    public void p_hitted_kick_sound()
    {
        game_manager.Instance.sound_mng.sm_hit_ef_sound_play("hit_kick");
    }
    public void p_grab_down_sound()
    {
        _player_sound.player_sound_play("p_grab_downed");
    }
    public void p_grab_down_sound2()
    {
        _enemy_sound.enemy_sound_play("grab_down");
    }

    public void p_guard_break_sound()
    {
        game_manager.Instance.sound_mng.sm_normal_ef_sound_play("break_sound");
    }
    #endregion
}

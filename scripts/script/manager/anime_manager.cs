using UnityEngine;


public class anime_manager : MonoBehaviour
{
    player_movement p_movement;
    //player_manager p_manager;
    ground_check ground_c;
    //Animator animator;

    
    float percent;

    private void Awake()
    {
        ground_c = FindObjectOfType<ground_check>();
        p_movement = FindObjectOfType<player_movement>();
    }
    // Update is called once per frame
    void Update()
    {
        move_anime();
        fall_anime();
        //root_motion_bool();
        
    }

    void move_anime()
    {
        if((!ground_c.is_ground() && !ground_c.is_stair()))
        {
            //p_movement.forward = 0;
            //p_movement.right = 0;
            percent = 0;
        }
        else
        {
            if(p_movement.forward != 0 || p_movement.right != 0)
            {
                if (ground_c.is_wall())
                {
                    percent = 0.22f;
                }
                else
                {
                    percent = (p_movement.run ? 2 : 1) * p_movement.move_dir.magnitude;
                }
            }
            else
            {
                percent = 0;
            }
        }

        if (!p_movement.target_lock_on)
        {
            p_movement._anime.SetFloat("vertical", percent, 0.1f, Time.deltaTime);
            p_movement._anime.SetFloat("horizontal", 0, 0.1f, Time.deltaTime);
        }
        else
        {
            if (p_movement.run || p_movement._anime.GetBool("guard_break")) 
            { 
                p_movement._anime.SetFloat("vertical", percent, 0.1f, Time.deltaTime);
                p_movement._anime.SetFloat("horizontal", 0, 0.1f, Time.deltaTime);

            }
            else 
            { 
                p_movement._anime.SetFloat("vertical", p_movement.forward, 0.1f, Time.deltaTime);
                p_movement._anime.SetFloat("horizontal", p_movement.right, 0.1f, Time.deltaTime);
            }
        }
    }

    float fall_timer;
    void fall_anime()
    {
        if (!ground_c.is_ground() && !ground_c.is_stair())
        {
            p_movement._anime.SetBool("is_atk", false);
            p_movement._anime.SetBool("is_atking", false);
        }
        else
        {
            if (p_movement.falling)
            {
                p_movement._anime.Play("Hard Landing");
                p_movement.move_dir = new Vector3(0,p_movement.move_dir.y,0);  // ∏ÿ√„
                p_movement.forward = 0;
                p_movement.right = 0;
                if(fall_timer <= 1.7f)
                {
                    fall_timer += Time.deltaTime;
                }
                else { fall_anime_cool_time(); }
                return;
            }
        }
    }

    void fall_anime_cool_time()
    {
        p_movement.falling = false;
        fall_timer = 0;
    }
    //void jump_atk_anime_cool_time()
    //{
    //    p_movement.jump_atk = false;
    //}
    
    //void root_motion_bool()
    //{
    //    if(!ground_c.is_cliff() || ground_c.is_wall())
    //    {
    //        p_movement._anime.applyRootMotion = false;
    //    }
    //    else
    //    {
    //        if (p_movement.is_attacking || p_movement._anime.GetBool("pushed_hit") || p_movement._anime.GetBool("grab_hit"))
    //        {
    //            p_movement._anime.applyRootMotion = true;
    //        }
    //        else
    //        {
    //            p_movement._anime.applyRootMotion = false;
    //        }
    //    }
    //}
    
}

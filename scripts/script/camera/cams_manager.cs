using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class cams_manager : MonoBehaviour
{

    public Volume volume;

    
    ingame_manager ingame_mng;
    
    public CinemachineVirtualCamera target_camara;
    public CinemachineVirtualCamera VirtualCamera;
    
    //락온
    [Header("lock on")]
    public List<enemy_manager> available_targets = new List<enemy_manager>();
    public float lockOnDistance = 30f;
    public float max_lock_on_distance = 30;
    public float locked_pivot_pos = 3.3f;
    public float unlocked_pivot_pos = 3.03f;
    public float lock_on_hit_smoothness;
    public LayerMask enemyLayer;
    public LayerMask ground_layer;

    [Header("mouse_sensitivity")]
    public float mouse_sensitivity; 
    int count;
    //float mouse_y;

    float timer;
    float cams_distance;
    float c_timer;

    [Header("lock on targets")]
    //public Transform cur_lock_on_target;
    public Transform nearest_lock_on_target;
    public Transform lock_on_left_target;
    public Transform lock_on_right_target;
    public Transform lock_on_img;
    public Transform lock_on_img_left;
    public Transform lock_on_img_right;
    public GameObject lock_on_cam;

    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    void Start()
    {
        mouse_sensitivity = game_manager.Instance.setting_mng.mouse_sensitivity_slider.value * 10f;
    }

    void Update()
    {
        toggle();
        mouse_input();
    }

    // 카메라 콜라이더 수정
    private void LateUpdate()
    {
        if (nearest_lock_on_target != null && ingame_mng.p_movement.target_lock_on)
        {
            RaycastHit hit;
            RaycastHit hit2;

            float en_distance = Vector3.Distance(target_camara.transform.position, nearest_lock_on_target.transform.position);
            float p_distance = Vector3.Distance(target_camara.transform.position, ingame_mng.p_movement.cam_pivot.transform.position);

            Vector3 dir = (target_camara.transform.position - nearest_lock_on_target.position) * -1;
            Vector3 dir_back = target_camara.transform.position - ingame_mng.p_movement.cam_pivot.transform.position;


            if (Physics.Raycast(target_camara.transform.position, dir, out hit, en_distance, ground_layer))
            {
                timer += Time.deltaTime;
                if (timer > 1f) { nearest_lock_on_target = null; timer = 0; }
            }
            else { timer = 0; }

            if (Physics.SphereCast(ingame_mng.p_movement.cam_pivot.transform.position, 0.2f , dir_back, out hit2, p_distance, ground_layer) &&
                hit2.distance <= 4.8f)
            {
                //target_camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = hit2.distance;
                cams_distance = Mathf.Clamp(hit2.distance, 0, 4.8f);
            }
            else
            {
                cams_distance = 4.8f;
            }

            target_camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance =
                Mathf.Lerp(target_camara.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance,
                cams_distance, Time.deltaTime * lock_on_hit_smoothness);
        }
        else
        {
            float distance = Vector3.Distance(ingame_mng.p_movement.cam_pivot.position, Camera.main.transform.position) - 0.5f;
            if (distance < 1 && c_timer <= 0.6f)
            {
                c_timer += Time.deltaTime * 2;
                VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = c_timer;
            }
            else if (distance >= 1 && c_timer > 0.1f)
            {
                c_timer -= Time.deltaTime * 2;
                VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = c_timer;
            }
        }
    }

    void toggle()
    {
        if (ingame_mng.p_movement.target_lock_on)
        {
            lock_on_cam.SetActive(true);
            
            target_camara.LookAt = nearest_lock_on_target;

        }
        else { lock_on_cam.SetActive(false); target_camara.LookAt = null; }
    }
    
    void mouse_input()
    {
        if ((ingame_mng.talk_mng.is_talk && count > 0) || (ingame_mng.is_pause) || (ingame_mng.upgrade_mng.is_upgrading) || ingame_mng.tutorial_mng.is_tutorial)
        {
            VirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
            VirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
            count = 0;
        }
        else if((!ingame_mng.talk_mng.is_talk && count <= 0))
        {
            VirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouse_sensitivity;
            VirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouse_sensitivity;
            count = 1;
        }

        
    }

    public void cams_lens_ef(float value)
    {
        volume.weight = value;
    }


    public void lock_on_view()
    {
        if (nearest_lock_on_target != null && ingame_mng.p_movement.target_lock_on)
        {   
            //캐릭터가 타겟 바라보기
            if (!ingame_mng.p_movement.run && !ingame_mng.p_movement._anime.GetBool("guard_break"))
            {
                if((ingame_mng.p_movement.rolling)) { return; }
                Vector3 p_dir = nearest_lock_on_target.transform.position - ingame_mng.p_movement.transform.position;
                Vector3 p_dir_real = new Vector3(p_dir.x, 0, p_dir.z);
                ingame_mng.p_movement.transform.rotation = Quaternion.Lerp(ingame_mng.p_movement.transform.rotation, Quaternion.LookRotation(p_dir_real), Time.deltaTime * 20);
            }
        }

    }

    public void handle_lock_on()
    {
        float shortest_distance = Mathf.Infinity;
        float shortest_distance_left_target = -Mathf.Infinity;
        float shortest_distance_right_target = Mathf.Infinity;

        Collider[] enemies = Physics.OverlapSphere(ingame_mng.p_movement.transform.position, lockOnDistance, enemyLayer);


        for (int i = 0; i < enemies.Length; i++)
        {
            enemy_manager en_obj = enemies[i].GetComponent<enemy_manager>(); //이거 중요 헷갈리지 말자

            if (en_obj != null)
            {
                Vector3 target_dir = en_obj.transform.position - transform.position;
                float distance_form_target = Vector3.Distance(transform.position, en_obj.transform.position);
                Vector3 dir = transform.position - VirtualCamera.Follow.transform.position;
                //float viewable_angle = VirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value;
                float viewable_angle = Vector3.Angle(target_dir, dir * -1); //-1이 앞을 의미

                RaycastHit hit;

                if ( viewable_angle > -50 && viewable_angle < 50 && distance_form_target <= max_lock_on_distance)
                {
                    //Debug.DrawLine(Camera.main.transform.position, en_obj.lock_on_transform.position);
                    if (Physics.Linecast(Camera.main.transform.position, en_obj.lock_on_transform.position, out hit, ground_layer))
                    {
                        //cant lock on
                        
                    }
                    else
                    {
                        available_targets.Add(en_obj);
                    }
                    
                }
            }
            else
            {
                ingame_mng.p_movement.target_lock_on = false;
                
            }
        }

        for (int j = 0; j < available_targets.Count; j++)
        {
            
            float distance_form_target = Vector3.Distance(transform.position, available_targets[j].transform.position);
            Vector3 dir_2 = transform.position - VirtualCamera.Follow.transform.position;
            Vector3 dir_1 = available_targets[j].transform.position - transform.position;
            float viewable_angle = Vector3.Angle(dir_1, dir_2 * -1);
            
            //가장 가까운 타겟
            if (viewable_angle > -20 && viewable_angle < 20 && distance_form_target < shortest_distance)
            {
                shortest_distance = distance_form_target;
                nearest_lock_on_target = available_targets[j].lock_on_transform;
                lock_on_img = available_targets[j].lock_on_img_transform;
            }
            if (ingame_mng.p_movement.target_lock_on)
            {
                Vector3 relative_en_pos = ingame_mng.p_movement.transform.InverseTransformPoint(available_targets[j].transform.position);
                var distance_from_left_target = relative_en_pos.x;
                var distance_from_right_target = relative_en_pos.x;

                if (relative_en_pos.x < 0 && available_targets[j] != nearest_lock_on_target &&
                    distance_from_left_target > shortest_distance_left_target)
                {
                    lock_on_left_target = available_targets[j].lock_on_transform;
                    lock_on_img_left = available_targets[j].lock_on_img_transform;
                }

                if (relative_en_pos.x > 0 && available_targets[j] != nearest_lock_on_target &&
                    distance_from_right_target < shortest_distance_right_target)
                {
                    lock_on_right_target = available_targets[j].lock_on_transform;
                    lock_on_img_right = available_targets[j].lock_on_img_transform;
                }
            }
        }
    }

    public void clear_lock_on()
    {
        available_targets.Clear();
        nearest_lock_on_target = null;
    }

    
}

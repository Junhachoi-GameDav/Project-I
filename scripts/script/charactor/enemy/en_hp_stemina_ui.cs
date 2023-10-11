using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class en_hp_stemina_ui : MonoBehaviour
{
    Transform player_cur_cam;
    Transform player_tar_cam;

    Vector3 dir1;
    Vector3 dir2;

    public GameObject enemy_hp_ui_group;
    public GameObject enemy_st_ui_group;

    public Slider hp_slider;
    public Slider st_slider;

    public Image hp_around;
    public Image st_around;
    public Image st;

    player_movement p_movement;
    cams_manager cams_mng;

    private void Start()
    {
        p_movement = FindObjectOfType<player_movement>();
        cams_mng =FindObjectOfType<cams_manager>();

        player_cur_cam = cams_mng.VirtualCamera.transform;
        player_tar_cam = cams_mng.target_camara.transform;
    }
    private void FixedUpdate()
    {
        dir1 = transform.position - player_cur_cam.position;
        dir2 = transform.position - player_tar_cam.position;

        Quaternion rotation = Quaternion.LookRotation(p_movement.target_lock_on ? dir2 : dir1);
        transform.rotation = rotation;
    }
    
    public void set_max_health(float max_health)
    {
        hp_slider.maxValue = max_health;
        hp_slider.value = max_health;
    }

    public void set_cur_health(float cur_health)
    {
        hp_slider.value = cur_health;
        if(hp_slider.value <= hp_slider.maxValue * 0.3f) { hp_around.color = Color.red; }
        else if(hp_slider.value <= hp_slider.maxValue * 0.6f) { hp_around.color = Color.yellow; }
        else {  hp_around.color = Color.white;}
    }

    public void set_max_stemina(float max_stemina)
    {
        st_slider.maxValue = max_stemina;
        st_slider.value = max_stemina;
    }
    public void set_cur_stemina(float cur_stemina)
    {
        st_slider.value = cur_stemina;
        if (st_slider.value >= st_slider.maxValue * 0.7f) { st_around.color = Color.red; }
        else if (st_slider.value >= st_slider.maxValue * 0.3f) { st_around.color = Color.yellow; }
        else { st_around.color = Color.white; }

        if(st_slider.value >= st_slider.maxValue) { st.color = Color.red; }
        else { st.color = Color.yellow;}
    }

    
}

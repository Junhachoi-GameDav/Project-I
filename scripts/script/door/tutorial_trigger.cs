using UnityEngine;
using UnityEngine.UI;

public class tutorial_trigger : MonoBehaviour
{
    public GameObject tutorial_window;

    public Button right_btn;
    public Button left_btn;

    public GameObject[] image_objs;
    //public BoxCollider boxCollider;
    public bool is_tutorial1_ckecked;
    public int num =0;


    ingame_manager ingame_mng;
    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            ingame_mng.tutorial_mng.is_tutorial = true;
            ingame_mng.tutorial_mng.time_controlling();
            image_objs[0].SetActive(true);
            game_manager.Instance.sound_mng.sm_ui_sound_play("tutorial_window");
            num++;
            left_btn.interactable = false;
            tutorial_window.SetActive(true);
            is_tutorial1_ckecked = true;
        }
    }

    public void next_page()
    {
        if(num ==  image_objs.Length) { return; }
        image_objs[num-1].SetActive(false);
        image_objs[num].SetActive(true);
        num++;
        check_last_page();
    }
    public void privous_page()
    {
        if (num <= 1) { return; }
        num--;
        image_objs[num].SetActive(false);
        image_objs[num-1].SetActive(true);
        check_last_page();
    }

    void check_last_page()
    {
        if(num == image_objs.Length)
        {
            right_btn.interactable = false;
            left_btn.interactable = true;
        }
        if(num ==1)
        {
            right_btn.interactable = true;
            left_btn.interactable = false;
        }
        else if(num != image_objs.Length)
        {
            right_btn.interactable = true;
            left_btn.interactable = true;
        }
    }
}

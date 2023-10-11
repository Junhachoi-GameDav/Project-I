
using UnityEngine;
using UnityEngine.UI;

public class lock_on_img_controller : MonoBehaviour
{
    //public Transform lust_lock_on_pos;

    public Image lock_on_img;

    cams_manager cams;

    float timer;
    float timer2;
    float num;

    private void Awake()
    {
        cams = FindObjectOfType<cams_manager>();
    }
    private void Start()
    {
        timer = 0.2f;
        num = 4;
    }

    void Update()
    {
        if(cams.nearest_lock_on_target == null)
        { 
            lock_on_img.enabled = false;
            timer = 0.2f;
            timer2 = 0;
            num = 4;
            lock_on_img.transform.localScale = new Vector3(num,num,1);
            return;
        }
        img_pos_ui_sys();
        show_up_img();
    }

    void img_pos_ui_sys()
    {
        lock_on_img.enabled = true;
        lock_on_img.rectTransform.position = Camera.main.WorldToScreenPoint(cams.lock_on_img.position);
    }

    void show_up_img()
    {
        if(timer > 0)
        {
            timer -=Time.deltaTime;
            timer2 += Time.deltaTime * 15f;
            lock_on_img.transform.localScale = new Vector3(num - timer2, num - timer2, 1);
        }
        else
        {
            lock_on_img.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

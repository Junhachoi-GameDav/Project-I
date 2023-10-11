
using UnityEngine;

public class ground_hit : MonoBehaviour
{
    GameObject obj_spark;
    ParticleSystem spark;
    //AudioSource ef_audio;
 
    private void Awake()
    {
        obj_spark = GameObject.Find("ground_hit_ef");
        spark = obj_spark.GetComponent<ParticleSystem>();
        //ef_audio = obj_spark.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            spark.transform.position = this.GetComponent<weapon>().ef_pos.position;
            spark.Play(); 
            //ef_audio.Play();
        }
    }
    
}

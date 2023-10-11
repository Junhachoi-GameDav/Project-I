using UnityEngine;

public class en_kyle_weapon : weapon
{
    //public BoxCollider colliders;
    public en_kyle _en_kyle;

    private void Awake()
    {
        _en_kyle = GetComponentInParent<en_kyle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block_collider"))
        {
            melee_collider.enabled = false;
        }
        else if (other.CompareTag("player"))
        {
            melee_collider.enabled = false;
        }
    }
}

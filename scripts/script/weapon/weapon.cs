using UnityEngine;

public class weapon : MonoBehaviour
{
    public enum type { melee, whip, axe, kick }
    public type _type;
    public float damage;
    public float rate;
    public TrailRenderer[] trail;
    public BoxCollider melee_collider;
    public Transform ef_pos;
}

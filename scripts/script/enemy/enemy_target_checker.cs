using UnityEngine;

public class enemy_target_checker : MonoBehaviour
{
    [Header("sphere Property")]
    [SerializeField] private float target_radius;
    
    [SerializeField] private LayerMask player_layer;

    [Header("ray Property")]
    [SerializeField] private float ray_distance;

    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, target_radius);
        Gizmos.DrawRay(transform.position, transform.forward * ray_distance);
    }

    public bool is_target_checked()
    {
        //RaycastHit ray_hit;
        Collider[] target = Physics.OverlapSphere(transform.position, target_radius, player_layer);
        
        if (target.Length > 0) { return true; } else { return false; }

        //return Physics.SphereCast(transform.position, target_radius, transform.forward, out ray_hit , 1, player_layer);
    }
    
    public bool is_grab_or_kick_skill_checked()
    {
        RaycastHit ray_hit;
        return Physics.Raycast(transform.position, transform.forward, out ray_hit, ray_distance, player_layer);
    }
}

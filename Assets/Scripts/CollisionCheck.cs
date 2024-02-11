using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Box Collider
        if (collision.collider is BoxCollider)
        {
            Debug.Log("Mesh Collider is colliding with an object with a Box Collider");
        }
    }
}

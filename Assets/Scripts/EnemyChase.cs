using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    // Speed at which the enemy chases the player
    public float speed = 3.0f;
    
    // Reference to the player's Transform
    private Transform player;

    void Start()
    {
        // Automatically find the player GameObject by tag.
        // Make sure the player GameObject has the "Player" tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    void Update()
    {
        // If the player exists, calculate the direction and move the enemy
        if (player != null)
        {
            // Calculate the normalized direction vector from enemy to player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move the enemy toward the player
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}

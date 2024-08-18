using UnityEngine;
using EZCameraShake;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public int size = 4;

    public GameManager gameManager;
    private void Start()
    {
        // Scale based on the size.
        transform.localScale = 0.5f * size * Vector3.one;

        // Add movement, bigger asteroids are slower.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);

        // Register creation
        gameManager.asteroidCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asteroids are only destroyed with bullets.
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            CameraShaker.Instance.ShakeOnce(1f, 1f, 1f, 1f);
            // Register the destruction with the game manager.
            
            
            gameManager.asteroidCount--;

            // Destroy the bullet so it doesn't carry on and hit more things.
            

            // If size > 1 spawn 2 smaller asteroids of size-1.
            if (size > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }

            // Spawn particles on destruction.
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            // Destroy this asteroid.

        }
    }
}
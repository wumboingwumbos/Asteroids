using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public GameObject player;
    public float speed;
    public GameManager gameManager;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
        
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet"))
        {

            // Get a reference to the GameManager
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            // Show the destroyed effect.
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            //add points
            ScoreManager.instance.AddPoint(1);

            // Destroy the ship.
            Destroy(gameObject);
        }
    }
}

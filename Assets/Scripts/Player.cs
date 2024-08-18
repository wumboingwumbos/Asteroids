using System.Collections;
using UnityEngine;
using EZCameraShake;
public class Player : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object references")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles;

    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    public Animator animator;
    public Animator pewanimator;
    public GameManager gameManager; 
    public ScoreManager scoreManager;
    private float mov;
    private float rot;
    public Joystick joystick;
    public int starCount;
    public bool collect = false;
    public GameObject star;
    private float acceleration;
    public int pews;

    [SerializeField] private AudioSource shootSoundEffect;
    [SerializeField] private AudioSource DeathSoundEffect;

    public void HandleShooting()
    {

        pewanimator.SetInteger("pew", 2);
        // Shooting.
        {
            shootSoundEffect.Play();
            Debug.Log("WJLKDSJ:GHJ:OHA:LSKJS;vLKJ:");
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Inherit velicity only in the forward direction of ship.
            Vector2 shipVelocity = shipRigidbody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

            // Don't want to inherit in the opposite direction, else we'll get stationary bullets.
            if (shipForwardSpeed < 0)
            {
                shipForwardSpeed = 0;
            }

            bullet.velocity = shipDirection * shipForwardSpeed;

            // Add force to propel bullet in direction the player is facing.
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }


    }
    
    private void Start()
    {
        // Get a reference to the attached RigidBody2D.
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            HandleShooting();
        }
        if (isAlive)
        {
            HandleShipMovement();
            HandleShipRotation();
        }
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        // Calculate the distance from the joystick's home position (magnitude of the input vector)
        float distanceFromHome = joystickInput.magnitude;
        // Calculate acceleration based on the distance from the home position
        acceleration = Mathf.Clamp01(distanceFromHome) * shipAcceleration;
        animator.SetFloat("Speed", distanceFromHome);
        

        




    }

    
    private void HandleShipRotation()
    {
        if (acceleration > 0)
        {
            // Get the horizontal and vertical input from the joystick
            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            // Calculate the angle in radians
            float angle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;

            // Create a rotation based on the angle
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

            // Rotate the ship towards the target rotation using Slerp for smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, shipRotationSpeed * Time.deltaTime);
            
        }
    }

    private void HandleShipMovement()
    {
        // Get the joystick input vector
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        // Calculate the distance from the joystick's home position (magnitude of the input vector)
        float distanceFromHome = joystickInput.magnitude;
        // Calculate acceleration based on the distance from the home position
        acceleration = Mathf.Clamp01(distanceFromHome) * shipAcceleration;
        if (acceleration > .2)
        {
            ;
            // Calculate the direction the ship is facing
            Vector2 shipDirection = transform.up;

            // Apply acceleration to the spaceship in the direction it's facing
            shipRigidbody.AddForce(shipDirection * acceleration);

            // Clamp velocity to the maximum velocity
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid") || collision.CompareTag("EnemyShip"))
        {
            
            CameraShaker.Instance.ShakeOnce(6f, 10f, .1f, 1f);
            isAlive = false;

            // Get a reference to the GameManager
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            // Restart game after delay.
            gameManager.GameOver();

            // Show the destroyed effect.
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            // Destroy the player.
            Destroy(gameObject);
        }
        if (collision.CompareTag("Star"))
        {
            DeathSoundEffect.Play();
            Destroy(collision.gameObject);
            gameManager.starCount++;
            Debug.Log(gameManager.starCount);
            ScoreManager.instance.AddPoint(1);
            collect = true;
        }
        if (collect == true)
        {
            Vector2 randomSpawnPosition = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));
            Instantiate(star, randomSpawnPosition, Quaternion.identity);
            collect = false;
        }
    }
}
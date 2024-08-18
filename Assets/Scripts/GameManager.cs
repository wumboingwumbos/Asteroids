using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Seeker seekerPrefab;
    public int asteroidCount = 0;
    public int starCount = 0;
    private int level = 0;
    public GameObject star;
    //public GameObject Player;
    public ScoreManager scoreManager;
    [SerializeField] private AudioSource DeathSoundEffect;
    private void Start()
    {
        for (int i = 0; i < 5; i++) 
        {
            Vector2 randomSpawnPosition = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));
            Instantiate(star, randomSpawnPosition, Quaternion.identity);
        }
        //Instantiate(Player);
        ScoreManager.instance.AddPoint(-10);

    }
private void Update()
    {
        // If there are no asteroids left, spawn more!
        if (asteroidCount == 0)
        {
            // Increase the level.
            level++;
            ScoreManager.instance.AddPoint(10);
            // Spawn the correct number for this level.
            // 1=>4, 2=>6, 3=>8, 4=>10 ...
            int numAsteroids = 2 + (level);
            for (int i = 0; i < numAsteroids; i++)
            {
                SpawnAsteroid();
            }
        }
    }

    private void SpawnAsteroid()
    {
        // How far along the edge.
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;

        // Which edge.
        int edge = Random.Range(0, 4);
        if (edge == 0)
        {
            viewportSpawnPosition = new Vector2(offset, 0);
        }
        else if (edge == 1)
        {
            viewportSpawnPosition = new Vector2(offset, 1);
        }
        else if (edge == 2)
        {
            viewportSpawnPosition = new Vector2(0, offset);
        }
        else if (edge == 3)
        {
            viewportSpawnPosition = new Vector2(1, offset);
        }

        // Create the asteroid.
        Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);
        Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
        asteroid.gameManager = this;
    }
    public void GameOver()
    {
        DeathSoundEffect.Play();
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        Debug.Log("Game Over");

        // Wait a bit before restarting.
        yield return new WaitForSeconds(3f);

        // Restart scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }
}
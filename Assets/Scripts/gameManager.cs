using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public Renderer backGround;
    public GameObject column;           // Reference to the column prefab.
    public List<GameObject> columns;    // List to store instantiated columns.
    public float speed = 2;             // Speed of the game objects.
    public GameObject rock1;            // Reference to the first rock prefab.
    public GameObject rock2;            // Reference to the second rock prefab.
    public List<GameObject> obstacles;  // List to store instantiated obstacles (rocks).
    public bool gameOver = false;       // Indicates whether the game is over.
    public bool start = false;          // Indicates whether the game has started.
    public GameObject mainMenu;         // Reference to the main menu GameObject.
    public GameObject gameOverMenu;     // Reference to the game over menu GameObject.
    private GameObject scoreObject;     // Reference to the score display GameObject.
    private int scoreValue = 100;       // Initial health value.
    private AudioSource audioPlayer;    // Reference to the audio player component.
    public AudioClip impact;            // AudioClip for collision impact sound.

    /// <summary>
    /// Called before the first frame update. Initializes game objects and settings.
    /// </summary>
    void Start()
    {
        // Instantiate columns
        for (int i = 0; i < 24; i++)
        {
            columns.Add(Instantiate(column, new Vector2(-11 + i, -3), Quaternion.identity));

        }

        // Instantiate initial obstacles (rocks)
        obstacles.Add(Instantiate(rock1, new Vector2(14, -2), Quaternion.identity));
        obstacles.Add(Instantiate(rock2, new Vector2(18, -2), Quaternion.identity));
        obstacles.Add(Instantiate(rock1, new Vector2(21, -2), Quaternion.identity));
        obstacles.Add(Instantiate(rock2, new Vector2(28, -2), Quaternion.identity));

        // Find and initialize score display
        scoreObject = GameObject.Find("score");
        scoreObject.GetComponent<TextMeshPro>().SetText("Health: {0}", scoreValue);

        // Instantiate audioPlayer
        audioPlayer = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called once per frame. Manages game state and updates game objects.
    /// </summary>
    void Update()
    {
        // Check for game start input
        if (start == false)
        {
            if (Input.GetKeyDown(KeyCode.S)) start = true;
        }

        // Handle game over state
        if (start == true && gameOver == true)
        {
            gameOverMenu.SetActive(true);
            if (Input.GetKeyDown(KeyCode.S)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Update game objects if the game is started and not over
        if (start == true && gameOver == false)
        {
            mainMenu.SetActive(false);
            backGround.material.mainTextureOffset += (new Vector2(0.06f, 0) * Time.deltaTime);

            // Update columns' positions
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].transform.position.x <= -12)
                    columns[i].transform.position = new Vector3(12, -3, 0);
                columns[i].transform.position += (new Vector3(-1, 0, 0) * Time.deltaTime * speed);

            }

            // Update obstacles' positions
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].transform.position.x <= -11)
                {
                    float randomRocks = Random.Range(11, 18);
                    obstacles[i].transform.position = new Vector3(randomRocks, -2, 0);
                }
                obstacles[i].transform.position += (new Vector3(-1, 0, 0) * Time.deltaTime * speed);
            }
        }
    }

    /// <summary>
    /// Handles collision with rocks, updates score, and plays impact sound.
    /// </summary>
    public void rockCollision()
    {
        scoreValue -= 10;
        if (scoreValue <= 0)
        {
            gameOver = true;
            scoreValue = 0;
        }
        scoreObject.GetComponent<TextMeshPro>().SetText("Health: {0}", scoreValue);
        audioPlayer.clip = impact;
        audioPlayer.Play();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public BoxSpawner boxSpawner;

    public CameraFollow mainCamera;
    public Text scoreText;
    public GameObject menu;
    public GameObject holder;

    private int score;
    private int boxCount;
    private AudioSource backgroudSoud;

    [HideInInspector]
    public Box currentBox;

    [HideInInspector]
    public List<Box> boxes;

    [HideInInspector]
    public bool isGameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        isGameOver = true;
        score = 0;
        boxCount = 0;
        backgroudSoud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isGameOver)
        {
            DetectInput();
        }
        else
        {
            backgroudSoud.Stop();
        }
    }

    void DetectInput()
    {
        if (Input.GetMouseButtonDown(0) && !isGameOver)
        {
            currentBox.DropBox();
        }
    }

    public void MoveCamera()
    {
        score++;
        scoreText.text = "Score: " + score;
        boxCount++;

        if (boxCount == 3)
        {
            boxCount = 0;
            mainCamera.targetPos.y += 2f;
        }
    }

    public void SpawnNewBox()
    {
        boxSpawner.Spawn();
    }

    public void StartGame()
    {
        scoreText.gameObject.SetActive(true);
        holder.SetActive(true);
        menu.SetActive(false);
        isGameOver = false;

        backgroudSoud.Play();

        SpawnNewBox();
    }

    private void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        Invoke("LoadCurrentScene", 3f);
    }
}

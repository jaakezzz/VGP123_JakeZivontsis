using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate PlayerController PlayerControllerDelegate(PlayerController playerInstance);
    public event PlayerControllerDelegate OnPlayerControllerCreated;

    public UnityEvent<int> OnLivesChanged;

    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    #endregion

    #region Player Controller Info
    [SerializeField] private PlayerController playerPrefab;
    private PlayerController playerInstance;
    public PlayerController PlayerInstance => playerInstance;
    private Transform currentCheckpoint;
    #endregion

    #region Game Stats
    private int lives = 3;
    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 0)
            {
                GameOver();
            }
            if (lives > value)
            {
                Respawn();
            }
            lives = value;
            OnLivesChanged?.Invoke(lives);
            Debug.Log("Lives: " + lives);
        }
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (SceneManager.GetActiveScene().name == "Title")
        //    {
        //        SceneManager.LoadScene("SampleScene");
        //        //Reset Game
        //        Lives = 3;
        //    }
        //    else
        //        SceneManager.LoadScene("Title");
        //}
    }

    private void Respawn()
    {
        Debug.Log("Respawn");
        playerInstance.transform.position = currentCheckpoint.position;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }

    public void InstantiatePlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentCheckpoint = spawnLocation;

        OnPlayerControllerCreated?.Invoke(playerInstance);
    }

    public void SetCheckpoint(Transform spawnLocation)
    {
        currentCheckpoint = spawnLocation;
    }
}

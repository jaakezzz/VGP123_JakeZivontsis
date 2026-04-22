using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate PlayerController PlayerControllerDelegate(PlayerController playerInstance);
    public event PlayerControllerDelegate OnPlayerControllerCreated;

    public UnityEvent<int> OnHealthChanged;

    public CanvasManager canvasManager;

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
    [SerializeField] private Transform currentCheckpoint;
    #endregion

    #region Game Stats
    private int playerHealth = 3;
    public int PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            int prevHealth = playerHealth;
            playerHealth = value;
            if (value <= 0)
            {
                Respawn();
            }
            if (playerHealth < prevHealth)
            {
                playerInstance.hurtSound();
            }
            OnHealthChanged?.Invoke(playerHealth);
            if (canvasManager != null)
                canvasManager.UpdateHearts(playerHealth);

            Debug.Log("Player health: " + playerHealth);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SceneManager.LoadScene("SampleScene");
                //Reset Game
                PlayerHealth = 3;
            }
            else
                SceneManager.LoadScene("Title");
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawn");
        playerInstance.DieAndRespawn(1.0f); // Play death anim, then finish respawn
    }

    public void FinishRespawn()
    {
        playerInstance.transform.position = currentCheckpoint.position;
        PlayerHealth = 3;
        foreach (Enemy enemy in Enemy.ActiveEnemies)
        {
            enemy.ResetEnemy();
        }
    }

    public void InstantiatePlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentCheckpoint = spawnLocation;

        if (canvasManager != null)
            playerInstance.SetCanvasManager(canvasManager);

        OnPlayerControllerCreated?.Invoke(playerInstance);
    }

    public void SetCheckpoint(Transform spawnLocation)
    {
        currentCheckpoint = spawnLocation;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            CanvasManager cm = null;
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                cm = go.GetComponentInChildren<CanvasManager>(true);
                if (cm != null) break;
            }

            if (cm != null)
            {
                canvasManager = cm;

                if (playerInstance != null)
                    playerInstance.SetCanvasManager(canvasManager);
            }
            else
            {
                Debug.LogWarning("CanvasManager not found in scene.");
            }
        }
    }
}

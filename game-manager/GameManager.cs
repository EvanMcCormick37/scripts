// GameManager.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_playerInstance = null;
    [SerializeField] private GameObject m_titleScreen = null;
    [SerializeField] private GameObject m_winScreen = null;
    [SerializeField] private bool loggingEnabled = false;
    [SerializeField] public bool restartLevelOnDeath = false;

    private int m_currentLevelIndex = 0;
    private Level m_currentLevel = null;
    private int m_total_levels;
    private bool m_isPlaying = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        m_total_levels = SceneManager.sceneCountInBuildSettings;
        if (loggingEnabled) Debug.Log("GameManager Awake");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called by Level.Bootstrap() when no real GameManager exists in the scene.
    // Finds the player, wires up the level, and starts immediately.
    public void InitializeForTesting(Level level)
    {
        restartLevelOnDeath = true;
        m_currentLevel = level;
        m_playerInstance = FindObjectOfType<KeyboardMovement>(true)?.gameObject;

        if (m_playerInstance == null)
            Debug.LogWarning("[GameManager] Bootstrap: No KeyboardMovement found — player death detection will not work.");

        GameStart();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_currentLevel = FindObjectOfType<Level>();
        m_playerInstance = FindObjectOfType<KeyboardMovement>(true)?.gameObject;

        if (loggingEnabled) Debug.Log($"[GameManager] Scene loaded: {scene.name} | Level: {m_currentLevel != null} | Player: {m_playerInstance != null}");

        if (m_currentLevel != null && m_playerInstance != null)
        {
            m_currentLevel.Initialize(this);
        }

        if (m_titleScreen != null) m_titleScreen.SetActive(true);
        if (m_winScreen != null) m_winScreen.SetActive(false);
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            if (m_currentLevel != null)
                m_currentLevel.UpdateLevel();

            if (m_playerInstance == null || !m_playerInstance.activeSelf)
                GameOver();
        }
        // Level-Switching Inputs Logic
        if (m_titleScreen.activeSelf && Keyboard.current.spaceKey.wasPressedThisFrame) GameStart();
        if (m_winScreen.activeSelf && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            m_winScreen.SetActive(false);
            NextLevel();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame) GoToLevel(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) GoToLevel(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) GoToLevel(2);
    }

    private void GameStart()
    {
        m_isPlaying = true;
        if (m_titleScreen != null) m_titleScreen.SetActive(false);
        if (m_winScreen != null) m_winScreen.SetActive(false);
        if (m_playerInstance != null) m_playerInstance.SetActive(true);

        if (m_currentLevel != null)
        {
            m_currentLevel.Initialize(this);
            m_currentLevel.StartLevel();
        }
    }

    public void OnWin(Level level)
    {
        m_isPlaying = false;

        if (m_winScreen != null)
        {
            m_winScreen.SetActive(true);
        }
    }

    public void EndRun()
    {
        m_isPlaying = false;
        if (m_playerInstance != null) m_playerInstance.SetActive(false);
        if (m_currentLevel != null) m_currentLevel.CleanUp();
        if (m_winScreen != null) m_winScreen.SetActive(false);
        if (m_titleScreen != null) m_titleScreen.SetActive(true);
    }

    public void GameOver()
    {
        m_isPlaying = false;
        if (m_playerInstance != null) m_playerInstance.SetActive(false);
        if (m_winScreen != null) m_winScreen.SetActive(false);
        if (m_titleScreen != null) m_titleScreen.SetActive(true);
        if (m_currentLevel != null) m_currentLevel.CleanUp();
        if (restartLevelOnDeath)
        {
            if (loggingEnabled) Debug.Log("[GameManager] Player died — restarting level.");
            GameStart();
        }
    }

    public bool IsPlaying() => m_isPlaying;

    private void GoToLevel(int i)
    {
        m_currentLevelIndex = i;
        SceneManager.LoadScene(i);
    }

    private void NextLevel() => GoToLevel(m_currentLevelIndex + 1);
}
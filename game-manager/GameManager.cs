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
    }

    private void Start()
    {
        if (loggingEnabled) Debug.Log("GameManager Started");
        if (m_titleScreen != null) m_titleScreen.SetActive(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_currentLevel = FindObjectOfType<Level>();
        m_playerInstance = FindObjectOfType<KeyboardMovement>(true);

        if (m_currentLevel != null && m_playerInstance != null)
        {
            m_currentLevel.Initialize(this);
            GameStart();
        }
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            if (m_currentLevel != null)
            {
                m_currentLevel.UpdateLevel();
            }
            // FIX 5: .active is deprecated, use .activeSelf
            if (m_playerInstance == null || !m_playerInstance.activeSelf)
            {
                GameOver();
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                GameStart();
            }
        }
        // TEMP: Remove before shipping
        // Press 1/2/3 to jump directly to that scene index
        if (Keyboard.current.digit1Key.wasPressedThisFrame) GoToLevel(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) GoToLevel(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) GoToLevel(2);

        if (loggingEnabled) Debug.Log($"Level Index: {m_currentLevelIndex}, Level: {m_currentLevel}, Player Instance: {m_playerInstance}, Playing: {m_isPlaying}");
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

    public void ShowWinScreen()
    {
        m_isPlaying = false;
        if (m_winScreen != null) m_winScreen.SetActive(true);
        StartCoroutine(WinSequenceDelay());
    }

    private IEnumerator WinSequenceDelay()
    {
        yield return new WaitForSeconds(5);

        if (m_currentLevelIndex + 1 < m_total_levels)
        {
            NextLevel();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        m_isPlaying = false;
        if (m_currentLevel != null) m_currentLevel.CleanUp();
        if (m_playerInstance != null) m_playerInstance.SetActive(false);
        if (m_winScreen != null) m_winScreen.SetActive(false);
        if (m_titleScreen != null) m_titleScreen.SetActive(true);
    }

    public bool IsPlaying()
    {
        return m_isPlaying;
    }

    private void GoToLevel(int i)
    {
        m_currentLevelIndex = i;
        SceneManager.LoadScene(i);
    }

    private void NextLevel()
    {
        GoToLevel(m_currentLevelIndex + 1);
    }
}
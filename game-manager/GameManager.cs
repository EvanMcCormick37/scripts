using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool m_scaleToFit = false;

    [SerializeField] private GameObject m_playerInstance = null;
    [SerializeField] private GameObject m_titleScreen = null;
    [SerializeField] private GameObject m_winScreen = null;

    [SerializeField] private bool loggingEnabled = false;

    // Drag your Arena or WaveEmitter component into this slot in the Inspector
    [SerializeField] private Level m_currentLevel = null;

    private bool m_isPlaying = false;

    private void Start()
    {
        if (m_currentLevel != null)
        {
            if (loggingEnabled) Debug.Log("GameManager Started");
            m_titleScreen.SetActive(true);
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
            if (m_playerInstance == null || !m_playerInstance.active)
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
    }

    private void GameStart()
    {
        m_isPlaying = true;
        if (m_titleScreen != null) m_titleScreen.SetActive(false);
        if (m_winScreen != null) m_winScreen.SetActive(false);

        if (m_playerInstance != null) m_playerInstance.SetActive(true);

        // Tell the level to begin its internal logic
        if (m_currentLevel != null) m_currentLevel.StartLevel();
        m_currentLevel.Initialize(this);
    }

    // Called by the Level when it decides the win condition is met
    public void ShowWinScreen()
    {
        m_isPlaying = false;
        if (m_winScreen != null) m_winScreen.SetActive(true);
        StartCoroutine(WinSequenceDelay());
    }

    private IEnumerator WinSequenceDelay()
    {
        yield return new WaitForSeconds(5);
        GameOver();
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
}
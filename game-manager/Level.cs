// Level.cs
using UnityEngine;

public abstract class Level : MonoBehaviour
{
    protected GameManager m_manager;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No GameManager found — bootstrapping test mode.");
            Bootstrap();
        }
    }

    private void Bootstrap()
    {
        GameObject managerObj = new GameObject("GameManager (Test Bootstrap)");
        GameManager manager = managerObj.AddComponent<GameManager>();

        Initialize(manager);
        manager.InitializeForTesting(this);
    }

    public virtual void Initialize(GameManager manager)
    {
        m_manager = manager;
    }

    public abstract void StartLevel();
    public abstract void UpdateLevel();
    public abstract void Win();
    public abstract void CleanUp();
}
using UnityEngine;

public abstract class Level : MonoBehaviour
{
    protected GameManager m_manager;

    // Called by GameManager to link itself to the level
    public virtual void Initialize(GameManager manager)
    {
        m_manager = manager;
    }

    // Forces child classes to implement these methods
    public abstract void StartLevel();
    public abstract void UpdateLevel();
    public abstract void Win();
    public abstract void CleanUp();
}
using UnityEngine;

public class WaveEmitter : Level
{
    [SerializeField] private GameObject[] m_waves = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private bool loggingEnabled = false;

    private int m_currentWaveIndex = 0;
    private GameObject m_activeWaveObject = null;
    private bool m_levelComplete = false;

    public override void StartLevel()
    {
        if (loggingEnabled)
        {
            Debug.Log($"Starting Level!");
        }
        m_currentWaveIndex = 0;
        m_levelComplete = false;

        if (m_waves != null && m_waves.Length > 0)
        {
            SpawnNextWave();
        }
        else
        {
            Win();
        }
    }

    public override void UpdateLevel()
    {
        if (loggingEnabled)
        {
            Debug.Log($"Updating Level!");
        }
        if (m_levelComplete) return;

        if (m_activeWaveObject != null && m_activeWaveObject.transform.childCount == 0)
        {
            Destroy(m_activeWaveObject);
            m_currentWaveIndex++;

            if (m_currentWaveIndex < m_waves.Length)
            {
                SpawnNextWave();
            }
            else
            {
                Win();
            }
        }
    }

    private void SpawnNextWave()
    {
        GameObject next_wave = m_waves[m_currentWaveIndex];
        Vector2 next_spawn = (spawnPoint != null) ? spawnPoint.position : next_wave.transform.position;
        m_activeWaveObject = Instantiate(next_wave, next_spawn, Quaternion.identity);
        if (loggingEnabled) Debug.Log($"Spawning Wave at {next_spawn}!");
    }

    public override void Win()
    {
        m_levelComplete = true;
        CleanUp();
        m_manager.OnWin(this);
    }

    public override void CleanUp()
    {
        if (m_activeWaveObject != null)
        {
            Destroy(m_activeWaveObject);
        }
        if (UbhObjectPool.instance != null) UbhObjectPool.instance.ReleaseAllBullet();
    }
}
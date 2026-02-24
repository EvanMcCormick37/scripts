using UnityEngine;
using System.Collections;

public class Arena : Level
{
    public int num_enemies = 3;
    public float arena_size = 10f;
    private bool m_bossSpawned = false;
    private bool m_waitingForBossSpawn = false;

    [SerializeField] private GameObject m_enemyPrefab = null;
    [SerializeField] private GameObject m_bossPrefab = null;
    [SerializeField] private GameObject m_bossMessage = null;

    public override void StartLevel()
    {
        m_bossSpawned = false;
        m_waitingForBossSpawn = false;
        SpawnEnemies();
    }

    public override void UpdateLevel()
    {
        if (m_bossSpawned)
        {
            CheckForGameWon();
        }
        else if (!m_waitingForBossSpawn)
        {
            CheckForBossSpawn();
        }
    }

    private void CheckForBossSpawn()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            m_waitingForBossSpawn = true;
            StartCoroutine(SpawnBoss());
        }
    }

    private void CheckForGameWon()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Win();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < num_enemies; i++)
        {
            Vector2 randPos = new Vector2(
                Random.Range(-arena_size, arena_size),
                Random.Range(-arena_size, arena_size));

            Instantiate(m_enemyPrefab, randPos, Quaternion.identity);
        }
    }

    private IEnumerator SpawnBoss()
    {
        if (UbhObjectPool.instance != null) UbhObjectPool.instance.ReleaseAllBullet();

        if (m_bossMessage != null) m_bossMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        if (m_bossMessage != null) m_bossMessage.SetActive(false);

        Vector2 randPos = new Vector2(
            Random.Range(-arena_size, arena_size),
            Random.Range(-arena_size, arena_size)
        );
        Instantiate(m_bossPrefab, randPos, Quaternion.identity);

        m_waitingForBossSpawn = false;
        m_bossSpawned = true;
    }

    public override void Win()
    {
        CleanUp();
        m_manager.ShowWinScreen(); // Pass control back to the UI manager
    }

    public override void CleanUp()
    {
        StopAllCoroutines();
        if (UbhObjectPool.instance != null) UbhObjectPool.instance.ReleaseAllBullet();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
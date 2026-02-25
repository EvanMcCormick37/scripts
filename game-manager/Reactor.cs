using UnityEngine;

public class ReactorCore : Level
{
    public GameObject core;
    public GameObject player;
    public Transform playerSpawn;

    public override void StartLevel()
    {
        player.transform.position = playerSpawn.position;
        player.SetActive(true);
        Instantiate(core, Vector2.zero, Quaternion.identity);
        Debug.Log("Core Instantiated!");

    }

    public override void UpdateLevel()
    {
        if (core == null)
        {
            Win();
        }
    }

    public override void Win()
    {
        CleanUp();
        m_manager.ShowWinScreen();
    }
    public override void CleanUp()
    {
        if (player != null) player.SetActive(false);
        StopAllCoroutines();
        if (UbhObjectPool.instance != null) UbhObjectPool.instance.ReleaseAllBullet();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
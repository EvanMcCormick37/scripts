using UnityEngine;

public class ReactorCore : Level
{
    public GameObject core;
    public GameObject player;
    public Transform playerSpawn;

    public override void StartLevel()
    {
        player.transform.position = playerSpawn.position;
        core.transform.position = Vector2.zero;
        player.SetActive(true);
    }

    public override void UpdateLevel()
    {
    }

    public override void Win()
    {
        CleanUp();
        m_manager.OnWin(this);
    }
    public override void CleanUp()
    {
        if (player != null) player.SetActive(false);
        StopAllCoroutines();
        if (UbhObjectPool.instance != null) UbhObjectPool.instance.ReleaseAllBullet();
    }
}
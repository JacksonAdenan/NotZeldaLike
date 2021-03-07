using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [HideInInspector]
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    [HideInInspector]
    public List<GameObject> pickupSpawners = new List<GameObject>();
    [Header("Scene")]
    public List<GameObject> enemySpawnersList = new List<GameObject>();
    public enum RoomStates { Inactive, Active, Cleared }
    [Header("Runtime")]
    public RoomStates roomStatesToggle = RoomStates.Inactive;
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public GameObject layout;
    bool activeFirstTimeRunning;
    bool clearedFirstTimeRunning;

    void Start()
    {
        foreach (GameObject enemySpawner in enemySpawnersList)
            enemySpawners.Add(enemySpawner.GetComponent<EnemySpawner>());

        RollEnemies();
    }

    void Update()
    {
        switch (roomStatesToggle)
        {
            case RoomStates.Inactive:
                break;
            case RoomStates.Active:
                if (activeFirstTimeRunning == false)
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                        Instantiate(enemySpawner.rolledEnemy, enemySpawner.transform);

                //Add room to minimap
                foreach (GameObject enemy in enemies)
                    roomStatesToggle = RoomStates.Cleared;
                break;
            case RoomStates.Cleared:
                if (clearedFirstTimeRunning == false)
                {
                    //Set room minimap to green.
                    //Roll Pickup.
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (roomStatesToggle == RoomStates.Inactive)
            roomStatesToggle = RoomStates.Active;
        //Camera
    }

    private void OnTriggerExit(Collider other)
    {
        if (roomStatesToggle == RoomStates.Active)
            roomStatesToggle = RoomStates.Inactive;

        foreach (GameObject enemy in enemies)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    void RollEnemies()
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            int roll = Random.Range(0, (enemySpawner.enemies.Count - 1));
            enemySpawner.rolledEnemy = enemySpawner.enemies[roll];
        }
    }
}

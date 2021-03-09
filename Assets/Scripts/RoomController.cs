using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    [HideInInspector]
    public List<GameObject> pickupSpawners = new List<GameObject>();
    public enum RoomStates { Inactive, Active, Cleared }
    [Header("Runtime")]
    public RoomStates roomStatesToggle = RoomStates.Inactive;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject layout;
    public GameObject outline;
    public GameObject enemySpawnerParent;
    public GameObject mapColourObject;
    bool activeFirstTimeRunning;
    bool activeFirstTimeRunningButTheOneThatResetsSometimes;
    bool clearedFirstTimeRunning;

    void Start()
    {
        foreach (Transform enemySpawnersObjects in layout.transform.GetChild(1).transform)
        {
            enemySpawners.Add(enemySpawnersObjects.GetComponent<EnemySpawner>());
        }

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
                {
                    mapColourObject.SetActive(false);
                    activeFirstTimeRunning = true;
                }
                if (activeFirstTimeRunningButTheOneThatResetsSometimes == false)
                {
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                        enemies.Add(Instantiate(enemySpawner.rolledEnemy, enemySpawner.transform));
                    activeFirstTimeRunningButTheOneThatResetsSometimes = true;
                }
                int count = 0;
                foreach (GameObject enemy in enemies)
                    if (enemy.activeSelf)
                        count++;
                if (count == 0)
                {
                    foreach (GameObject enemy in enemies)
                        Destroy(enemy);
                    roomStatesToggle = RoomStates.Cleared;
                }
                count = 0;
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
        if (other.CompareTag("Player"))
        {
            if (roomStatesToggle == RoomStates.Inactive)
                roomStatesToggle = RoomStates.Active;

            Time.timeScale = 1.0f;

            //Camera
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (roomStatesToggle == RoomStates.Active)
                roomStatesToggle = RoomStates.Inactive;
            activeFirstTimeRunningButTheOneThatResetsSometimes = false;
            foreach (GameObject enemy in enemies)
            {
                enemies.Remove(enemy);
                Destroy(enemy);
            }

            Time.timeScale = 0.0f;
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

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

    // Lock and key system stuff:
    public bool isKeyRoom = false;
    private GameObject keySpawner;
    GameManager gameManager;
    [HideInInspector]
    public bool isExit = false;
    public GameObject gate;

    void Start()
    {
        foreach (Transform enemySpawnersObjects in layout.transform.GetChild(1).transform)
        {
            enemySpawners.Add(enemySpawnersObjects.GetComponent<EnemySpawner>());
        }

        RollEnemies();

        // Getting key spawner.
        //keySpawner = layout.transform.Find("KeySpawner").gameObject;
        gameManager = GameManager.GetInstance();

        //ActivateKey();
        GenerateLock();
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

    void ActivateKey()
    {
        if (isKeyRoom && gameManager.keyRequired)
        {
            keySpawner.SetActive(true);
        }
    }

    void GenerateLock()
    {
        if (gameManager.keyRequired && isExit)
        {
            GameObject gateObj = Instantiate(gate);
            GameObject triggers = outline.transform.Find("Triggers").gameObject;
            GameObject transitionTrigger = triggers.transform.GetChild(0).gameObject;
            gateObj.transform.parent = transitionTrigger.transform;
            gateObj.transform.localPosition = Vector3.zero;
            gateObj.transform.localRotation = Quaternion.identity;
        }
    }
}

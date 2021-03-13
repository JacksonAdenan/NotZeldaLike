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

    public Room roomReference;
    // Lock and key system stuff:
    public bool isKeyRoom = false;
    private GameObject keySpawner;
    GameManager gameManager;
    //[HideInInspector]
    public bool isExit = false;
    //public GameObject gate;

    public bool isCurrentRoom = false;

    void Start()
    {
        gameManager = GameManager.GetInstance();


        foreach (Transform enemySpawnersObjects in layout.transform.GetChild(1).transform)
        {
            enemySpawners.Add(enemySpawnersObjects.GetComponent<EnemySpawner>());
        }

        RollEnemies();

        // Getting key spawner. We make sure to only try set it's active state if we actually found it.
        if (layout.transform.Find("Key"))
        {
            keySpawner = layout.transform.Find("Key").gameObject;
            ActivateKey();
        }




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
                    gameManager.amountClearedRooms++;
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

        // Spawn the monster key in the room we're in if we've cleared everything.
        if (isCurrentRoom && gameManager.isMonsterKeyReady)
        {
            SpawnMonsterKey();
            gameManager.isMonsterKeyReady = false;
            gameManager.isMonsterKeySpawned = true;
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

            // Setting it to the "current room"
            isCurrentRoom = true;
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

            // Removing "current room" from the room we're leaving.
            isCurrentRoom = false;
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
        // Making all key spawns false if they're not the key room.
        keySpawner.SetActive(false);
        if (isKeyRoom && gameManager.keyRequired)
        {
            keySpawner.SetActive(true);
        }
    }

    void GenerateLock()
    {
        if (gameManager.keyRequired && isExit)
        {
            switch (roomReference.pattern)
            {
                case RoomPattern.Up:
                    {
                        GameObject gateObj = Instantiate(Resources.Load<GameObject>("Rooms/Gate/Gate (Up)"));
                        gateObj.transform.parent = this.transform;

                        gateObj.transform.localPosition = new Vector3(0, 0, -6.3f);
                        gateObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case RoomPattern.Down:
                    {
                        GameObject gateObj = Instantiate(Resources.Load<GameObject>("Rooms/Gate/Gate (Down)"));
                        gateObj.transform.parent = this.transform;

                        gateObj.transform.localPosition = new Vector3(0, 0, 6.3f);
                        gateObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case RoomPattern.Left:
                    {
                        GameObject gateObj = Instantiate(Resources.Load<GameObject>("Rooms/Gate/Gate (Left)"));
                        gateObj.transform.parent = this.transform;

                        gateObj.transform.localPosition = new Vector3(8, 0, 0);
                        gateObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case RoomPattern.Right:
                    {
                        GameObject gateObj = Instantiate(Resources.Load<GameObject>("Rooms/Gate/Gate (Right)"));
                        gateObj.transform.parent = this.transform;

                        gateObj.transform.localPosition = new Vector3(-8, 0, 0);
                        gateObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
            }

        }

    }

    void SpawnMonsterKey()
    {
        GameObject monsterKey = Instantiate(gameManager.monsterKey);
        monsterKey.transform.parent = keySpawner.transform.parent;
        monsterKey.transform.localPosition = new Vector3(keySpawner.transform.localPosition.x + 0.5f, keySpawner.transform.localPosition.y, keySpawner.transform.localPosition.z);
    }

}

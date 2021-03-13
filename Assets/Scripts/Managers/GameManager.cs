using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (GameManager.instance != null)
        {
            return GameManager.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on GameManager, but GameManager has no instance.");
            return null;
        }

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created GameManager multiple times!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            SpawnPlayer();
            endRoomLayouts = Resources.LoadAll<GameObject>("Rooms/EndLayouts");

            // Getting all the cool rooms.
            LoadCoolRooms();
            
        }


    }

    private bool hasResetPlayer = false;

    public int numberOfLayouts;

    public float cameraZAxis = -3;
    public float cameraYAxis = 10;
    bool transitionReloadLock = false;

    TransitionManager transitionManager;
    public GameObject transitionManagerPrefab;

    public int levelCount = 1;
    public int zoneCount = 1;
    public TextMeshProUGUI levelUI;

    // We have pointers to these rooms so that we can use them to spawn the player.
    //[HideInInspector]
    public GameObject currentEntranceRoom;
    //[HideInInspector]
    public GameObject currentExitRoom;


    // Replace this with loading it in from the resources folder.
    public GameObject player;

    private PlayerManager playerManager;
    private GameObject playerInstance;

    private NavMeshBaker navmeshBaker;

    public GameObject healthUIParent;
    public GameObject armourUIParent;


    // Timer stuff.
    //public GameObject timerUI;
    public TextMeshProUGUI timerTextUI;
    [Tooltip("Time in seconds")]
    public float timePerLevel = 60;
    private float timeLeft = 0;
    private bool isGameOver = false;


    [HideInInspector]
    public GameObject[] endRoomLayouts;

    // Lock and key system stuff.
    public bool keyRequired = false;
    public bool hasKey = false;

    // Loading in cool rooms.
    [HideInInspector]
    public List<GameObject> coolRooms;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnPlayer();
        SetPlayerPos();
        SpawnCamera();

        playerManager = PlayerManager.GetInstance();
        playerInstance = playerManager.player;

        navmeshBaker = NavMeshBaker.GetInstance();

        transitionManager = Instantiate(transitionManagerPrefab).GetComponent<TransitionManager>();
        levelUI.text = "Level:" + zoneCount.ToString() + "-" + levelCount.ToString();

        timeLeft = timePerLevel;
    }

    // Update is called once per frame
    void Update()
    {
        // Put SpawnCamera() here to check the camera y axis and z axis in real time. Useful for positioning it just right.
        //SpawnCamera();


        if (Input.GetKeyDown(KeyCode.X))
            LoadNewLevel();

        GameTimer();
        DisplayTime();
    }

	private void FixedUpdate()
	{

        // I think resetting the player pos has to be here because when we reload the scene we have to wait for DungeonGenerators Awake() function to be called before we re position the player.
        if (!hasResetPlayer && currentEntranceRoom != null)
        {
            Debug.Log("!NOTE!========= NAV MESH REBAKED ==========!NOTE!");
            transitionManager = Instantiate(transitionManagerPrefab).GetComponent<TransitionManager>();
            ResetPlayer();
            navmeshBaker.ResetBaker();
        }
    }

	public void SpawnPlayer()
    {
        player = Instantiate(player);
        //player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);

        //SpawnCamera();

    }
    
    // SetPlayerPos() is used once at the start of the game. ResetPlayerPos() is used on level reload.
    public void SetPlayerPos()
    {
        player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);
    }
    public void ResetPlayerPos()
    {

        // WARNING --------- Have no idea but when the player is not kinematic and gets stuck inside a thing on spawn, you can change its position like this. So my temporary solution
        // is just to make him kinematic for a split second then change him back.

        playerManager.playerRigidbody.isKinematic = true;
        playerInstance.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);
        playerManager.playerRigidbody.isKinematic = false;
    }

    public void SpawnCamera()
    {
        Camera theCamera = Camera.main;

        theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    }


    public void LoadNewLevel()
    {
        if (transitionReloadLock == false)
        {
            transitionReloadLock = true;
            transitionManager.Transition();
            StartCoroutine(PlayWait(1.6f));
        }  
    }

    IEnumerator PlayWait(float time)
    {
        yield return new WaitForSeconds(time);
        if (levelCount == 5)
        {
            zoneCount++;
            levelCount = 1;
        }
        else
            levelCount++;
        levelUI.text = "Level: " + zoneCount.ToString() + "-" + levelCount.ToString();
        SceneManager.LoadScene("DanielsTestScene");
        transitionReloadLock = false;
        hasResetPlayer = false;
        currentEntranceRoom = null;

        ResetTimer();
    }

    public void ResetPlayer()
    {
        
        ResetPlayerPos();
        SpawnCamera();
        hasResetPlayer = true;
        hasKey = false;
    }

    public void GameTimer()
    {
        if (!isGameOver)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                isGameOver = true;
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game is over!");
    }

    private void DisplayTime()
    {

        if (isGameOver)
        {
            timerTextUI.text = "TIME: " + "00" + ":" + "00";
            return;
        }

        int min = Mathf.FloorToInt(timeLeft / 60);
        int sec = Mathf.FloorToInt(timeLeft % 60);

        timerTextUI.text = "TIME: " + min.ToString("00" + ":" + sec.ToString("00"));

    }

    private void ResetTimer()
    {
        timeLeft = timePerLevel;
    }

    private void LoadCoolRooms()
    {

        GameObject[] allRooms = Resources.LoadAll<GameObject>("Rooms/Layouts");

        for (int i = 0; i < allRooms.Length; i++)
        {
            LayoutData layout = allRooms[i].GetComponent<LayoutData>();
            if (!layout.CompatibleWithEverything)
            {
                coolRooms.Add(allRooms[i]);
            }
        }

      
    }
}

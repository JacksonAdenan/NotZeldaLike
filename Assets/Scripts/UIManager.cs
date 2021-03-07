using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    public static UIManager GetInstance()
    {
        if (UIManager.instance != null)
        {
            return UIManager.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on UIManager, but UIManager has no instance.");
            return null;
        }

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created UIManager multiple times!");
        }
        else
        {
            instance = this;
        }
    }


    public Canvas gui;
    public GameObject healthIcon;
    public GameObject armourIcon;


    private PlayerManager playerManager;

    private List<GameObject> healthIcons;
    

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.GetInstance();
        healthIcons = new List<GameObject>();

        //Hiding the health and armour sprites that are visible in editor.
        healthIcon.SetActive(false);
        armourIcon.SetActive(false);

        for (int i = 0; i < playerManager.health; i++)
        {
            AddIcon(IconType.HealthIcon, i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // (healthIcons.Count < playerManager.health)
        //
        //  AddIcon(IconType.HealthIcon);
        //

        
    }

    public enum IconType
    { 
        HealthIcon,
        ArmourIcon
    }
    public void AddIcon(IconType type, int iconNumber)
    {
        if (type == IconType.HealthIcon)
        {
            GameObject newHealth = Instantiate(healthIcon);
            newHealth.SetActive(true);
            //newHealth.transform.position = new Vector3(healthIcon.transform.position.x, healthIcon.transform.position.y, healthIcon.transform.position.z);
            newHealth.transform.parent = gui.transform;

            newHealth.transform.localScale = Vector3.one;
            newHealth.transform.position = new Vector3(20 + (iconNumber * 60), healthIcon.transform.position.y, healthIcon.transform.position.z);

            healthIcons.Add(newHealth);
            Debug.Log("Spawned health icon.");
        }
    }

    public void RemoveIcon(IconType type)
    {
        if (type == IconType.HealthIcon)
        {
            if (healthIcons.Count > 0)
            { 
                GameObject healthIcon = healthIcons[healthIcons.Count - 1];
                healthIcons.Remove(healthIcons[healthIcons.Count - 1]);
                Destroy(healthIcon);
                Debug.Log("Spawned health icon.");
            }
        }
    }
}

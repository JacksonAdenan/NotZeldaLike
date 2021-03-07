using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

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
    }

    // Update is called once per frame
    void Update()
    {
        if (healthIcons.Count < playerManager.health)
        {
            AddIcon(IconType.HealthIcon);
        }
    }

    enum IconType
    { 
        HealthIcon,
        ArmourIcon
    }
    void AddIcon(IconType type)
    {
        if (type == IconType.HealthIcon)
        {
            GameObject newHealth = Instantiate(healthIcon);
            //newHealth.transform.position = new Vector3(healthIcon.transform.position.x, healthIcon.transform.position.y, healthIcon.transform.position.z);
            newHealth.transform.parent = gui.transform;

            newHealth.transform.localScale = Vector3.one;
            newHealth.transform.localPosition = new Vector3(healthIcon.transform.position.x + playerManager.health * 50, healthIcon.transform.position.y, healthIcon.transform.position.z);

            healthIcons.Add(newHealth);
            Debug.Log("Spawned health icon.");
        }
    }
}

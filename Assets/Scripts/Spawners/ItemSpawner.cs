using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Health,
    Armour
}
public class ItemSpawner : MonoBehaviour
{
    public ItemType spawnerType;

    public GameObject health;
    public GameObject armour;

    // Start is called before the first frame update
    void Start()
    {
        SpawnItem(spawnerType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Health:
                GameObject newHealth = Instantiate(health);
                newHealth.transform.position = this.transform.position;
                break;
            case ItemType.Armour:
                GameObject newArmour = Instantiate(armour);
                newArmour.transform.position = this.transform.position;
                break;
        }
    }
}

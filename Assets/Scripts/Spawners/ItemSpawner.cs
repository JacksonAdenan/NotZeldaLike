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
    public bool health;
    public bool armour;

    public GameObject healthObj;
    public GameObject armourObj;

    private List<ItemType> listOfItemsTicked;
    private ItemType typeSelected;

    // Start is called before the first frame update
    void Start()
    {
        listOfItemsTicked = new List<ItemType>();
        AddSelectedItems();
        PickType();
        SpawnItem(typeSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddSelectedItems()
    {
        if (health)
            listOfItemsTicked.Add(ItemType.Health);
        if (armour)
            listOfItemsTicked.Add(ItemType.Armour);
    }
    void PickType()
    {
        int randomNum = Random.Range(0, listOfItemsTicked.Count);
        typeSelected = listOfItemsTicked[randomNum];
    }
    public void SpawnItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Health:
                GameObject newHealth = Instantiate(healthObj);
                newHealth.transform.position = this.transform.position;
                break;
            case ItemType.Armour:
                GameObject newArmour = Instantiate(armourObj);
                newArmour.transform.position = this.transform.position;
                break;
        }
    }
}

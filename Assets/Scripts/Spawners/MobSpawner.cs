using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// These mob types are just place holders until we discuss what monsters we actually want.
public enum MobType
{ 
    Goblin,
    Ranged_Goblin
}
public class MobSpawner : MonoBehaviour
{

    public MobType spawnerType;

    // For now ill keep prefabs here but replace it by loading in resources.
    public GameObject goblin;
    public GameObject ranged_Goblin;


    // Start is called before the first frame update
    void Start()
    {
        SpawnMob(spawnerType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Replace this code with resources.load stuff.
    public void SpawnMob(MobType type)
    {
        switch (type)
        {
            case MobType.Goblin:
                GameObject newGoblin = Instantiate(goblin);
                newGoblin.transform.position = this.gameObject.transform.position;
                break;
            case MobType.Ranged_Goblin:
                GameObject newRangedGoblin = Instantiate(ranged_Goblin);
                newRangedGoblin.transform.position = this.gameObject.transform.position;
                break;
        }
    }
}

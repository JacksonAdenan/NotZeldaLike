using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// These mob types are just place holders until we discuss what monsters we actually want.
public enum MobType
{ 
    Skeleton,
    Zombie,
    Thromp
}
public class MobSpawner : MonoBehaviour
{

    public MobType spawnerType;

    // For now ill keep prefabs here but replace it by loading in resources.
    public GameObject skeleton;
    public GameObject zombie;


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
            case MobType.Skeleton:
                GameObject newGoblin = Instantiate(skeleton);
                newGoblin.transform.position = this.gameObject.transform.position;
                break;
            case MobType.Zombie:
                GameObject newRangedGoblin = Instantiate(zombie);
                newRangedGoblin.transform.position = this.gameObject.transform.position;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager GetInstance()
    {
        if (PlayerManager.instance != null)
        {
            return PlayerManager.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on PlayerManager, but PlayerManager has no instance.");
            return null;
        }

    }
	private void Awake()
	{
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created PlayerManager multiple times!");
        }
        else
        {
            instance = this;
        }    
	}


	public float health;
    public int armour;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitPlayer(float damageAmount)
    {
        // The player will lose 1 peice of armour if they have it, if the player has no armour the damage goes to their health.
        if (armour > 0)
        {
            armour -= 1;
        }
        else
        { 
            health -= damageAmount;
        }
    }
    public void AddArmour(int armourAmount)
    {
        armour += armourAmount;
    }
    public void AddHealth(float healthAmount)
    {
        health += healthAmount;
    }
}

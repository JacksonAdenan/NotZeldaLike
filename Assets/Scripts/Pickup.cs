using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PickupType
{ 
    HEALTH,
    ARMOUR,
    MAX_HEALTH,
    MAX_ARMOUR,
    MOVEMENT_SPEED,
    KNOCK_BACK,
    DAMAGE
}
public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int amount;


    private Transform itemsTransform;
    public float rotationSpeed = 0.05f;
    public float bobbingSpeed = 0.1f;
    public float bobbingHeight = 0.5f;

    public float originalY = 0.4f;
    //private float originalY;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        itemsTransform = this.gameObject.transform;
        //originalY = itemsTransform.position.y;
        playerManager = PlayerManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        itemsTransform.Rotate(new Vector3(0, 0.1f, 0));

        float newY = (Mathf.Sin(Time.time * bobbingSpeed)) * bobbingHeight + originalY;

        itemsTransform.position = new Vector3(itemsTransform.position.x, newY, itemsTransform.position.z);
    }

    void DoPickUp()
    {
        switch (type)
        {
            case PickupType.HEALTH:
                playerManager.AddHealth(amount);
                break;
            case PickupType.ARMOUR:
                playerManager.AddArmour(amount);
                break;
            case PickupType.MAX_ARMOUR:
                playerManager.AddMaxArmour(amount);
                break;
            case PickupType.MAX_HEALTH:
                playerManager.AddMaxHealth(amount);
                break;
            case PickupType.MOVEMENT_SPEED:
                playerManager.AddMovementSpeed(amount);
                break;
            case PickupType.KNOCK_BACK:
                playerManager.AddKnockback(amount);
                break;
            case PickupType.DAMAGE:
                playerManager.AddDamage(amount);
                break;
        }
    }

	public void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        { 
            DoPickUp();
            Debug.Log("Picked up " + type.ToString());
            Destroy(this.gameObject);
        }
	}
}

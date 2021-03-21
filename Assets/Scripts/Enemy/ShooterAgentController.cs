using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAgentController : BaseAgentController
{

	// Shooter's don't have a melee zone
	public override BoxCollider FindMeleeZone()
	{
		return null;
	}


	public override void Shoot(Vector3 target)
	{
	    GameObject newBullet = Instantiate(ammunition);
	
	    // Giving the bullet a reference to the controller it came from and the controllers collider.
	    BulletData thisData = newBullet.GetComponent<BulletData>();
	    thisData.agent = this;
	    thisData.agentsCollider = this.gameObject.GetComponent<Collider>();
	
	    newBullet.transform.position = this.transform.position;
	    Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
	    Vector3 shotDirection = target - newBullet.transform.position;
	    shotDirection = Vector3.Normalize(shotDirection);
	    shotDirection *= bulletSpeed;
	    bulletRigidbody.AddForce(shotDirection, ForceMode.Impulse);
	}
}

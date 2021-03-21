using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAgentController : BaseAgentController
{
    public override BoxCollider FindMeleeZone()
    {
        BoxCollider collider = this.gameObject.transform.Find("MeleeZone").GetComponent<BoxCollider>();
        return collider;
    }

	public override void Shoot(Vector3 target)
	{
		throw new System.NotImplementedException();
	}
}

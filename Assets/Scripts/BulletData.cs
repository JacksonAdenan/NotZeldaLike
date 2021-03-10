using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    [HideInInspector]
    public AgentController agent;
    [HideInInspector]
    public Collider agentsCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.collider != agentsCollider)
        {
            Destroy(this.gameObject);
        }
	}
}

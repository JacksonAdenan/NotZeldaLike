using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy()
	{
        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = this.transform.position;
	}
}

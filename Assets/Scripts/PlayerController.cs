using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float xTranslation = Input.GetAxis("Horizontal");
        float yTranslation = Input.GetAxis("Vertical");
        if (xTranslation != 0 || yTranslation != 0)
        {
            // We put yTranslation in the z value because in our game, y means going into the sky.
            Vector3 movement = new Vector3(xTranslation, 0, yTranslation).normalized;
            //movement = Vector3.Normalize(movement);
            player.position += movement * speed * Time.deltaTime;
        }
    }
}

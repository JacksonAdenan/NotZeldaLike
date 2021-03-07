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

        LookAtMouse();
    }

    void LookAtMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }


}

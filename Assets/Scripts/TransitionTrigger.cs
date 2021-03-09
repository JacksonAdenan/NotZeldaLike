using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    Vector3 playerLocation;
    Vector3 cameraLocation;
    Camera camera;
    public enum Direction { Up, Down, Left, Right }
    public Direction directionToggle;

    private void Start()
    {
        camera = Camera.main;
        switch (directionToggle)
        {
            case Direction.Up:
                playerLocation = new Vector3(0, 0, -2);
                cameraLocation = new Vector3(0, 0, -12);
                break;
            case Direction.Down:
                playerLocation = new Vector3(0, 0, 2);
                cameraLocation = new Vector3(0, 0, 12);
                break;
            case Direction.Left:
                playerLocation = new Vector3(2, 0, 0);
                cameraLocation = new Vector3(14, 0, 0);
                break;
            case Direction.Right:
                playerLocation = new Vector3(-2, 0, 0);
                cameraLocation = new Vector3(-14, 0, 0);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Transition Trigger ERFGEFEFE");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Transition Trigger");
            other.transform.position = transform.position + playerLocation;
            camera.transform.position += cameraLocation;
        }
    }
}

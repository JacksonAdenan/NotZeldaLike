using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    Vector3 location;
    public enum Direction { Up, Down, Left, Right }
    public Direction directionToggle;

    private void Start()
    {
        switch (directionToggle)
        {
            case Direction.Up:
                location = new Vector3(0, 0, 2);
                break;
            case Direction.Down:
                location = new Vector3(0, 0, -2);
                break;
            case Direction.Left:
                location = new Vector3(-2, 0, 0);
                break;
            case Direction.Right:
                location = new Vector3(2, 0, 0);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Transition Trigger");
            other.transform.position = transform.position + location;
        }
    }
}

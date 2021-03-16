using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Animator bottomUp;
    public Animator topDown;
    public Color transitionColour;

    void Start()
    {
        bottomUp.GetComponent<Image>().color = transitionColour;
        topDown.GetComponent<Image>().color = transitionColour;
    }

    public void TransitionColorChange(Color color)
    {
        bottomUp.GetComponent<Image>().color = color;
        topDown.GetComponent<Image>().color = color;
    }
    public void Transition(float speed = 1)
    {
        bottomUp.speed = speed;
        topDown.speed = speed;
        bottomUp.SetBool("transition", true);
        topDown.SetBool("transition", true);
    }
}

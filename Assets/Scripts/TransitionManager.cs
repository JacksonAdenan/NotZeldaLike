using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public Animator bottomUp;
    public Animator topDown;

    public void Transition()
    {
        bottomUp.SetBool("transition", true);
        topDown.SetBool("transition", true);
    }
}

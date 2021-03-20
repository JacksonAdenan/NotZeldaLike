using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public enum HeartStates { Disabled, Empty, Full }
    public HeartStates heartToggle = HeartStates.Disabled;
    public RawImage heartImage;

    private void Start()
    {
        heartImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        heartImage.enabled = true;
        switch (heartToggle)
        {
            case HeartStates.Disabled:
                heartImage.enabled = false;
                break;
            case HeartStates.Empty:
                heartImage.texture = Resources.Load<RenderTexture>("Textures/HeartEmpty");
                break;
            case HeartStates.Full:
                heartImage.texture = Resources.Load<RenderTexture>("Textures/HeartFull");
                break;
        }
    }
}

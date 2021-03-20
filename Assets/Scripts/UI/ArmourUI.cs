using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourUI : MonoBehaviour
{
    public enum ArmourStates { Disabled, Empty, Full }
    public ArmourStates armourToggle = ArmourStates.Disabled;
    public RawImage armourImage;

    private void Start()
    {
        armourImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        armourImage.enabled = true;
        switch (armourToggle)
        {
            case ArmourStates.Disabled:
                armourImage.enabled = false;
                break;
            case ArmourStates.Empty:
                armourImage.texture = Resources.Load<RenderTexture>("Textures/ShieldEmpty");
                break;
            case ArmourStates.Full:
                armourImage.texture = Resources.Load<RenderTexture>("Textures/ShieldFull");
                break;
        }
    }
}
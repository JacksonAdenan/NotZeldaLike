using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    private Transform itemsTransform;
    public float rotationSpeed = 0.05f;
    public float bobbingSpeed = 0.1f;
    public float bobbingHeight = 0.5f;

    public float originalY;

    // Start is called before the first frame update
    void Start()
    {
        itemsTransform = this.gameObject.transform;
        originalY = itemsTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        itemsTransform.Rotate(new Vector3(0, 0.1f, 0));

        float newY = (Mathf.Sin(Time.time * bobbingSpeed)) * bobbingHeight + originalY;

        itemsTransform.position = new Vector3(itemsTransform.position.x, newY, itemsTransform.position.z);
    }
}

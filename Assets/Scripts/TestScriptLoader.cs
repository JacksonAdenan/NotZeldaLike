using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptLoader : MonoBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        TestParent[] array = { new TestScript(), new TestScript2() };

        for (int i = 0; i < array.Length; i++)
        {
            array[i].PrintName();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

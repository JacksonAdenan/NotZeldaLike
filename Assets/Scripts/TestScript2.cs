using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : TestParent
{
    public int test { get; set; }

    // Start is called before the first frame update

    public TestScript2() { }
    void Start() => PrintMessage();


    // Update is called once per frame
    void Update()
    {

    }

    public void PrintMessage()
    {
        Debug.Log("It works.");
    }

    void TestParent.PrintName()
    {
        Debug.Log("TestScript 2");
    }
}

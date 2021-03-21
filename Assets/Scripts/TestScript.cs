﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : TestParent
{
    public int test { get; set; }

    public TestScript() {}

    // Start is called before the first frame update
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
        Debug.Log("TestScript 1");
    }
}

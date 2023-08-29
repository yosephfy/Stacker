using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BeforeStart : MonoBehaviour
{
    private GameObject startBox;
    private bool start;
    public static BeforeStart Instance;

    private void Awake()
    {
        startBox = this.gameObject;

        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    private void OnMouseDown()
    {
        if (!start)
            start = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        start = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start = true;
        }
    }

    public void SetStarted(bool st)
    {
        start = st;
    }

    public bool IsStarted()
    {
        return start;
    }

    public void debugRestart()
    {
        Debug.Log("Yo restart");
    }
}
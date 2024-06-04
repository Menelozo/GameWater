using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingWindow : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Loader.LoadScene("MainMenu");
        }
    }
}

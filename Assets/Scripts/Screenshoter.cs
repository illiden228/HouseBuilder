using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshoter : MonoBehaviour
{
    private string _path;
    private const string FOLDER_NAME = "Screenshots"; 
    private const string SCREEN_NAME = "Screenshot"; 
    
    private void Start()
    {
        _path = $"{Application.persistentDataPath}/{FOLDER_NAME}";
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string currentTime = DateTime.Now.ToString("MM-dd-yy-HH-mm-ss");
            
            ScreenCapture.CaptureScreenshot($"{_path}/{SCREEN_NAME} {currentTime}.png", 1);
        }
    }
}

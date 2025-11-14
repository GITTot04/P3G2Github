using System;
using System.IO;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    public static ConfigLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Name of scene config file.
    private const string gameDataFileName = "config.json";
    public Configs Configs { get; private set; } = new Configs();
}

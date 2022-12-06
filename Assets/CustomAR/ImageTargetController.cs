using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ImageFileSourceData
{
    public string Path;
    public string Name;
    public float Scale;
}

public class ImageTargetController : MonoBehaviour
{
    public string trackImageName;
    private void Awake()
    {
        trackImageName = name;
    }

}


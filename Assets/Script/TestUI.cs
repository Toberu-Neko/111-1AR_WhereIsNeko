using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUI : MonoBehaviour
{
    public GameObject[] map;
    public TextMeshProUGUI testText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool haveActive = false;
        testText.text = "";
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i].activeSelf)
            {
                testText.text = testText.text + map[i].name + "\n";
                haveActive = true;
            }
        }
        if (!haveActive)
        {
            testText.text = "No active map.";
        }
    }
}

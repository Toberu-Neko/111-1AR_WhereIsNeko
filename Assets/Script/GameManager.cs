using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this; 
    }

    [Header("更改時請連同下面的aRDetect也改成x*y")]
    [Tooltip("更改請同時改變Grid Layout Group的設定，還有下面的AR相關設定也要。")]
    public int x, y;
    public GameObject[] aRDetect;
}

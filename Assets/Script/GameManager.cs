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

    [Header("���ɽгs�P�U����aRDetect�]�令x*y")]
    [Tooltip("���ЦP�ɧ���Grid Layout Group���]�w�A�٦��U����AR�����]�w�]�n�C")]
    public int x, y;
    public GameObject[] aRDetect;
}

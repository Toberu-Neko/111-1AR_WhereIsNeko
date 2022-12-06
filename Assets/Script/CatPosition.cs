using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatPosition : MonoBehaviour
{
    [Header("按鈕")]
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject repositonButton;
    
    [Header("地圖")]
    private GameObject[] debugPosition;
    private GameObject[] alphaPosition;
    [SerializeField] private GameObject debugMapPrefab;
    [SerializeField] private GameObject alphaMiddle;
    [SerializeField] private GameObject alphaTestGameObj;

    [Header("DebugMode")]
    [SerializeField] bool debugMode;

    private GameObject debugGameObj;
    private GameObject[,] aRDetect;
    private GameObject[,] debugMap, debugCatPosition, debugArrow, alphaMap, alphaCatPosition, alphaArrow;
    private int x, y;
    private int nowPositionX, nowPositionY;
    private bool[,] catInPosition;
    bool scaned;
    

    void Start()
    {
        x = GameManager.instance.x;
        y = GameManager.instance.y;
        debugGameObj = GameObject.Find("Canvas/Debug");
        //alphaTestGameObj = GameObject.Find("Canvas/AlphaTest");


        if (GameManager.instance.aRDetect.Length != x * y)
        {
            Debug.LogError("地圖長寬錯誤！請重新檢查GameManager。");
        }
        //resetButton.SetActive(false);
        //repositonButton.SetActive(false);

        scaned = false;

        aRDetect = new GameObject[x, y];
        debugMap = new GameObject[x, y];
        debugCatPosition = new GameObject[x,y];
        debugArrow = new GameObject[x,y];
        alphaMap = new GameObject[x, y];
        alphaCatPosition = new GameObject[x, y];
        alphaArrow = new GameObject[x,y];

        catInPosition = new bool[x, y];



        debugPosition = new GameObject[x * y];
        alphaPosition = new GameObject[x * y];
        for (int i = 0; i < x * y; i++)
        {
            Instantiate(debugMapPrefab, debugGameObj.transform.Find("Grid Layout Group"));
            Instantiate(alphaMiddle, alphaTestGameObj.transform.Find("Maps"));
            debugPosition[i] = debugGameObj.transform.Find("Grid Layout Group").GetChild(i).gameObject;
            alphaPosition[i] = alphaTestGameObj.transform.Find("Maps").GetChild(i).gameObject;
        }

        int randomX = UnityEngine.Random.Range(0, x);
        int randomY = UnityEngine.Random.Range(0, y);
        int attachCount = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //附加
                aRDetect[i, j] = GameManager.instance.aRDetect[attachCount];

                debugMap[i, j] = debugPosition[attachCount];
                debugCatPosition[i, j] = debugMap[i, j].transform.Find("Middle").gameObject;
                debugArrow[i, j] = debugMap[i, j].transform.Find("Arrow").gameObject;

                alphaMap[i, j] = alphaPosition[attachCount];
                alphaCatPosition[i, j] = alphaMap[i, j].transform.Find("Middle").gameObject;
                alphaArrow[i, j] = alphaMap[i, j].transform.Find("Arrow").gameObject;

                //j是橫排，i是直排。i=2,j=0在o位置。
                // x x x
                // x x x
                // o x x

                alphaCatPosition[i, j].SetActive(false);
                alphaArrow[i, j].SetActive(false);

                debugCatPosition[i, j].SetActive(false);
                debugArrow[i, j].SetActive(false);

                attachCount++;

                if (i < randomX)
                {
                    if (i == randomX - 1 && j == randomY)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }

                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, -90);
                }
                if (i > randomX)
                {
                    if (i == randomX + 1 && j == randomY)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                }
                if(i == randomX && j > randomY)
                {
                    if (j == randomY + 1)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                }
                if (i == randomX && j < randomY)
                {
                    if (j == randomY - 1)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (i == randomX && j == randomY)
                {
                    debugCatPosition[i, j].SetActive(true);
                    debugArrow[i, j].SetActive(false);
                    alphaCatPosition[i, j].SetActive(false);
                    alphaArrow[i, j].SetActive(false);

                    nowPositionX = i;
                    nowPositionY = j;
                    catInPosition[i, j] = true;
                }
                else
                {
                    catInPosition[i, j] = false;
                }

                Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name+ " , " + catInPosition[i, j]);
            }
        }
    }
    private void Update()
    {
        if (debugMode && alphaTestGameObj.activeInHierarchy)
        {
            alphaTestGameObj.SetActive(false);
        }
        if (!debugMode && debugGameObj.activeInHierarchy)
        {
            debugGameObj.SetActive(false);
        }

        if (!scaned && !debugMode)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (aRDetect[i, j].activeInHierarchy && !scaned)
                    {
                        alphaMap[i, j].SetActive(true);
                        scaned = true;

                        //跳出視窗
                        if (catInPosition[i, j])
                        {
                            resetButton.SetActive(true);
                        }
                        else if(!catInPosition[i, j])
                        {
                            repositonButton.SetActive(true);
                        }
                        //成功的話跳重新開始按鈕
                        //沒成功的話換位置
                    }
                    else
                        alphaMap[i, j].SetActive(false);
                }
            }
        }

    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ClearMap()
    {
        scaned = false;
        repositonButton.SetActive(false);

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                debugMap[i, j].SetActive(false);
            }
        }
    }
    public void ChangeCatPosition()
    {
        Debug.Log("貓咪換位置！");
        bool xCanPlus = true;
        bool xCanMinuse = true;
        bool yCanPlus = true;
        bool yCanMinuse = true;

        if(nowPositionX == 0)
        {
            xCanMinuse = false;
        }
        if(nowPositionX == x - 1)
        {
            xCanPlus = false;
        }
        if(nowPositionY == 0)
        {
            yCanMinuse = false;
        }
        if (nowPositionY == y - 1)
        {
            yCanPlus = false;
        }


        bool toX = false;
        bool toY = false;

        //0往X軸移動，1往Y軸移動。
        int xYRandom = UnityEngine.Random.Range(0, 2);

        if(xYRandom == 0)
        {
            toX = true;
            toY = false;
        }
        if (xYRandom == 1)
        {
            toX = false;
            toY = true;
        }

        if (toX)
        {
            if(xCanPlus && xCanMinuse)
            {
                int _random = UnityEngine.Random.Range(0, 2);
                if (_random == 0)
                    nowPositionX--;
                if (_random == 1)
                    nowPositionX++;
            }
            if (!xCanPlus && xCanMinuse)
            {
                nowPositionX--;
            }
            if (xCanPlus && !xCanMinuse)
            {
                nowPositionX++;
            }

        }
        if (toY)
        {
            if (yCanPlus && yCanMinuse)
            {
                int _random = UnityEngine.Random.Range(0, 2);
                if (_random == 0)
                    nowPositionY--;
                if (_random == 1)
                    nowPositionY++;
            }
            if (!yCanPlus && yCanMinuse)
            {
                nowPositionY--;
            }
            if (yCanPlus && !yCanMinuse)
            {
                nowPositionY++;
            }
        }

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //j是橫排，i是直排。i=2,j=0在o位置。
                // x x x
                // x x x
                // o x x
                debugCatPosition[i, j].SetActive(false);
                debugArrow[i, j].SetActive(false);

                alphaCatPosition[i, j].SetActive(false);
                alphaArrow[i, j].SetActive(false);
                //Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name);
                if (i < nowPositionX)
                {
                    if (i == nowPositionX - 1 && j == nowPositionY)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, -90);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, -90);
                }
                if (i > nowPositionX)
                {
                    if (i == nowPositionX + 1 && j == nowPositionY)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                }
                if (i == nowPositionX && j > nowPositionY)
                {
                    if (j == nowPositionY + 1)
                    {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                }
                if (i == nowPositionX && j < nowPositionY)
                {
                    if (j == nowPositionY - 1)
                        {
                        debugArrow[i, j].SetActive(true);
                        alphaArrow[i, j].SetActive(true);
                    }
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                    alphaArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (i == nowPositionX && j == nowPositionY)
                {
                    debugCatPosition[i, j].SetActive(true);
                    debugArrow[i, j].SetActive(false);

                    alphaCatPosition[i, j].SetActive(true);
                    alphaArrow[i, j].SetActive(false);

                    catInPosition[i, j] = true;
                    nowPositionX = i;
                    nowPositionY = j;
                }

                if (i == nowPositionX && j == nowPositionY)
                {
                    debugCatPosition[i, j].SetActive(true);
                    alphaCatPosition[i, j].SetActive(true);
                    catInPosition[i, j] = true;
                    Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name + " , " + catInPosition[i, j]);
                }
                else
                {
                    debugCatPosition[i, j].SetActive(false);
                    alphaCatPosition[i, j].SetActive(false);

                    catInPosition[i, j] = false;
                }
            }
        }
        if(!debugMode)
            ClearMap();
    }
}

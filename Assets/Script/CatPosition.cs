using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CatPosition : MonoBehaviour
{
    [Header("���y�T�{�e��")]
    [SerializeField] GameObject confirmPage;
    [SerializeField] GameObject confirmButton;
    [SerializeField] TextMeshProUGUI confirmText;

    [Header("���s")]
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject repositonButton;
    
    [Header("�a��")]
    private GameObject[] debugPosition;
    private GameObject[] alphaPosition;
    [SerializeField] private GameObject debugMapPrefab;
    [SerializeField] private GameObject alphaMiddle;
    [SerializeField] private GameObject alphaTestGameObj;

    [Header("DebugMode")]
    [SerializeField] bool debugMode;

    private GameObject debugGameObj;
    private GameObject[,] aRDetect;
    //Alpha�O�����j�ϡBDebug�O�E�c��C
    private GameObject[,] debugMap, debugCatPosition, debugArrow, alphaMap, alphaCatPosition, alphaArrow;
    private int x, y;
    private int nowPositionX, nowPositionY;
    private bool[,] catInPosition;

    bool scaned;
    int keep_i, keep_j;

    void Start()
    {
        x = GameManager.instance.x;
        y = GameManager.instance.y;
        debugGameObj = GameObject.Find("Canvas/Debug");
        //alphaTestGameObj = GameObject.Find("Canvas/AlphaTest");

        if (GameManager.instance.aRDetect.Length != x * y)
        {
            Debug.LogError("�a�Ϫ��e���~�I�Э��s�ˬdGameManager�C");
        }
        resetButton.SetActive(false);
        repositonButton.SetActive(false);

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
                //���[
                aRDetect[i, j] = GameManager.instance.aRDetect[attachCount];

                debugMap[i, j] = debugPosition[attachCount];
                debugCatPosition[i, j] = debugMap[i, j].transform.Find("Middle").gameObject;
                debugArrow[i, j] = debugMap[i, j].transform.Find("Arrow").gameObject;

                alphaMap[i, j] = alphaPosition[attachCount];
                alphaCatPosition[i, j] = alphaMap[i, j].transform.Find("Middle").gameObject;
                alphaArrow[i, j] = alphaMap[i, j].transform.Find("Arrow").gameObject;
                alphaMap[i, j].SetActive(false);

                //j�O��ơAi�O���ơCi=2,j=0�bo��m�C
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

        if (!debugMode && !scaned)
        {
            int scanCount = 0;
            
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (aRDetect[i, j].activeInHierarchy)
                    {
                        scanCount++;
                        keep_i = i;
                        keep_j = j;
                    }
                }
            }
            if (scanCount == 1)
            {
                //��ܤFalphaMap[i, j]�a�ϡA�O�_�T�w�H
                confirmText.text = "Chose " + aRDetect[keep_i,keep_j].name + ".";
                confirmButton.SetActive(true);
                confirmPage.SetActive(true);
            }
            else if (scanCount != 0)
            {
                //���y�F�W�L�@�Ӧa�ϡA�Э��s���y
                confirmText.text = "Chose more than one map.";
                confirmButton.SetActive(false);
                confirmPage.SetActive(true);
            }
            else if (scanCount == 0)
            {
                confirmText.text = "";
                confirmPage.SetActive(false);
                confirmPage.SetActive(false);
            }
        }
    }
    public void ConfirmChoice()
    {
        //���\���ܸ����s�}�l���s
        //�S���\���ܴ���m
        scaned = true;

        alphaMap[keep_i, keep_j].SetActive(true);
        if (catInPosition[keep_i, keep_j])
        {
            resetButton.SetActive(true);
        }
        else if (!catInPosition[keep_i, keep_j])
        {
            repositonButton.SetActive(true);
        }
    }
    public void ClearMap()
    {
        scaned = false;
        repositonButton.SetActive(false);

        alphaMap[keep_i, keep_j].SetActive(false);
     /*   for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                alphaMap[i, j].SetActive(false);
            }
        }*/
    }
    public void ChangeCatPosition()
    {
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

        //0��X�b���ʡA1��Y�b���ʡC
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
                //j�O��ơAi�O���ơCi=2,j=0�bo��m�C
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
                    //Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name + " , " + catInPosition[i, j]);
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
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

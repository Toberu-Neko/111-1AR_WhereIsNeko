using UnityEngine;

public class CatPosition : MonoBehaviour
{
    private GameObject[,] aRDetect;
    [SerializeField]private GameObject[] debugPosition;
    private GameObject[,] debugMap, debugCatPosition, debugArrow;
    private int x, y;
    private int nowPositionX, nowPositionY;
    [SerializeField] private bool[,] catInPosition;
    

    void Start()
    {
        x = GameManager.instance.x;
        y = GameManager.instance.y;

        if(GameManager.instance.aRDetect.Length != x * y)
        {
            Debug.LogError("地圖長寬錯誤！請重新檢查GameManager。");
        }
        aRDetect = new GameObject[x, y];
        debugMap = new GameObject[x, y];
        debugCatPosition = new GameObject[x,y];
        debugArrow = new GameObject[x,y];
        catInPosition = new bool[x, y];
        int randomX = Random.Range(0, x);
        int randomY = Random.Range(0, y);
        int attachCount = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //j是橫排，i是直排。i=2,j=0在o位置。
                // x x x
                // x x x
                // o x x
                aRDetect[i, j] = GameManager.instance.aRDetect[attachCount];
                debugMap[i, j] = debugPosition[attachCount];
                debugCatPosition[i, j] = debugMap[i, j].transform.Find("Middle").gameObject;
                debugArrow[i, j] = debugMap[i, j].transform.Find("Arrow").gameObject;
                debugCatPosition[i, j].SetActive(false);
                debugArrow[i, j].SetActive(false);
                debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                attachCount++;
                //Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name);
                if (i < randomX)
                {
                    //debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                if (i > randomX)
                {
                    //debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                }
                if(i == randomX && j > randomY)
                {
                    //debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, -90);
                }
                if (i == randomX && j < randomY)
                {
                    debugArrow[i, j].SetActive(true);
                    debugArrow[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                }

                if (i == randomX && j == randomY)
                {
                    debugCatPosition[i, j].SetActive(true);
                    debugArrow[i, j].SetActive(false);
                    catInPosition[i, j] = true;
                    nowPositionX = i;
                    nowPositionY = j;
                }
                else
                    catInPosition[i, j] = false;

                Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name+ " , " + catInPosition[i, j]);
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
                int _random = Random.Range(0, 2);
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
                int _random = Random.Range(0, 2);
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
                if (i == nowPositionX && j == nowPositionY)
                {
                    debugCatPosition[i, j].SetActive(true);
                    catInPosition[i, j] = true;
                    Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name + " , " + catInPosition[i, j]);
                }
                else
                {
                    debugCatPosition[i, j].SetActive(false);
                    catInPosition[i, j] = false;
                }
            }
        }
    }
    void Update()
    {
        
    }
}

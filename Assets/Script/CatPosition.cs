using UnityEngine;

public class CatPosition : MonoBehaviour
{
    private GameObject[,] aRDetect;
    private int x, y;
    private int nowPositionX, nowPositionY;
    [SerializeField] private bool[,] catInPosition;
    

    void Start()
    {
        x = GameManager.instance.x;
        y = GameManager.instance.y;

        if(GameManager.instance.aRDetect.Length != x * y)
        {
            Debug.LogError("�a�Ϫ��e���~�I�Э��s�ˬdGameManager�C");
        }
        aRDetect = new GameObject[x, y];
        catInPosition = new bool[x, y];
        int randomX = Random.Range(0, x);
        int randomY = Random.Range(0, y);
        int attachCount = 0;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //j�O��ơAi�O���ơCi=2,j=0�bo��m�C
                // x x x
                // x x x
                // o x x
                aRDetect[i, j] = GameManager.instance.aRDetect[attachCount];
                attachCount++;
                //Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name);

                if (i == randomX && j == randomY)
                {
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
        Debug.Log("�߫}����m�I");
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
                    catInPosition[i, j] = true;
                    Debug.Log(i + " , " + j + " , " + aRDetect[i, j].name + " , " + catInPosition[i, j]);
                }
                else
                catInPosition[i, j] = false;
            }
        }
    }
    void Update()
    {
        
    }
}

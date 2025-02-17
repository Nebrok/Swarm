using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public static WorldGrid Instance;

    [SerializeField]
    private int _worldGridWidth;
    [SerializeField]
    private float _gridSize = 1f;

    [SerializeField] bool _showGrid = true;

    private bool[,] _gridOccupation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(this);
        }

        _gridOccupation = new bool[_worldGridWidth, _worldGridWidth];
        for (int j = 0; j < _worldGridWidth; j++)
        {
            for (int i = 0; i < _worldGridWidth; i++)
            {
                _gridOccupation[i, j] = false;
            }
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!_showGrid)
        {
            return;
        }

        int i = 0;
        int j = 0;
        for (int x = -(_worldGridWidth / 2); x < (_worldGridWidth / 2); x++)
        {
            j = 0;
            for (int y = -(_worldGridWidth / 2); y < (_worldGridWidth / 2); y++)
            {
                if (_gridOccupation[i, j])
                {
                    Gizmos.color = new Color(1, 0, 0, 0.1f);
                }
                else
                {
                    Gizmos.color = new Color(0, 1, 0, 0.1f);
                }
                Gizmos.DrawWireCube(new Vector3(x + _gridSize / 2, 0, y + _gridSize / 2), new Vector3(0.95f, 0.4f, 0.95f));
                j++;
            }
            i++;
        }
    }

    public static Vector3 GetWorldPosFromGrid(Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(0, 0, 0);

        worldPos.x = gridPos.x * Instance.GridSize;
        worldPos.z = gridPos.y * Instance.GridSize;

        Debug.Log(worldPos);
        return worldPos;
    }

    public static Vector2Int GetGridPosFromWorldPos(Vector2 worldPos)
    {
        Vector2Int gridPos = new Vector2Int(0, 0);

        return gridPos;
    }

    public void PlaceInGrid(Vector2Int gridPos, int gridWidth, int gridHeight)
    {
        int halfSize = _worldGridWidth / 2;
        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                int x = gridPos.x + halfSize + i;
                int y = gridPos.y + halfSize + j;
                //Debug.Log("Sizes " + x + " , " + y + " , " + halfSize);
                _gridOccupation[x, y] = true;
            }
        }

    }





    public float GridSize
    {
        get {  return _gridSize; }
    }


}

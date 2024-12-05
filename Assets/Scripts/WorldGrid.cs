using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    public static WorldGrid Instance;

    [SerializeField]
    private int _worldGridWidth = 100;
    [SerializeField]
    private float _gridSize = 1f;

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
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        for (int x = -(_worldGridWidth / 2); x < (_worldGridWidth / 2); x++)
        {
            for (int y = -(_worldGridWidth / 2); y < (_worldGridWidth / 2); y++)
            {
                Gizmos.DrawWireCube(new Vector3(x, 0, y), new Vector3(0.95f, 0.4f, 0.95f));
            }
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

    public float GridSize
    {
        get {  return _gridSize; }
    }


}

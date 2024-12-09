using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building")]
    [SerializeField]
    protected Vector2Int _gridPos; //Bottom-Right grid cell 
    [SerializeField]
    protected int _gridWidth;
    [SerializeField]
    protected int _gridHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SetTransformToGridPos()
    {
        WorldGrid.Instance.PlaceInGrid(_gridPos, _gridWidth, _gridHeight);

        Vector3 updatedPosition = WorldGrid.GetWorldPosFromGrid(_gridPos);
        updatedPosition.x += transform.localScale.x / 2;
        updatedPosition.z += transform.localScale.z / 2;
        updatedPosition.y = transform.localScale.y / 2;

        transform.position = updatedPosition;
    }
}

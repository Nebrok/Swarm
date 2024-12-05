using System.Collections.Generic;
using UnityEngine;

public interface IStorable
{
    public bool IsStorable();

    public bool IsStored();

    public void SetStored(bool value);

    public GameObject GetGameObject();
}


public class Storage : Building
{
    private List<IStorable> _storedItems = new List<IStorable>();
    private float _interationRange = 2f;

    private bool _storageModified = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gridPos = new Vector2Int(-17, -48);
        _gridWidth = 4;
        _gridHeight = 2;

        Vector3 updatedPosition = WorldGrid.GetWorldPosFromGrid(_gridPos);
        updatedPosition.x += transform.localScale.x / 2;
        updatedPosition.z += transform.localScale.z / 2;

        transform.position = updatedPosition;
        



    }

    // Update is called once per frame
    void Update()
    {
        if (_storageModified)
        {
            int index = 0;
            float runningHeightOffset = 0;
            foreach (IStorable item in _storedItems)
            {
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
                float itemHeight = item.GetGameObject().transform.localScale.y;
                newPosition.y += runningHeightOffset + itemHeight / 2;
                runningHeightOffset += itemHeight;
                item.GetGameObject().transform.position = newPosition;
                index++;
            }
            _storageModified = false;
        }
    }

    public bool AddItem(IStorable item)
    {
        _storedItems.Add(item);
        item.SetStored(true);
        _storageModified = true;
        return true;
    }

    public IStorable GetItem()
    {
        IStorable item = _storedItems[_storedItems.Count - 1];
        item.SetStored(false);

        return item;
    }

    #region Properties
    public float InterationRange
    {
        get { return _interationRange; }
    }
    #endregion

}

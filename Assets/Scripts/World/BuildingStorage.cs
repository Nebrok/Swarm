using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BuildingStorage : MonoBehaviour, IStorage
{
    private List<IStorable> _storedItems = new List<IStorable>();


    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItem(IStorable item)
    {
        _storedItems.Add(item);
        item.GetGameObject().transform.rotation = Quaternion.identity;
        item.SetStored(true);
        UpdateStorageDisplay();
        return true;
    }

    public IStorable GetItem()
    {
        IStorable item = _storedItems[_storedItems.Count - 1];
        item.SetStored(false);

        return item;
    }

    public void UpdateStorageDisplay()
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
    }

    public string GetStorageManifest()
    {
        string manifest = string.Empty;

        for (int i = 0;  i < _storedItems.Count; i++)
        {
            if (i == _storedItems.Count - 1)
            {
                manifest += _storedItems[i].GetItemName();
                continue;
            }
            manifest += _storedItems[i].GetItemName() + ", ";
        }
        return manifest;
    }

    public void ClearStorage()
    {
        _storedItems.Clear();
        UpdateStorageDisplay();
    }
}

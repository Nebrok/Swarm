using UnityEngine;

public interface IPickable
{
    public bool IsPickUpable();

    public bool IsPickedUp();

    public void SetPickedUp(bool value);
}

public class Resource : MonoBehaviour, IStorable, IPickable
{
    [SerializeField]
    private string _resourceName = string.Empty;
    
    public string ResourceName
    {
        get { return _resourceName; }
        set { _resourceName = value; }
    }

    private bool _isStored = false;
    private bool _isPickedUp = false;

    private GameObject _targetedBy = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

    }

    public bool IsStorable()
    {
        return true;
    }

    public bool IsTargeted()
    {
        return _targetedBy != null;
    }

    public GameObject IsTargetedBy()
    {
        return _targetedBy;
    }

    public void SetTargetedBy(GameObject targetedBy)
    {
        _targetedBy = targetedBy;
    }

    public bool IsStored()
    {
        return _isStored;
    }

    public void SetStored(bool value)
    {
        _isStored = value;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsPickUpable()
    {
        return true;
    }

    public bool IsPickedUp()
    {
        return _isPickedUp;
    }

    public void SetPickedUp(bool value)
    {
        _isPickedUp = value;
    }
 
    public string GetItemName()
    {
        return _resourceName;
    }
}

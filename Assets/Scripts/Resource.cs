using UnityEngine;


public interface IMineable
{
    public void Mine();
}

public interface IPickable
{
    public bool IsPickUpable();

    public bool IsPickedUp();

    public void SetPickedUp(bool value);
}

public class Resource : MonoBehaviour, IMineable, IStorable, IPickable
{
    public enum ResourceTier
    {
        Source, Raw, Refined
    }

    [SerializeField]
    private ResourceTier _resourceTier;

    private GameObject _childResource;

    private bool _isStored = false;
    private bool _isPickedUp = false;

    private GameObject _targetedBy = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _childResource = Resources.Load<GameObject>("Prefabs/ResourceRaw");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

    }

    public void Mine()
    {
        for (int i = 0; i < 1; i++)
        {
            Vector3 newPosition = new Vector3(transform.position.x, _childResource.transform.position.y, transform.position.z);
            Instantiate(_childResource, newPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public bool IsStorable()
    {
        if (_resourceTier == ResourceTier.Raw || _resourceTier == ResourceTier.Refined)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        if (_resourceTier == ResourceTier.Raw || _resourceTier == ResourceTier.Refined)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool IsPickedUp()
    {
        return _isPickedUp;
    }

    public void SetPickedUp(bool value)
    {
        _isPickedUp = value;
    }
 }

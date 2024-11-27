using UnityEngine;


public interface IMineable
{
    public void Mine();
}

public interface IPickable
{
    public bool IsPickUpable();
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




        //Outdated
        /*
        void UpdateModel()
        {
            MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer child in children)
            {
                Destroy(child.gameObject);
            }
            switch (_resourceTier)
            {
                case ResourceTier.Source:
                    Instantiate(_sourceModelPrefab, transform);
                    break;
                case ResourceTier.Raw:
                    Instantiate(_rawModelPrefab, transform);
                    break;
                case ResourceTier.Refined:
                    Instantiate(_refinedModelPrefab, transform);
                    break;
            }
        }
        */


 }

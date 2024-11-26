using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResourceTier
    {
        Source, Raw, Refined
    }

    [SerializeField]
    private ResourceTier _resourceTier;

    [SerializeField]
    GameObject _sourceModelPrefab;
    [SerializeField]
    GameObject _rawModelPrefab;
    [SerializeField]
    GameObject _refinedModelPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resourceTier = ResourceTier.Source;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateModel();

    }


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


}

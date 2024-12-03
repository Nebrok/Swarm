using UnityEngine;



public interface IMineable
{
    public void Mine();
}


public class Source : MonoBehaviour , IMineable
{

    [SerializeField]
    private GameObject _resourceProduced;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void Mine()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0f, 2f), 0, Random.Range(0f, 2f));
            Vector3 newPosition = new Vector3(transform.position.x, _resourceProduced.transform.position.y, transform.position.z);
            newPosition += randomOffset;
            Instantiate(_resourceProduced, newPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

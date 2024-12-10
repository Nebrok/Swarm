using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField]
    GameObject sphere;


    Vector3 vertex1 = new Vector3(0, 0, 10);
    Vector3 vertex2 = new Vector3(8.66f, 0, -5f);
    Vector3 vertex3 = new Vector3(-8.66f, 0, -5f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            float factor1 = Random.Range(0, 1f);
            float factor2 = Random.Range(0, 1 - factor1);
            float factor3 = 1 - factor1 - factor2;

            Vector3 newPos = vertex1 * factor1 + vertex2 * factor2 + vertex3 * factor3;
            Instantiate(sphere, newPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
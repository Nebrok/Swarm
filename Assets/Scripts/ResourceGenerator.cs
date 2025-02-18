using System.Collections;
using UnityEngine;


public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] public GameObject ResourceToSpawn;

    [SerializeField] private int _spawnNum;

    [SerializeField] private float _spawnRange;

    [SerializeField] bool drawSpawnArea = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _spawnNum; i++)
        {
            Vector3 randomLoc = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));
            randomLoc += transform.position;

            Instantiate(ResourceToSpawn, randomLoc, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if (drawSpawnArea)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_spawnRange, _spawnRange, _spawnRange));
        }
    }
}
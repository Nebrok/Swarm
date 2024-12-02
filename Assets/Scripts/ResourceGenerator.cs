using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject ResourceToSpawn;
    [SerializeField]
    public float SpawnTime;

    [SerializeField]
    private float _spawnRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnNewResource());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnRange, _spawnRange, _spawnRange));
    }


    IEnumerator SpawnNewResource()
    {
        while (true)
        {
            Vector3 resourcePos = new Vector3(Random.Range(-_spawnRange/2, _spawnRange/2), 0.25f, Random.Range(-_spawnRange/2, _spawnRange/2));
            resourcePos += transform.position;

            Instantiate(ResourceToSpawn, resourcePos, Quaternion.identity);
            yield return new WaitForSeconds(SpawnTime);
        }
    }
}
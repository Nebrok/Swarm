
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private float stickSpawnMaxRadius = 4f;
    private float stickSpawnMinRadius = 1f;

    private GameObject _stickPrefab;

    void Start()
    {
        _stickPrefab = Resources.Load<GameObject>("Prefabs/Stick");
        StartCoroutine(DropStick());
    }

    void Update()
    {
        float localScale = transform.localScale.x;
        float change = 1 / (20 * (localScale * localScale));
        float newScale = change * Time.deltaTime + localScale;
        if (newScale > 2) newScale = 2;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    IEnumerator DropStick()
    {
        while (true)
        {
            Vector3 randomLocation = new Vector3(0, 0, 0);

            for (int i = 0; i < 10; i++)
            {
                float randomX = Random.Range(-stickSpawnMaxRadius, stickSpawnMaxRadius);
                float randomZ = Random.Range(-stickSpawnMaxRadius, stickSpawnMaxRadius);
                randomLocation = new Vector3(randomX, 0, randomZ);

                float rLocMag = randomLocation.magnitude;
                if (rLocMag < stickSpawnMinRadius || rLocMag > stickSpawnMaxRadius)
                {
                    continue;
                }
            }

            Vector3 newLocation = randomLocation + transform.position;
            newLocation.y = _stickPrefab.transform.position.y;

            
            float diceRoll = Random.value * 100;

            if (randomLocation.x != 0 && diceRoll < 1)
            {
                Instantiate(_stickPrefab, newLocation, Quaternion.identity);
            }

            yield return new WaitForSeconds(1);
        }
    }
}

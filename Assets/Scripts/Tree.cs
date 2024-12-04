using UnityEngine;

public class Tree : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float localScale = transform.localScale.x;
        float change = 1 / (20 * (localScale * localScale));
        float newScale = change * Time.deltaTime + localScale;
        if (newScale > 2) newScale = 2;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}

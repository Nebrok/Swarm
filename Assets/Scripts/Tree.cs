using UnityEngine;

public class Tree : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float localScale = transform.localScale.x;
        float change = 1 / (5 *(localScale * localScale));
        float newScale = change * Time.deltaTime + localScale;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}

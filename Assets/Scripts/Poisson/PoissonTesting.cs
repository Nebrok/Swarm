using UnityEngine;

public class PoissonTesting : MonoBehaviour
{
    [SerializeField] int _samplingHalfWidth;
    [SerializeField] float _minimumDist;

    PoissonDiskSamples _distro;

    [SerializeField] GameObject _testSphere;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _distro = new PoissonDiskSamples(_samplingHalfWidth, _minimumDist, 30);

        foreach (Vector2 pos in _distro.PoissonDiscSample)
        {
            Vector3 newPos = new Vector3(pos.x, 0.1f, pos.y);
            Instantiate(_testSphere, newPos, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

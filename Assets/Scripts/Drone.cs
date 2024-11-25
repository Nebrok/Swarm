using UnityEngine;

public class Drone : MonoBehaviour
{
    private Hub _parentHub = null;

    private float _movementSpeed = 2f;

    private Vector3 _currentTarget;


    #region Setters
    public void SetParentHub(Hub newParentHub)
    {
        _parentHub = newParentHub;
    }

    public void SetCurrentTarget(Vector3 newTarget)
    {
        _currentTarget = newTarget;
    }
    #endregion 

    void Start()
    {
        
    }

    public void UpdateEntity()
    {
        if (_currentTarget != null)
        {
            MoveTowards(_currentTarget, 0.75f);
        }
    }


    void MoveTowards(Vector3 target, float throttle)
    {
        //Keeps the drone locked on the y plane
        Vector3 actualTargetPosition = new Vector3(target.x, transform.position.y, target.z);

        transform.position = Vector3.MoveTowards(transform.position, actualTargetPosition, Time.deltaTime * _movementSpeed * throttle);
    }

    void CollectResource(Vector3 resourcePosition)
    {

    }

    
}

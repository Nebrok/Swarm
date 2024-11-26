using System.Collections.Generic;
using UnityEngine;


public interface IMovable
{
    public float GetMaxSpeed();
}

public class Drone : MonoBehaviour, IMovable
{
    private Hub _parentHub = null;

    private float _movementSpeed = 2f;

    private Vector3 _currentTarget;

    private TaskSystem _taskSystem;

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

    #region Getters
    public float GetMaxSpeed()
    {
        return _movementSpeed;
    }
    #endregion


    void Awake()
    {
        _taskSystem = new TaskSystem();
    }

    public void UpdateEntity()
    {
        if (_currentTarget != null)
        {
            //MoveTowards(_currentTarget, 0.75f);
        }

        _taskSystem.Run();
    }


    void MoveTowards(Vector3 target, float throttle)
    {
        //Keeps the drone locked on the y plane
        Vector3 actualTargetPosition = new Vector3(target.x, transform.position.y, target.z);
        transform.position = Vector3.MoveTowards(transform.position, actualTargetPosition, Time.deltaTime * _movementSpeed * throttle);
    }

    public void CollectResource(GameObject resource)
    {
        _taskSystem.AddTask(new TravelToEntity(gameObject, resource, 0.75f));
    }

    
}



public class TravelToEntity : Task
{
    private GameObject _goal;
    private GameObject _self;
    private float _throttle;
    private float _maxSpeed;

    public TravelToEntity(GameObject self, GameObject goal, float throttle) : base("TravelToEntity")
    {
        _self = self;
        _goal = goal;
        _throttle = throttle;
        _self.TryGetComponent(out IMovable movableInterface);
        _maxSpeed = movableInterface.GetMaxSpeed();
    }

    public override void Execute()
    {
        if (TaskStatus == Status.Pending)
        {
            TaskStatus = Status.Ongoing;
        }

        //Keeps the drone locked on the y plane
        Vector3 target = _goal.transform.position;
        Transform selfTransform = _self.transform;
        Vector3 actualTargetPosition = new Vector3(target.x, selfTransform.position.y, target.z);
        selfTransform.position = Vector3.MoveTowards(selfTransform.position, actualTargetPosition, Time.deltaTime * _maxSpeed * _throttle);

        float distToObject = (_goal.transform.position - selfTransform.position).magnitude;
        if (distToObject < 1f)
        {
            TaskStatus = Status.Finished;
        }


    }
}
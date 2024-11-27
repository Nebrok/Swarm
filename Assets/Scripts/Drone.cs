using System.Collections.Generic;
using UnityEngine;


public interface IMovable
{
    public float GetMaxSpeed();
}

public class Drone : MonoBehaviour, IMovable
{
    private Hub _parentHub = null;

    private float _movementSpeed = 4f;
    private float _interactionRadius = 1f;

    private bool _isCarrying = false;
    private GameObject _carriedItem;

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

    public Hub GetParentHub()
    {
        return _parentHub;
    }
    #endregion


    void Awake()
    {
        _taskSystem = new TaskSystem();
    }

    public void UpdateEntity()
    {
        _taskSystem.Run();
    }

    //Objective
    public void GoMineResource(GameObject resource)
    {
        _taskSystem.AddTask(new TravelToEntity(gameObject, resource, 0.75f));
        _taskSystem.AddTask(new DroneMineSurroundings(this));
    }

    //Action
    public void Mine()
    {
        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, _interactionRadius);
        
        foreach (Collider collider in surroundingObjects)
        {
            if (collider.CompareTag("Resource"))
            {
                IMineable resource = collider.gameObject.GetComponent<IMineable>();
                if (resource != null)
                {
                    resource.Mine();
                }
            }
        }
    }

    //Action
    public void Carry(GameObject resource)
    {
        _isCarrying = true;
        _carriedItem = resource;
        resource.transform.SetParent(transform, false);
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

        float distToObject = (target - selfTransform.position).magnitude;
        if (distToObject < 1f)
        {
            TaskStatus = Status.Finished;
            Debug.Log("TaskFinished");
        }


    }
}

public class DroneReturnToHub : Task
{
    private GameObject _goal;
    private Drone _self;
    private float _throttle;
    private float _maxSpeed;


    public DroneReturnToHub(Drone self, float throttle) : base("Return to Hub")
    {
        _self = self;
        _goal = _self.GetParentHub().gameObject;
        _throttle = throttle;
        _maxSpeed = _self.GetMaxSpeed();
    }

    public override void Execute()
    {
        if (TaskStatus == Status.Pending)
        {
            TaskStatus = Status.Ongoing;
        }

        Vector3 target = _goal.transform.position;
        Transform selfTransform = _self.transform;
        Vector3 actualTargetPosition = new Vector3(target.x, selfTransform.position.y, target.z);
        selfTransform.position = Vector3.MoveTowards(selfTransform.position, actualTargetPosition, Time.deltaTime * _maxSpeed * _throttle);

        float distToObject = (target - selfTransform.position).magnitude;
        if (distToObject < 2.5f)
        {
            TaskStatus = Status.Finished;
        }


    }


}

public class DroneMineSurroundings : Task
{
    private Drone _self;

    public DroneMineSurroundings(Drone self) : base("DroneMineSurroundings")
    {
        _self = self;
    }

    public override void Execute()
    {
        _self.Mine();
        TaskStatus = Status.Finished;
    }
}


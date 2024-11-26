using Mono.Cecil;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public interface IMovable
{
    public float GetMaxSpeed();
}

public interface ICanCarryItems
{
    public void Carry(GameObject item);
    public GameObject Drop();
}

public class Drone : MonoBehaviour, IMovable, ICanCarryItems
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

    //ActionSequence
    public void GoMineResource(GameObject resource)
    {
        _taskSystem.AddTask(new TravelToEntity(gameObject, resource, 0.75f));
        _taskSystem.AddTask(new DroneMineSurroundings(this));
    }

    //ActionSequence
    public void TestObjective(GameObject resource, GameObject dropOff)
    {
        _taskSystem.AddTask( new TravelToEntity(gameObject, resource, 0.75f));
        _taskSystem.AddTask( new PickUpItemNearby(this, resource));
        _taskSystem.AddTask( new TravelToEntity(gameObject, dropOff, 0.75f));
        _taskSystem.AddTask( new DropItem(this));

    }

    //ActionSequence
    public void DropCurrentItemAndMoveNewItemToDepot(GameObject item, Storage depot)
    {
        if (_isCarrying)
        {
            _taskSystem.AddTask(new DropItem(this));
        }
        _taskSystem.AddTask(new TravelToEntity(gameObject, item, 0.75f));
        _taskSystem.AddTask(new PickUpItemNearby(this, item));
        _taskSystem.AddTask(new TravelToEntity(gameObject, depot.gameObject, 0.75f));
        _taskSystem.AddTask(new StoreItem(this, depot));
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
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        resource.transform.position = newPosition;

    }

    //Action
    public GameObject Drop()
    {
        if (!_isCarrying)
        {
            return null;
        }
        _carriedItem.transform.parent = null;
        Vector3 newPosition = transform.position + transform.forward;
        newPosition.y = _carriedItem.transform.localScale.y / 2;

        _carriedItem.transform.position = newPosition;
        GameObject droppedItem = _carriedItem;
        _carriedItem = null;
        _isCarrying = false;
        return droppedItem;
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

public class PickUpItemNearby : Task
{
    ICanCarryItems _self;
    GameObject _item;

    public PickUpItemNearby(ICanCarryItems self, GameObject item) : base("PickUpItemNearby")
    {
        _self = self;
        _item = item;
    }

    public override void Execute()
    {
        _self.Carry(_item);
        TaskStatus = Status.Finished;
    }
}

public class DropItem : Task
{
    ICanCarryItems _self;

    public DropItem(ICanCarryItems self) : base("DropItem")
    {
        _self = self;
    }

    public override void Execute()
    {
        _self.Drop();
        TaskStatus = Status.Finished;
    }
}

public class StoreItem : Task
{
    ICanCarryItems _self;
    Storage _targetDepot;

    public StoreItem(ICanCarryItems self, Storage depot) : base("StoreItem")
    {
        _self = self;
        _targetDepot = depot;
    }

    public override void Execute()
    {
        _self.Drop().TryGetComponent(out Resource item);
        IStorable itemToStore = item;

        _targetDepot.AddItem(itemToStore);
        TaskStatus = Status.Finished;
    }
}


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
    float _rotationOffset = 90;
    //on justaguy model -transform.right is facing forward


    private float _movementSpeed = 4f;
    private float _interactionRadius = 1.5f;

    private bool _isCarrying = false;
    private GameObject _carriedItem;

    private TaskSystem _taskSystem;

    #region Setters
    public void SetParentHub(Hub newParentHub)
    {
        _parentHub = newParentHub;
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
        Vector3 newRotation = new Vector3(0, _rotationOffset, 0);
        transform.rotation = Quaternion.Euler(newRotation);
    }

    public void UpdateEntity()
    {
        _taskSystem.Run();
        if (_isCarrying && _carriedItem != null)
        {
            Vector3 carriedItemPos = transform.position + -transform.right;
            carriedItemPos.y = 1f;


            _carriedItem.transform.position = carriedItemPos;
            _carriedItem.transform.rotation = transform.rotation;
        }
    }

    public int GetTaskQueueLength()
    {
        return _taskSystem.GetTaskQueueLength();
    }

    //ActionSequence
    public void GoMineResource(GameObject resource)
    {
        _taskSystem.AddTask(new TravelToEntity(gameObject, resource, 0.75f));
        _taskSystem.AddTask(new DroneMineSurroundings(this));
    }

    //ActionSequence
    public void MoveNewItemToDepot(GameObject item, Storage depot)
    {
        if (_isCarrying)
        {
            _taskSystem.AddTask(new DropItem(this));
        }
        if (item.TryGetComponent(out Resource resource))
        {
            resource.SetTargetedBy(gameObject);
        }
        _taskSystem.AddTask(new TravelToEntity(gameObject, item, 0.75f));
        _taskSystem.AddTask(new PickUpItemNearby(this, item));
        _taskSystem.AddTask(new TravelToEntity(gameObject, depot.gameObject, 0.75f));
        _taskSystem.AddTask(new StoreItem(this, depot));
        ReturnToHub();
    }

    //ActionSequence
    public void ReturnToHub()
    {
        _taskSystem.AddTask(new DroneReturnToHub(this, 0.75f));
    }

    //Action
    public void Mine()
    {
        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, _interactionRadius);
        
        foreach (Collider collider in surroundingObjects)
        {
            if (collider.CompareTag("Source"))
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
        
        if (resource.TryGetComponent(out IPickable component))
        {
            component.SetPickedUp(true);
        }
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
        if (_carriedItem.TryGetComponent(out IPickable component))
        {
            component.SetPickedUp(false);
        }

        _carriedItem.transform.position = newPosition;
        GameObject droppedItem = _carriedItem;
        _carriedItem = null;
        _isCarrying = false;
        return droppedItem;
    }

    private void OnDrawGizmos()
    {
        Vector3 rayPos = transform.position;
        rayPos.y += 1f;
        Gizmos.DrawRay(rayPos, -transform.right);
    }
}

//Tasks
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
        selfTransform.right = selfTransform.position - actualTargetPosition;
        selfTransform.position = Vector3.MoveTowards(selfTransform.position, actualTargetPosition, Time.deltaTime * _maxSpeed * _throttle);

        float distToObject = (target - selfTransform.position).magnitude;
        if (distToObject < 1f)
        {
            TaskStatus = Status.Finished;
            //Debug.Log("TaskFinished");
        }
    }
}

public class DroneReturnToHub : Task
{
    private Hub _goal;
    private Drone _self;
    private float _throttle;
    private float _maxSpeed;
    private Vector3 targetPosition;


    public DroneReturnToHub(Drone self, float throttle) : base("Return to Hub")
    {
        _self = self;
        _goal = _self.GetParentHub();
        _throttle = throttle;
        _maxSpeed = _self.GetMaxSpeed();

        targetPosition = _goal.GetFreeIdleCoordinates();
        TaskPriority = Priority.Low;
    }

    public override void Execute()
    {
        if (TaskStatus == Status.Pending)
        {
            TaskStatus = Status.Ongoing;
        }

        Transform selfTransform = _self.transform;
        Vector3 actualTargetPosition = new Vector3(targetPosition.x, selfTransform.position.y, targetPosition.z);
        selfTransform.right = selfTransform.position - actualTargetPosition;
        selfTransform.position = Vector3.MoveTowards(selfTransform.position, actualTargetPosition, Time.deltaTime * _maxSpeed * _throttle);

        float distToObject = (actualTargetPosition - selfTransform.position).magnitude;
        if (distToObject < 0.01f)
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


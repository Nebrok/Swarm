using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hub : MonoBehaviour
{
    //Hub World Data
    [SerializeField]
    private float _operationRadius;
    private List<Resource> _freeResources = new List<Resource>();
    private List<Resource> _targetResources = new List<Resource>();
    private List<Storage> _storages = new List<Storage>();
    private List<Source> _sources = new List<Source>();


    //Storage
    private List<IStorable> _storedItems = new List<IStorable>();

    //Drones
    [SerializeField]
    List<Drone> ChildDrones = new List<Drone>();
    private int _maxDrones = 20;
    List<Vector3> _allIdlePositions = new List<Vector3>();
    List<Vector3> _freeIdlePositions = new List<Vector3>();


    //Idle
    private float _minimumRadius = 3f;
    private float _maximumRadius = 6f;


    void Start()
    {
        _allIdlePositions = GenerateDronePositions();
        _freeIdlePositions.AddRange(_allIdlePositions);

        for (int i = 0; i < 3; i++)
        {
            CreateNewDrone();
        }

        StartCoroutine(HubUpdateAvailiability());
        StartCoroutine(ScanEnvironmentRoutine());
    }

    void Update()
    {
        foreach (Drone drone in ChildDrones)
        {
            drone.UpdateEntity();
        }

        List<Resource> updatedResources = new List<Resource>();
        foreach (Resource resource in _freeResources)
        {
            Storage targetStorage = FindStorageOfType(resource.ResourceName);
            if (targetStorage == null)
            {
                continue;
            }
            Drone assignedDrone = GetUnassignedDrone();
            if (assignedDrone != null && !_targetResources.Contains(resource))
            {
                assignedDrone.MoveNewItemToDepot(resource.gameObject, targetStorage);
                updatedResources.Add(resource);
                _targetResources.Add(resource);
            }
            
        }
        foreach (Resource resource in updatedResources)
        {
            _freeResources.Remove(resource);
        }
    }

    public void CreateNewDrone()
    {
        if (ChildDrones.Count < _maxDrones)
        {
            GameObject dronePrefab = Resources.Load<GameObject>("Prefabs/justaguy");

            Vector3 newPos = GetFreeIdleCoordinates();
            newPos.y = dronePrefab.transform.position.y;

            Drone newDrone = Instantiate(dronePrefab, newPos, Quaternion.identity).GetComponent<Drone>();
            newDrone.SetParentHub(this);
            ChildDrones.Add(newDrone);
        }
        else
        {
            Debug.Log("Hub cannot surpervise any more drones");
        }
    }

    public void ScanEnvironment()
    {
        _freeResources.Clear();
        _storages.Clear();
        Collider[] objects = Physics.OverlapSphere(transform.position, _operationRadius);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.TryGetComponent(out Resource resource))
            {
                if (resource.IsStored())
                {
                    continue;
                }
                _freeResources.Add(resource);
            }

            if (objects[i].gameObject.TryGetComponent(out Storage storageDepot))
            {
                _storages.Add(storageDepot);
            }

            if (objects[i].gameObject.TryGetComponent(out Source source))
            {
                _sources.Add(source);
            }




        }

        Debug.Log($"Currently {_freeResources.Count} resources laying around.");
        Debug.Log($"Currently {_storages.Count} storage depot in operation area.");
    }

    public Drone GetUnassignedDrone()
    {
        foreach (Drone drone in ChildDrones)
        {
            if (drone.GetTaskQueueLength() == 0)
            {
                return drone;
            }
        }
        return null;
    }

    public Storage FindStorageOfType(string type)
    {
        foreach (Storage storage in _storages)
        {
            if (storage.StorageType == type)
            {
                return storage;
            }
        }
        return null;
    }

    public List<Vector3> GenerateDronePositions()
    {
        List<Vector3> finalPositions = new List<Vector3>();

        while (finalPositions.Count < _maxDrones)
        {
            float randomX = Random.Range(-_maximumRadius, _maximumRadius);
            float randomZ = Random.Range(-_maximumRadius, _maximumRadius);
            Vector3 randomLocation = new Vector3(randomX, 0, randomZ);


            float rLocMag = randomLocation.magnitude;
            if (rLocMag < _minimumRadius || rLocMag > _maximumRadius)
            {
                //Debug.Log("MagCheck");
                continue;
            }

            bool nearbyLocation = false;
            foreach (Vector3 existingPos in finalPositions)
            {
                if ((existingPos - randomLocation).magnitude < 1.5f) nearbyLocation = true;
            }
            if (nearbyLocation) continue;


            finalPositions.Add(randomLocation);
        }

        return finalPositions;
    }

    public void CheckIdleAvailiability()
    {
        _freeIdlePositions.Clear();
        _freeIdlePositions.AddRange(_allIdlePositions);
        foreach (Vector3 position in _allIdlePositions)
        {
            Collider[] objects = Physics.OverlapSphere(position + transform.position, 0.2f);
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].name.Contains("Drone"))
                {
                    _freeIdlePositions.Remove(position);
                }
            }
        }
    }

    public Vector3 GetFreeIdleCoordinates()
    {
        int idlePositionIndex = Random.Range(0, _freeIdlePositions.Count);
        Vector3 selectedPosition = _freeIdlePositions[idlePositionIndex];
        _freeIdlePositions.RemoveAt(idlePositionIndex);
        return selectedPosition + transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 3f);
        Gizmos.DrawWireSphere(transform.position, _operationRadius);
    }

    IEnumerator HubUpdateAvailiability()
    {
        while (true)
        {
            //Debug.Log("Update Idle Availiability");
            CheckIdleAvailiability();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ScanEnvironmentRoutine()
    {
        while (true)
        {
            //Debug.Log("Hub Scanning Environment");
            ScanEnvironment();
            yield return new WaitForSeconds(1f);
        }
    }


}

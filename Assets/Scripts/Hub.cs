using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
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

        for (int i = 0; i < 20; i++)
        {
            CreateNewDrone();
        }
    }

    void Update()
    {
        foreach (Drone drone in ChildDrones)
        {
            drone.UpdateEntity();
        }
    }

    public void CreateNewDrone()
    {
        if (ChildDrones.Count < _maxDrones)
        {
            GameObject dronePrefab = Resources.Load<GameObject>("Prefabs/Drone");

            Vector3 newPos = GetFreeIdleCoordinates();
            newPos.y = dronePrefab.transform.position.y;

            Drone newDrone = Instantiate(dronePrefab, newPos, Quaternion.identity).GetComponent<Drone>();
            ChildDrones.Add(newDrone);
        }
        else
        {
            Debug.Log("Hub cannot surpervise any more drones");
        }
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
    }


    IEnumerator HubUpdateAvailiability()
    {
        while (true)
        {
            CheckIdleAvailiability();
            yield return new WaitForSeconds(0.5F);
        }
    }


}

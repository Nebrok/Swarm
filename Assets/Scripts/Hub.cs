using System.Collections.Generic;

using UnityEngine;

public class Hub : MonoBehaviour
{
    //Drones
    [SerializeField]
    List<Drone> ChildDrones = new List<Drone>();

    //Test
    [SerializeField]
    List<Resource> TestResources = new List<Resource>();
    [SerializeField]
    GameObject testPickUpItem;
    [SerializeField]
    Storage testDropOffPoint;


    void Start()
    {
        int index = 0;
        foreach (Drone drone in ChildDrones)
        {
            drone.SetParentHub(this);
            drone.MoveNewItemToDepot(TestResources[index].gameObject, testDropOffPoint);
            index++;
        }

        PoissonDiskSamples samples = new PoissonDiskSamples(10, 1f);
    }

    void Update()
    {
        foreach (Drone drone in ChildDrones)
        {
            drone.UpdateEntity();
        }
    }



}

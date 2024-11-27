using System.Collections.Generic;

using UnityEngine;

public class Hub : MonoBehaviour
{
    //Resources
    [SerializeField]
    Transform WaterSourceTransform;
    [SerializeField]
    Transform IronSourceTransform;
    [SerializeField]
    Transform CoalSourceTransform;

    //Drones
    [SerializeField]
    List<Drone> ChildDrones = new List<Drone>();
    [SerializeField]
    Transform GoalTransform;

    //Resources
    [SerializeField]
    List<Resource> TestResources = new List<Resource>();

    [SerializeField]
    private float _water = 10;
    private float _waterLoss = -0.03f;

    [SerializeField]
    private float _iron = 2;
    private float _ironLoss = -0.002f;

    [SerializeField]
    private float _coal = 1;
    private float _coalBurnRate = -0.001f;

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
            //drone.GoMineResource(GoalTransform.gameObject);
            drone.DropCurrentItemAndMoveNewItemToDepot(TestResources[index].gameObject, testDropOffPoint);
            index++;
        }
    }

    void Update()
    {
        //Resource Drain
        _water += _waterLoss;
        _iron += _ironLoss;
        _coal += _coalBurnRate;



        foreach (Drone drone in ChildDrones)
        {
            drone.UpdateEntity();
        }
    }



}

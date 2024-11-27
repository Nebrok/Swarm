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

    [SerializeField]
    private float _water = 10;
    private float _waterLoss = -0.03f;

    [SerializeField]
    private float _iron = 2;
    private float _ironLoss = -0.002f;

    [SerializeField]
    private float _coal = 1;
    private float _coalBurnRate = -0.001f;


    void Start()
    {
        foreach (Drone drone in ChildDrones)
        {
            drone.SetParentHub(this);
            drone.GoMineResource(GoalTransform.gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GT_State : MonoBehaviour
{
    public enum Status
    {
        None, Pending, Ongoing, Finished
    };

    public Status StateStatus = Status.None; 

    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void EndState();
}

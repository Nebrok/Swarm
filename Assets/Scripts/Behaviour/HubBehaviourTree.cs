using UnityEngine;

public class HubBehaviourTree : BehaviourTree
{
    protected override BehaviourNode SetupTree()
    {
        BehaviourNode rootNode = new Sequence(this);





        return rootNode;
    }
}

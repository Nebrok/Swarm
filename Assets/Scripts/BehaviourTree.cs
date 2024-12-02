using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTree : MonoBehaviour
{
    protected BehaviourNode root = null;

    public Dictionary<string, object> TreeData = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        root = SetupTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (root != null)
        {
            root.Evaluate();
        }
    }

    protected abstract BehaviourNode SetupTree();
}

public enum NodeState
{
    Running,
    Success,
    Failure
}

public class BehaviourNode
{
    protected BehaviourTree _tree;
    public NodeState State;
    BehaviourNode _parent = null;

    protected List<BehaviourNode> _children = new List<BehaviourNode>();

    public BehaviourNode(BehaviourTree tree)
    {
        _tree = tree;
    }

    public void GiveParent(BehaviourNode node)
    {
        _parent = node;
    }

    public void AddChild(BehaviourNode child)
    {
        _children.Add(child);
    }

    public virtual NodeState Evaluate()
    {
        return NodeState.Failure;
    }
}

public class Selector : BehaviourNode
{
    public Selector(BehaviourTree tree) : base(tree)
    {

    }

    public override NodeState Evaluate()
    {
        foreach (BehaviourNode child in _children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    State = NodeState.Running;
                    return State;
                case NodeState.Success:
                    State = NodeState.Success;
                    return State;
                case NodeState.Failure:
                    continue;
            }
        }

        State = NodeState.Failure;
        return State;
    }
}

public class Sequence : BehaviourNode
{
    public Sequence(BehaviourTree tree) : base(tree)
    {

    }

    public override NodeState Evaluate()
    {
        bool anyChildrenRunning = false;

        foreach (BehaviourNode child in _children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    anyChildrenRunning = true;
                    break;
                case NodeState.Success:
                    State = NodeState.Success;
                    continue;
                case NodeState.Failure:
                    State = NodeState.Failure;
                    return State;
            }
        }

        State = anyChildrenRunning ? NodeState.Running : NodeState.Success;
        return State;
    }
}

public class Conditional : BehaviourNode
{
    private string _key;
    private bool _desiredEvaluation;


    public Conditional(BehaviourTree tree, bool desiredEvaluation, string key) : base(tree)
    {
        _key = key;
        _desiredEvaluation = desiredEvaluation;
    }

    public override NodeState Evaluate()
    {
        if ((bool)_tree.TreeData[_key] == _desiredEvaluation)
        {
            _children[0].Evaluate();
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}

public class Tautology : BehaviourNode
{
    public Tautology(BehaviourTree tree) : base(tree) { }

    public override NodeState Evaluate()
    {
        _children[0].Evaluate();
        return NodeState.Success;
    }
}
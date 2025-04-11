using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GT_StateMachine : MonoBehaviour
{
    public List<GT_State> _possibleStates = new List<GT_State>();
    private GT_State _currentState;

    protected void UpdateStateMachine()
    {
        _currentState?.UpdateState();
    }

    protected void TransitionState(GT_State nextState)
    {
        for (int i = 0; i < _possibleStates.Count; i++)
        {
            if (_possibleStates[i] == nextState)
            {
                _currentState?.EndState();
                _currentState = _possibleStates[i];
                _currentState.StartState();
                return;
            }
        }
        Debug.Log("Invalid State Transition");
    }

    public GT_State CurrentState { get { return _currentState; } }

    public GT_State FindState<state>()
    {
        for (int i = 0; i < _possibleStates.Count; i++)
        {
            if (_possibleStates[i].GetType() == typeof(state))
            {
                return _possibleStates[i];
            }
        }
        Debug.LogError("Could not find state");
        return null;
    }
}

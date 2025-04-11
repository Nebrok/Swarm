using UnityEngine;

public class GT_Drone : MonoBehaviour, IMovable, ICanCarryItems
{
    float _rotationOffset = 90;
    //on justaguy model -transform.right is facing forward

    private float _movementSpeed = 4f;
    private float _interactionRadius = 1.5f;

    private bool _isCarrying = false;
    private GameObject _carriedItem;

    #region Getters
    public float GetMaxSpeed()
    {
        return _movementSpeed;
    }
    #endregion
    
    void Awake()
    {
        Vector3 newRotation = new Vector3(0, _rotationOffset, 0);
        transform.rotation = Quaternion.Euler(newRotation);
    }

    public void UpdateEntity()
    {
        if (_isCarrying && _carriedItem != null)
        {
            Vector3 carriedItemPos = transform.position + -transform.right;
            carriedItemPos.y = 1f;


            _carriedItem.transform.position = carriedItemPos;
            _carriedItem.transform.rotation = transform.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rayPos = transform.position;
        rayPos.y += 1f;
        Gizmos.DrawRay(rayPos, -transform.right);
    }

    public void Carry(GameObject resource)
    {
        _isCarrying = true;
        _carriedItem = resource;

        if (resource.TryGetComponent(out IPickable component))
        {
            component.SetPickedUp(true);
        }
    }

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

}




public class GT_MoveToState : GT_State
{
    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        throw new System.NotImplementedException();
    }
}
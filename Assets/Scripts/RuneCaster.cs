using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneCaster : MonoBehaviour
{
    [SerializeField] private float _maxEnergy = 100;
    [SerializeField] private float _currEnergy = 100;
    [SerializeField] private string _castLayerName = "RuneSurface";

    [SerializeField] private Transform _castCheck = null;
    [SerializeField] private float _castDistance = 0.2f;

    private GameObject _runeSurface = null;

    private void Awake()
    {
        Debug.Assert(_castCheck != null, "_castCheck is not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        _runeSurface = CheckCast();
    }

    private void OnInteract(InputValue inputValue)
    {
        if (_runeSurface != null)
        {
            Debug.Log("On Interact", _runeSurface);
        }
    }

    private GameObject CheckCast()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer(_castLayerName);
        RaycastHit hit;

        Ray ray = new Ray(_castCheck.transform.position, _castCheck.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, _castDistance, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            return hit.transform.gameObject;
        }

        return null;
    }

    public void AddEnergy(float energyDelta)
    {
        _currEnergy = Mathf.Clamp(_currEnergy + energyDelta, 0, _maxEnergy);
    }
}

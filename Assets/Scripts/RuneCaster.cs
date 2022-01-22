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
    [SerializeField] private Transform _floorCheck = null;
    [SerializeField] private float _castDistance = 0.2f;
    [SerializeField] private float _floorCastDistance = 0.1f;

    [SerializeField] private GameObject _floorRune = null;

    private RuneSurface _nearbyRuneSurface = null;

    private void Awake()
    {
        Debug.Assert(_castCheck != null, "_castCheck is not assigned!");
        Debug.Assert(_floorCheck != null, "_floorCheck is not assigned!");
        Debug.Assert(_floorRune != null, "_floorRune is not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        // Rune Surface Cast
        _nearbyRuneSurface = CheckCast();
    }

    private void OnInteract(InputValue inputValue)
    {
        // Rune Surface Cast
        if (_nearbyRuneSurface != null)
        {
            Debug.Log("On Interact", _nearbyRuneSurface);
            _nearbyRuneSurface.SpawnRune();
        }
        else
        {
            // Rune Floor Cast
            TryFloorCast();
        }
    }

    private RuneSurface CheckCast()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer(_castLayerName);
        RaycastHit hit;

        Ray ray = new Ray(_castCheck.transform.position, _castCheck.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, _castDistance, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (_nearbyRuneSurface != null && _nearbyRuneSurface.gameObject == hit.collider.gameObject)
            {
                return _nearbyRuneSurface;
            }
            else
            {
                return hit.collider.GetComponent<RuneSurface>();
            }
        }

        return null;
    }

    private void TryFloorCast()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("RuneFloor");
        Ray ray = new Ray(_floorCheck.transform.position, _floorCheck.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        RaycastHit hit;

        Debug.Log("Try floor cast");

        if (Physics.Raycast(ray, out hit, _floorCastDistance, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            Instantiate(_floorRune, transform.position, Quaternion.identity);
        }
    }

    public void AddEnergy(float energyDelta)
    {
        _currEnergy = Mathf.Clamp(_currEnergy + energyDelta, 0, _maxEnergy);
    }
}

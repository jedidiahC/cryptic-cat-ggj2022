using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneCaster : MonoBehaviour
{
    public event System.Action<RuneStructure> OnCast = delegate { };
    public event System.Action<RuneStructure> OnStopCasting = delegate { };

    [SerializeField] private float _maxEnergy = 100;
    [SerializeField] private float _currEnergy = 100;
    [SerializeField] private string _castLayerName = "RuneSurface";

    [SerializeField] private Transform _castCheck = null;
    [SerializeField] private Transform _floorCheck = null;
    [SerializeField] private float _castDistance = 0.2f;
    [SerializeField] private float _floorCastDistance = 0.1f;

    [SerializeField] private GameObject _floorRune = null;
    [SerializeField] private Vector3 _runePlantOffset = Vector3.zero;

    [SerializeField] private float _energyDrainRate = 0.5f;
    [SerializeField] private float _energyRegenRate = 0.3f;

    [SerializeField] private CharacterMovement _characterMovement = null;

    [Header("Inputs")]
    [SerializeField] private Key _castKey = Key.E;
    [SerializeField] private List<RuneStructure> _runeStructures = null;

    private RuneSurface _nearbyRuneSurface = null;
    private Keyboard _keyboard;
    private bool _isCasting = false;
    private bool _hasCast = false;
    private RuneStructure _selectedRuneStructure;

    public float CurrEnergy { get { return _currEnergy; } }
    public float CurrEnergyNormalized { get { return _currEnergy / _maxEnergy; } }
    public bool IsCasting { get { return _isCasting; } }

    private void Awake()
    {
        Debug.Assert(_castCheck != null, "_castCheck is not assigned!");
        Debug.Assert(_floorCheck != null, "_floorCheck is not assigned!");
        Debug.Assert(_floorRune != null, "_floorRune is not assigned!");

        _keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        // Rune Surface Cast
        _nearbyRuneSurface = CheckCast();
        _isCasting = Keyboard.current[_castKey].isPressed;
        _selectedRuneStructure = _runeStructures[0];

        if (_isCasting && !_hasCast && IsOnFloor())
        {
            Casting();
            _characterMovement.SetDisabled(true);
        }
        else
        {
            AddEnergy(_energyRegenRate * Time.deltaTime);
        }

        if (!_isCasting)
        {
            _hasCast = false;
            _characterMovement.SetDisabled(false);
            OnStopCasting(_selectedRuneStructure);
        }
    }

    private void Casting()
    {
        OnCast(_selectedRuneStructure);
        AddEnergy(-_energyDrainRate * Time.deltaTime);

        if (_currEnergy > 0.1f)
        {
            if (AreAllKeysHeld(_selectedRuneStructure))
            {
                SuccessfulCast();
            }
        }
        else
        {
            _hasCast = true;
            OnStopCasting(_selectedRuneStructure);
        }
    }

    private bool AreAllKeysHeld(RuneStructure runeStructure)
    {
        for (int i = 0; i < runeStructure.keyCombinations.Length; i++)
        {
            if (!_keyboard[runeStructure.keyCombinations[i]].isPressed)
            {
                return false;
            }
        }
        return true;
    }

    private void SuccessfulCast()
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

        _hasCast = true;
        OnStopCasting(_selectedRuneStructure);
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
        if (IsOnFloor())
        {
            Instantiate(_floorRune, transform.position + _runePlantOffset, Quaternion.identity);
        }
    }

    private bool IsOnFloor()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("RuneFloor");
        Ray ray = new Ray(_floorCheck.transform.position, _floorCheck.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        RaycastHit hit;

        // Debug.Log("Try floor cast");
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        return Physics.Raycast(ray, out hit, _floorCastDistance, layerMask);
    }

    public void AddEnergy(float energyDelta)
    {
        _currEnergy = Mathf.Clamp(_currEnergy + energyDelta, 0, _maxEnergy);
    }
}

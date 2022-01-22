using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMeter : MonoBehaviour
{
    [SerializeField] private Image _energyMeter = null;
    [SerializeField] private RuneCaster _runeCaster = null;

    private void Awake()
    {
        Debug.Assert(_energyMeter != null, "_energyMeter is not assigned!");
        Debug.Assert(_runeCaster != null, "_runeCaster is not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        _energyMeter.fillAmount = _runeCaster.CurrEnergyNormalized;
    }
}

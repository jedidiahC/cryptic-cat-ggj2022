using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepMeter : MonoBehaviour
{
    [SerializeField] private Image _sleepMeter = null;
    [SerializeField] private Target _target = null;

    private void Awake()
    {
        Debug.Assert(_sleepMeter != null, "sleepMeter is not assigned!");
        Debug.Assert(_target != null, "_target is not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        _sleepMeter.fillAmount = _target.GetSleepLvlNormalized();
    }
}

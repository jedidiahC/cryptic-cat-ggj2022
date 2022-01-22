using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RuneCastUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel = null;
    [SerializeField] private TextMeshProUGUI[] _letters = null;
    [SerializeField] private Color _unselected = Color.black;
    [SerializeField] private Color _selected = Color.green;
    [SerializeField] private RuneCaster _runeCaster = null;

    private RuneStructure _runeStructure;
    private bool _isCasting = false;

    private void Awake()
    {
        Debug.Assert(_panel != null, "_panel is not assigned!");
        Debug.Assert(_letters != null && _letters.Length > 0, "_letters are not assigned!");
        Debug.Assert(_runeCaster != null, "_runeCaster is not assigned!");

        _runeCaster.OnCast += OnCast;
        _runeCaster.OnStopCasting += OnStopCasting;

        _panel.SetActive(false);
    }

    private void OnCast(RuneStructure runeStruct)
    {
        if (_letters.Length != runeStruct.keyCombinations.Length) { return; }
        _panel.SetActive(true);

        for (int i = 0; i < _letters.Length; i++)
        {
            _letters[i].text = runeStruct.keyCombinations[i].ToString();
        }

        _isCasting = true;
        _runeStructure = runeStruct;
    }

    private void OnStopCasting(RuneStructure runeStruct)
    {
        _panel.SetActive(false);
        _isCasting = false;
    }

    private void Update()
    {
        if (_isCasting)
        {
            transform.localScale = transform.parent.localScale;
            for (int i = 0; i < _runeStructure.keyCombinations.Length; i++)
            {
                _letters[i].color = Keyboard.current[_runeStructure.keyCombinations[i]].isPressed ? _selected : _unselected;
            }
        }
    }
}

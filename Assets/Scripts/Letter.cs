using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _letterText = null;

    private void Awake()
    {
        Debug.Assert(_letterText != null, "_letterText is not assigned!");
    }

    public void SetText(string text)
    {
        _letterText.text = text;
    }

    public void SetVisible(bool isVisible)
    {
        _letterText.enabled = isVisible;
    }

}

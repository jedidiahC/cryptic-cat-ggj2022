using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public UnityEvent onOpenMainMenu;

    private void Start()
    {
        onOpenMainMenu.Invoke();
    }
}

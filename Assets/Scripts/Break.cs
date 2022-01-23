using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    [SerializeField] private Animator _breakAnimator = null;

    public void OnBreakStart()
    {
        _breakAnimator.gameObject.SetActive(true);
        _breakAnimator.SetTrigger("breakStart");
    }

    public void OnBreakEnd()
    {
        Debug.Log("On break end");
        _breakAnimator.SetTrigger("breakEnd");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Collider _collider = null;

    public bool CanInteract { get { return _canInteract; } }

    protected bool _canInteract = false;

    public abstract bool Interact(InteractData interactData);

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            _canInteract = true;
            character.RegisterInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            _canInteract = false;
            character.UnregisterInteractable(this);
        }
    }
}

public struct InteractData
{
    public Character Character;
    public Vector3 FaceVec;
}
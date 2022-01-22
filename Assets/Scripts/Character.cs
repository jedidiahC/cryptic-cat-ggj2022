using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb = null;

    [Header("Debug")]
    [SerializeField] private List<Interactable> _interactables = new List<Interactable>();

    private void Awake()
    {
        Debug.Assert(_rb != null, "_rb is not assigned!");
    }


    public void RegisterInteractable(Interactable interactable)
    {
        _interactables.Add(interactable);
    }

    public void UnregisterInteractable(Interactable interactable)
    {
        _interactables.Remove(interactable);
    }

    public void ClearInteractables()
    {
        _interactables.Clear();
    }

    // PlayerInput On Interact callback function.
    public void OnInteract(InputValue inputValue)
    {
        Interact();
    }

    public void Interact()
    {
        if (_interactables.Count < 1 || _interactables[0] == null) return;

        Interactable selectedInteractable = _interactables[0];
        InteractData interactData = new InteractData();

        interactData.FaceVec = selectedInteractable.transform.position - transform.position;
        interactData.Character = this;

        selectedInteractable.Interact(interactData);
    }
}

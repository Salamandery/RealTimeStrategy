using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    private Camera mainCamera;
    private List<UnitBehaviour> selectedUnits = new List<UnitBehaviour>();
    public List<UnitBehaviour> GetSelectedUnits()
    {
        return selectedUnits;
    }
    [SerializeField]
    private LayerMask layerMask;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DeselectUnits();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void DeselectUnits()
    {
        foreach (UnitBehaviour selected in selectedUnits)
        {
            selected.Deselect();
        }

        selectedUnits.Clear();
    }
    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        bool rayCondition = Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            layerMask
        );
        if (!rayCondition) { return; }

        bool unitCondition = hit.collider.TryGetComponent<UnitBehaviour>(out UnitBehaviour unitBehaviour);
        if (!unitCondition) { return; }

        if (!unitBehaviour.hasAuthority) { return; }

        selectedUnits.Add(unitBehaviour);

        foreach (UnitBehaviour selected in selectedUnits)
        {
            selected.Select();
        }
    }
}

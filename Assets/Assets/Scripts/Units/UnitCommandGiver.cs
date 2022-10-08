using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField]
    private UnitSelectionHandler unitSelectionHandler;
    [SerializeField]
    private LayerMask layerMask;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        //unitSelectionHandler
    }

    void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        bool rayCondition = Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            layerMask
        );

        if (!rayCondition) { return; }

        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
        foreach (UnitBehaviour unit in unitSelectionHandler.GetSelectedUnits())
        {
            unit.GetUnitsMovements().CmdMove(point);
        }
    }
}

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

        if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }
            TryTarget(target);
            return;
        }

        TryMove(hit.point);
    }

    private void TryTarget(Targetable targetable)
    {
        foreach (UnitBehaviour unit in unitSelectionHandler.GetSelectedUnits())
        {
            unit.GetTargeter().CmdSetTarget(targetable.gameObject);
        }
    }

    private void TryMove(Vector3 point)
    {
        foreach (UnitBehaviour unit in unitSelectionHandler.GetSelectedUnits())
        {
            unit.GetUnitsMovements().CmdMove(point);
        }
    }
}

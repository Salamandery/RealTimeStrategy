using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField]
    private RectTransform unitSelectionArea;
    [SerializeField]
    private LayerMask layerMask;
    private NetworkPlayerBehaviour player;
    private Camera mainCamera;
    private Vector2 startPosition;
    private List<UnitBehaviour> selectedUnits = new List<UnitBehaviour>();
    public List<UnitBehaviour> GetSelectedUnits()
    {
        return selectedUnits;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<NetworkPlayerBehaviour>();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(
            Mathf.Abs(areaWidth),
            Mathf.Abs(areaHeight)
        );
        unitSelectionArea.anchoredPosition = startPosition + new Vector2(
            areaWidth / 2,
            areaHeight / 2
        );

    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (UnitBehaviour selected in selectedUnits)
            {
                selected.Deselect();
            }

            selectedUnits.Clear();
        }

        unitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }
    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
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

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (UnitBehaviour unitBehaviour in player.GetPlayerUnits())
        {
            if (selectedUnits.Contains(unitBehaviour)) { continue; }
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unitBehaviour.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x &&
                screenPosition.y > min.y && screenPosition.y < max.y
            )
            {
                selectedUnits.Add(unitBehaviour);
                unitBehaviour.Select();
            }
        }
    }
}

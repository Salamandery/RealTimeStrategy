using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class UnitsMovements : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    private Camera mainCamera;

    #region Server
    [Command]
    public void CmdMove(Vector3 position)
    {
        bool NavHitCondition = NavMesh.SamplePosition(
            position,
            out NavMeshHit hit,
            1f,
            NavMesh.AllAreas
        );
        if (!NavHitCondition) { return; }

        agent.SetDestination(hit.position);
    }
    #endregion
    #region Client
    /*   public override void OnStartAuthority()
       {
           mainCamera = Camera.main;
       }

       private void OnMoveAction()
       {
           if (!hasAuthority) { return; }
           if (!Input.GetMouseButtonDown(1)) { return; }

           Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

           if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

           CmdMove(hit.point);
       }

       void Update()
       {
           OnMoveAction();
       }*/
    #endregion
}

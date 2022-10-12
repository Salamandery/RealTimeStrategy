using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class UnitsMovements : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Targeter targeter;
    [SerializeField]
    private float chaseRange = 10f;

    #region Server
    [ServerCallback]
    void Update()
    {
        hasTargetToChase();
        if (!agent.hasPath) { return; }
        if (agent.remainingDistance > agent.stoppingDistance) { return; }

        agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();

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
    private void hasTargetToChase()
    {
        Targetable target = targeter.GetTarget();

        if (target != null)
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;
            if (distance > chaseRange * chaseRange)
            {
                agent.SetDestination(target.transform.position);
            }
            else if (agent.hasPath)
            {
                agent.ResetPath();
            }

            return;
        }
    }
    #endregion
}

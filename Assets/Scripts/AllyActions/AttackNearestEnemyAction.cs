using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackNearestEnemy", story: "Attack the nearest enemy threatening the ally", category: "Action", id: "be413a932bc1887523ea468096a4cf38")]
public partial class AttackNearestEnemyAction : Action
{
    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<NavMeshAgent> navAgent;
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;
    [SerializeReference]
    public BlackboardVariable<float> attackRange;
    [SerializeReference]
    public BlackboardVariable<float> attackCooldown;
    [SerializeReference]
    public BlackboardVariable<float> lastAttackTime;
    [SerializeReference]
    public BlackboardVariable<float> damageAmount;
    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        GameObject enemy = perception.Value.GetNearestEnemy();

        if (enemy == null)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, enemy.transform.position);

        if (distance > attackRange)
        {
            navAgent.Value.isStopped = false;
            navAgent.Value.SetDestination(enemy.transform.position);
            return Status.Running;
        }
        else //In range so can perform attack
        {
            navAgent.Value.isStopped = true;

            Vector3 direction = (enemy.transform.position - allyTransform.Value.position).normalized;
            direction.y = 0; //TODO maybe wrong
            if(direction != Vector3.zero)
            {
                allyTransform.Value.rotation = Quaternion.LookRotation(direction);
            }

            if(Time.time - lastAttackTime >= attackCooldown)
            {
                PerformAttack(enemy);
                lastAttackTime.Value = Time.time;
            }

            return Status.Running;
        }
    }

    void PerformAttack(GameObject enemy)
    {
        Health targetHealth = enemy.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            Debug.Log($"[Ally] Attacking {enemy.name} for {damageAmount.Value} damage");
        }
        else
        {
            Debug.Log($"[Ally] Attacking nearest eneny: {enemy.name}");
        }
    }
}


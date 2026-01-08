using UnityEngine;


public enum AllyCommand
{
    None,
    Wait,
    AttackTarget,
    Interact,
    Flee,
    DefendSelf,
    DefendPlayer,
    AssistPlayer
}


public class AllyCommandData : MonoBehaviour
{
    [Header("Current Command State")]
    public AllyCommand currentCommand = AllyCommand.None; //Base command - following player
    public AllyCommand pausedCommand = AllyCommand.None; //Command that is paused that will resume after critical actions occur
    public GameObject targetEnemy; //Target to attack when command is AttackTarget
    public GameObject targetObject; //Object to interact with when command is Interact
    public Vector3 waitPosition; //Position to wait at when command is Wait

    public bool isSelfUnderAttack = false; //Is the ally under attack
    public bool isPlayerUnderAttack = false; //Is the player under attack
    public bool isAreaSafe = true;

    public float currentHealth = 100f;
    public float maxHealth = 100f;


    public Transform playerTransform;

    public GameObject[] nearbyEnemies = new GameObject[0];
    public GameObject[] nearbyHealthPickups = new GameObject[0];

    public GameObject targetPickup = null;
    public Vector3 followPosition;
    public float followDistance;

    public float HealthPercentage => currentHealth / maxHealth;

    public bool IsHealthCritical(float threshold = 0.25f) //Check if health is critical
    {
        return HealthPercentage < threshold;
    }

    public bool IsPlayerHealthLow(float treshhold = 0.5f)
    {
        return false; //TODO Add player health check
    }

    public void PauseCurrentCommand(AllyCommand newCommand)
    {
        pausedCommand = currentCommand;
        currentCommand = newCommand;
        Debug.Log($"[Ally] Paused {pausedCommand}, now executing {newCommand}");
    }

    public void ResumePausedCommand()
    {
        if (pausedCommand != AllyCommand.None)
        {
            currentCommand = pausedCommand;
            pausedCommand = AllyCommand.None;
            Debug.Log($"[Ally] Resumed command: {currentCommand}");
        }
        else
        {
            currentCommand = AllyCommand.None;
        }
    }

    public void ClearCommand()
    {
        currentCommand = AllyCommand.None;
        targetEnemy = null;
        targetObject = null;
        Debug.Log("[Ally] Command cleared");
    }

    public bool IsTargetEnemyValid()
    {
        return targetEnemy != null && targetEnemy.activeInHierarchy;
    }

    public bool IsTargetObjectValid()
    {
        return targetEnemy != null && targetObject.activeInHierarchy;
    }

    public Vector3 GetFollowPosition()
    {
        Vector3 followPos = playerTransform.position - playerTransform.forward * followDistance;
        return followPos;
    }

}

public static class CommandPriority
{
    public static int GetPriority(AllyCommand command)
    {
        switch (command)
        {
            case AllyCommand.Flee: return 5;
            case AllyCommand.DefendSelf: return 4;
            case AllyCommand.DefendPlayer: return 4;
            case AllyCommand.AttackTarget: return 3;
            case AllyCommand.Interact: return 2;
            case AllyCommand.Wait: return 2;
            case AllyCommand.AssistPlayer: return 1;
            case AllyCommand.None: return 0;
            default: return 0;
        }
    }

    public static bool CanOverride(AllyCommand current, AllyCommand incoming)
    {
        return GetPriority(incoming) > GetPriority(current);
    }

    public static bool TrySetCommand(AllyCommandData data, AllyCommand newCommand)
    {
        if(CanOverride(data.currentCommand, newCommand))
        {
            if(GetPriority(newCommand) >= 4 && data.currentCommand != AllyCommand.None)
            {
                data.PauseCurrentCommand(newCommand);
            }
            else
            {
                data.currentCommand = newCommand;
            }
            return true;
        }
        return false;
    }
}

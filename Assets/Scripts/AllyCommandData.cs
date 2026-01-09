using UnityEngine;


public enum AllyCommand //All the valid Allycommands
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
    public bool isAreaSafe = true; //Is the area safe

    public Transform playerTransform;
    public Health playerHealth;
    public Health selfHealth;

    public GameObject[] nearbyEnemies = new GameObject[0];
    public GameObject[] nearbyHealthPickups = new GameObject[0];

    public GameObject targetPickup = null;
    public Vector3 followPosition;
    public float followDistance;

    public float HealthPercentage => selfHealth.currentHealth / selfHealth.maxHealth;

    private void Awake()
    {
        selfHealth = GetComponent<Health>();
    }
    private void Update()
    {
        if(currentCommand == AllyCommand.AttackTarget && targetEnemy == null) //Ensures the ally doesnt get stuck when the commanded enemy dies.
        {
            Debug.Log("[Ally] Target destroyed, resuming follow");
            currentCommand = AllyCommand.None;
        }
    }
    public bool IsHealthCritical(float threshold = 0.25f) //Check if health is critical
    {
        return HealthPercentage < threshold;
    }

    public bool IsPlayerHealthLow(float treshhold = 0.5f) //Check if player health is low (healing needed)
    {
        if(playerHealth == null)
        {
            playerHealth = playerTransform.GetComponent<Health>();
        }
        return (playerHealth.currentHealth / playerHealth.maxHealth) <= treshhold;
    }

    public void PauseCurrentCommand(AllyCommand newCommand) //Save current command and set new command
    {
        pausedCommand = currentCommand;
        currentCommand = newCommand;
        Debug.Log($"[Ally] Paused {pausedCommand}, now executing {newCommand}");
    }

    public void ResumePausedCommand() //Resume the paused command
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

    public void ClearCommand() //Resets targets and clears command
    {
        currentCommand = AllyCommand.None;
        targetEnemy = null;
        targetObject = null;
        Debug.Log("[Ally] Command cleared");
    }

    public bool IsTargetEnemyValid() //Checks if target enemy is active and not null
    {
        return targetEnemy != null && targetEnemy.activeInHierarchy;
    }

    public bool IsTargetObjectValid() //Checks if target object is active and not null
    {
        return targetObject != null && targetObject.activeInHierarchy;
    }

    public Vector3 GetFollowPosition() //Gets a follow position near the player
    {
        Vector3 offset = transform.position - playerTransform.position;

        if(offset.magnitude > followDistance)
            return playerTransform.position + offset.normalized * followDistance;

        return transform.position;
        //Vector3 followPos = playerTransform.position - playerTransform.forward * followDistance;
        //return followPos;
    }

}

public static class CommandPriority //NOT USED
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

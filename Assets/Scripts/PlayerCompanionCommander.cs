using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerCompanionCommander : MonoBehaviour
{
    [SerializeField] CrosshairFeedback crosshair;
    [SerializeField] float interactCooldown = 1f;
    float currentInteractCooldown;

    public GameObject ally;

    public float interactionRange = 10f;

    public LayerMask interactableLayers;

    public Color validTargetColor = Color.green;
    public Color normalColor = Color.white;

    public bool showDebugRays = true;

    private Camera playerCamera;
    private AllyCommandData allyCommandData;
    private GameObject currentLookTarget;

    private void Start()
    {
        playerCamera = Camera.main;

        if(ally != null)
        {
            allyCommandData = ally.GetComponent<AllyCommandData>();
            if(allyCommandData == null)
            {
                Debug.LogError("Ally does not have AllyCommandData component");
            }
        }
        else
        {
            Debug.LogError("Ally rreference not set in PlayerCompanionCommander");
        }
    }

    private void Update()
    {
        DetectLookTarget();
        currentInteractCooldown = Mathf.Max(0, currentInteractCooldown - Time.deltaTime);
        
    }

    void DetectLookTarget() //Checks with a raycast what is being looked at and saves it.
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (showDebugRays)
        {
            Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.yellow);
        }

        if(Physics.Raycast(ray, out hit, interactionRange, interactableLayers))
        {
            currentLookTarget = hit.collider.gameObject;

            if(currentLookTarget != null)
            {
                crosshair.SetForTarget(currentLookTarget);
            }

        }
        else
        {
            currentLookTarget = null;
            crosshair.SetNormal();
        }
    }

    public void IssueCommand(CallbackContext context) //Called in the input system, issues commands to ally depending on what is looked at
    {
        if(currentInteractCooldown == 0)
        {
            if (currentLookTarget == null || allyCommandData == null)
                return;

            if (currentLookTarget.CompareTag("Enemy"))
            {
                IssueAttackCommand(currentLookTarget);
                crosshair.FlashCommandIssued();

            }
            else if (currentLookTarget.CompareTag("Interactable"))
            {
                IssueInteractCommand(currentLookTarget);
                crosshair.FlashCommandIssued();
            }
            else if (currentLookTarget == ally)
            {
                ToggleWaitCommand();
                crosshair.FlashCommandIssued();
            }
            else
            {
                Debug.Log("Looking at non-commandable object: " + currentLookTarget.name);
            }
            currentInteractCooldown = interactCooldown;
        }

    }

    void IssueAttackCommand(GameObject enemy) //Orders ally to attack the looked at enemy (needs to be in range still)
    {
        allyCommandData.currentCommand = AllyCommand.AttackTarget;
        allyCommandData.targetEnemy = enemy;
        allyCommandData.targetObject = null;

        Debug.Log($"[Commander] Issued attack command on: {enemy.name}");
    }

    void IssueInteractCommand(GameObject interactableObject) //Issues interact command to the ally to the targeted button
    {
        allyCommandData.currentCommand = AllyCommand.Interact;
        allyCommandData.targetObject = interactableObject;
        allyCommandData.targetEnemy = null;

        Debug.Log($"[Commander] Issued Interact command on: {interactableObject.name}");
    }

    void ToggleWaitCommand() //If the ally is looked at, commands it to wait, but this is lower priority then defending for example
    {
        if(allyCommandData.currentCommand == AllyCommand.Wait)
        {
            allyCommandData.currentCommand = AllyCommand.None;
            Debug.Log("[Commander] Ally resuming follow");
        }
        else
        {
            allyCommandData.currentCommand = AllyCommand.Wait;
            allyCommandData.waitPosition = ally.transform.position;
            Debug.Log("[Commander] Ally commanded to wait");
        }
    }

    public void CancelCurrentCommand() //Cancels currernt command and resets
    {
        if(allyCommandData != null)
        {
            allyCommandData.currentCommand = AllyCommand.None;
            allyCommandData.targetEnemy = null;
            allyCommandData.targetObject = null;
            Debug.Log("[Commander] Command cancelled");
        }
    }

    private void OnDrawGizmos() //Shows a ray in the editor for testing purposes
    {
        if (!showDebugRays || playerCamera == null) return;

        Gizmos.color = Color.cyan;
        Vector3 forward = playerCamera.transform.forward * interactionRange;
        Gizmos.DrawWireSphere(playerCamera.transform.position + forward, 0.5f);

        if(currentLookTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.transform.position, currentLookTarget.transform.position);
        }
    }
}


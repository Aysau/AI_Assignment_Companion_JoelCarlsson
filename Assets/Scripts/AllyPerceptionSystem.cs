using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AllyPerceptionSystem : MonoBehaviour
{
    public float enemyDetectionRange = 15f;
    public float healthPickupDetectionRange = 20f;
    public float safeAreaRange = 10f;
    public float perceptionUpdateRate = 0.2f;
    public LayerMask enemyLayer;
    public LayerMask healthPickupLayer;
    public float attackingDistance = 3f;
    public float playerThreatenDistance = 5f;

    private AllyCommandData commandData;
    private Transform playerTransform;
    private float perceptionTimer;

    private List<GameObject> detectedEnemies = new List<GameObject>();
    private List<GameObject> detectedHealthPickups = new List<GameObject>();

    private void Start()
    {
        commandData = GetComponent<AllyCommandData>();
        if(commandData == null)
        {
            Debug.LogError("AllyPerceptionSystem requires AllyCommandData component");
            enabled = false;
            return;
        }

        playerTransform = commandData.playerTransform;

        if(playerTransform == null)
        {
            Debug.LogError("Player transform not set in AllyCommandData");
        }
    }

    private void Update() //Ensures we don't hurt performance too much by having a cooldown on the perception checks
    {
        perceptionTimer += Time.deltaTime;

        if(perceptionTimer >= perceptionUpdateRate)
        {
            UpdatePerception();
            perceptionTimer = 0f;
        }

        
    }

    void UpdatePerception()
    {
        CleanupDestroyedObjects();

        DetectEnemies();
        DetectHealthPickups();
        CheckThreats();
        UpdateAreaSafety();
    }

    private void UpdateAreaSafety() //Checks if area is safe and updates variable
    {
        commandData.isAreaSafe = true;

        foreach(GameObject enemy in detectedEnemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if(distance <= safeAreaRange)
            {
                commandData.isAreaSafe = false;
                break;
            }
        }
    }

    private void CheckThreats() //Checks for threats either near player or the ally
    {
        commandData.isSelfUnderAttack = false;
        commandData.isPlayerUnderAttack = false;

        foreach(GameObject enemy in detectedEnemies)
        {
            if (enemy == null) continue;

            float distanceToSelf = Vector3.Distance(transform.position, enemy.transform.position);
            float distanceToPlayer = playerTransform != null ? Vector3.Distance(playerTransform.position, enemy.transform.position) : Mathf.Infinity;

            if(distanceToSelf <= attackingDistance)
            {
                commandData.isSelfUnderAttack = true;
            }

            if(distanceToPlayer <= playerThreatenDistance)
            {
                commandData.isPlayerUnderAttack = true;
            }
        }
    }

    private void DetectHealthPickups() //Detects and saves nearby health pickups
    {
        detectedHealthPickups.Clear();

        Collider[] pickupCOlliders = Physics.OverlapSphere(transform.position, healthPickupDetectionRange, healthPickupLayer);

        foreach(Collider collider in pickupCOlliders)
        {
            if (collider.gameObject.activeInHierarchy)
            {
                detectedHealthPickups.Add(collider.gameObject);
            }
        }

        commandData.nearbyHealthPickups = detectedHealthPickups.ToArray();
    }

    private void DetectEnemies() //Detects and saves nearby enemies
    {
        detectedEnemies.Clear();
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, enemyDetectionRange, enemyLayer);

        foreach(Collider collider in enemyColliders)
        {
            if (collider.gameObject.activeInHierarchy)
            {
                detectedEnemies.Add(collider.gameObject);
            }
        }

        commandData.nearbyEnemies = detectedEnemies.ToArray();
    }

    public GameObject GetNearestEnemy() //Gets the nearest enemy
    {
        if (detectedEnemies.Count == 0) return null;

        return detectedEnemies.Where(IsValid).OrderBy(e => Vector3.Distance(transform.position , e.transform.position)).FirstOrDefault();
        //return detectedEnemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
    }

    public GameObject GetPlayerAttacker() //Gets the player attacker
    {
        if(detectedEnemies.Count == 0 || playerTransform == null) return null;


        return detectedEnemies.Where(IsValid).OrderBy(e => Vector3.Distance(playerTransform.position, e.transform.position)).FirstOrDefault();
    }

    public GameObject GetNearestHealthPickup() //Gets the nearest health pickup
    {
        if (detectedHealthPickups.Count == 0) return null;

        return detectedHealthPickups.Where(IsValid).OrderBy(p => Vector3.Distance(transform.position, p.transform.position)).FirstOrDefault();
    }

    private static bool IsValid(GameObject obj) //Checks if an object is valid
    {
        return obj != null && obj.activeInHierarchy;
    }

    private void CleanupDestroyedObjects() //Cleans up the lists to ensure no invalid objects to prevent null or similar
    {
        detectedEnemies.RemoveAll(e => !IsValid(e));
        detectedHealthPickups.RemoveAll(p => !IsValid(p));
    }


    private void OnDrawGizmosSelected() //Show ranges in the editor to help test
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healthPickupDetectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, safeAreaRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackingDistance);
    }
}

using UnityEngine;

public class GuardSensors : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private LayerMask targetLayers;
    [Header("View")]
    [SerializeField] private float viewDistance = 10f;
    [Range(1f, 180f)]
    [SerializeField] private float viewAngleDegrees = 90f;
    [Header("Line of Sight")]
    [SerializeField] private Transform eyes;
    [SerializeField] private LayerMask occlusionMask = ~0; //everything by default

    public float ViewDistance => viewDistance;
    public float ViewAngleDegrees => viewAngleDegrees;
    private Transform EyesTransform => eyes != null ? eyes :
    transform;

    public bool TrySenseTarget(out GameObject target, out
    Vector3 lastKnownPosition, out bool hasLineOfSight)
    {

        target = null;
        lastKnownPosition = default;
        hasLineOfSight = false;
        Vector3 eyePos = EyesTransform.position;
        Collider[] hits = Physics.OverlapSphere(eyePos, viewDistance, targetLayers);

        foreach (Collider collider in hits)
        {
            Transform candidate = collider.transform;
            Vector3 toTarget = candidate.position - eyePos;
            float dist = toTarget.magnitude;

            Vector3 dir = toTarget / Mathf.Max(dist, 0.0001f);
            float angle = Vector3.Angle(EyesTransform.forward, dir);

            if (angle > viewAngleDegrees * 0.5f)
            {
                continue;
            }

            if (Physics.Raycast(eyePos, dir, out RaycastHit rayHit, dist, occlusionMask))
            {
                if (rayHit.transform != candidate) continue;
            }

            target = candidate.gameObject;
            lastKnownPosition = candidate.position;
            hasLineOfSight = true;
            return true;
        }

        return false;




    }
}


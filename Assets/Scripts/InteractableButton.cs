using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private bool singleUse = false;

    public UnityEvent onPressed;

    private bool hasBeenUsed;

    public void Press()
    {
        if (singleUse && hasBeenUsed) return;

        hasBeenUsed = true;
        onPressed?.Invoke();
    }
}

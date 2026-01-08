using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    public void OnDeath()
    {
        Debug.Log(gameObject.name + " has been defeated");
        Destroy(gameObject);
    }
}

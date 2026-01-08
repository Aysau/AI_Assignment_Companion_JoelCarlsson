using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] GameObject bridge;

    public void ShowBridge()
    {
        bridge.SetActive(true);
    }
    public void HideBridge()
    {
        bridge.SetActive(false);
    }
}

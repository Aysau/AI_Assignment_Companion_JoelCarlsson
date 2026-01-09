using UnityEngine;

public class BridgeController : MonoBehaviour //Script for letting buttons enable/disable objects
{
    [SerializeField] GameObject bridge;
    [SerializeField] bool startEnabled = true;
    private void Awake()
    {
        if(!startEnabled)
        HideBridge();
    }
    public void ShowBridge()
    {
        bridge.SetActive(true);
    }
    public void HideBridge()
    {
        bridge.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairFeedback : MonoBehaviour
{
    [SerializeField] Image crosshairImage;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color enemyColor = Color.red;
    public Color interactableColor = Color.green;
    public Color allyColor = Color.cyan;
    public Color commandIssuedColor = Color.yellow;

    Coroutine flashRoutine;

    public void SetNormal()
    {
        crosshairImage.color = normalColor;
    }

    public void SetForTarget(GameObject target)
    {
        if(flashRoutine == null)
        {
            if (target.CompareTag("Enemy"))
                crosshairImage.color = enemyColor;
            else if (target.CompareTag("Interactable"))
                crosshairImage.color = interactableColor;
            else if (target.CompareTag("Ally"))
                crosshairImage.color = allyColor;
            else
                crosshairImage.color = normalColor;
        }

    }

    public void FlashCommandIssued(float duration = 0.15f)
    {
        if(flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine(duration));
    }

    IEnumerator FlashRoutine(float duration)
    {
        Color original = normalColor;
        crosshairImage.color = commandIssuedColor;
        yield return new WaitForSeconds(duration);
        crosshairImage.color = original;
        flashRoutine = null;
    }
}

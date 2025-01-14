using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private GameObject[] objectsToFade;

    public void TriggerFade(float fadeDuration, float blackDuration)
    {
        StartCoroutine (Fade(fadeDuration, blackDuration));
    }

    private IEnumerator Fade(float fadeDuration, float blackDuration)
    {
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / fadeDuration;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, lerpValue);

            // Fade Objects
            foreach (var obj in objectsToFade)
            {
                CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1 - lerpValue;
                }

                // deactivate objects when they are fully faded
                if (lerpValue >= 1)
                {
                    obj.SetActive(false);
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(blackDuration);

        while (lerpValue > 0) 
        {
            lerpValue -= Time.deltaTime / fadeDuration;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, lerpValue);
            yield return null;
        }
    }
}

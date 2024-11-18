using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{


    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackDuration = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

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

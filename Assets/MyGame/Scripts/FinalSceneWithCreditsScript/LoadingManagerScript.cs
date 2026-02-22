using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManagerScript : MonoBehaviour
{
    [Header("Black Screen")]
    public Image blackScreenImage;
    public float fadeDuration = 2f;

    void Start()
    {

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BlackScreenBrunnen()
    {
        StartCoroutine(FadeBlackScreen());
    }

    IEnumerator FadeBlackScreen()
    {
        float elapsed = 0f;
        Color color = blackScreenImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / fadeDuration);
            blackScreenImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        blackScreenImage.color = new Color(color.r, color.g, color.b, 0f);
        blackScreenImage.gameObject.SetActive(false);
    }
}
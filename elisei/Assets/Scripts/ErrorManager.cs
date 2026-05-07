using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorManager : MonoBehaviour
{
    public Text errorText;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Получаем компонент Canvas Group с объекта текста
        canvasGroup = errorText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Если забыл добавить в инспекторе — добавим сами
            canvasGroup = errorText.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0; // Изначально невидим
    }

    public void ShowError(string message, float delay)
    {
        StopAllCoroutines();
        StartCoroutine(FadeError(message, delay));
    }

    private IEnumerator FadeError(string message, float delay)
    {
        errorText.text = message;
        float fadeDuration = 0.5f; // Длительность анимации появления/исчезновения

        // 1. Появление (Fade In)
        float currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        // 2. Ожидание
        yield return new WaitForSeconds(delay);

        // 3. Исчезновение (Fade Out)
        currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, currentTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
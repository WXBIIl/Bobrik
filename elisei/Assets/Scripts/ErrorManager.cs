using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorManager : MonoBehaviour
{
    [Header("Настройки UI")]
    public CanvasGroup errorPanelGroup; // Сюда перетащи объект ErrorPanel
    public Text errorText;              // Сюда перетащи текст внутри панели

    private void Awake()
    {
        // В начале игры делаем панель полностью прозрачной
        if (errorPanelGroup != null) errorPanelGroup.alpha = 0;
    }

    public void ShowError(string message, float delay)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(message, delay));
    }

    private IEnumerator FadeRoutine(string message, float delay)
    {
        errorText.text = message;
        float fadeTime = 0.4f;

        // Плавное появление панели и текста
        yield return StartCoroutine(Fade(0, 1, fadeTime));

        // Ждем
        yield return new WaitForSeconds(delay);

        // Плавное исчезновение
        yield return StartCoroutine(Fade(1, 0, fadeTime));
    }

    private IEnumerator Fade(float start, float end, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            errorPanelGroup.alpha = Mathf.Lerp(start, end, elapsed / time);
            yield return null;
        }
        errorPanelGroup.alpha = end;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Base : MonoBehaviour
{
    public int hp = 100;
    public Image myImage;
    public Sprite normalSprite;
    public Sprite blockSprite;

    public float jumpHeight = 60f;
    public float moveDistance = 50f;

    public IEnumerator AnimateJump()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0, jumpHeight, 0);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(targetPos, startPos, t);
            yield return null;
        }
    }

    public IEnumerator AnimateAttack(bool isBoss)
    {
        Vector3 startPos = transform.localPosition;
        float dir = isBoss ? 1f : -1f; 
        Vector3 targetPos = startPos + new Vector3(moveDistance * dir, 0, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        transform.localPosition = startPos;
    }

    public IEnumerator FlashRed()
    {
        myImage.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        myImage.color = Color.white;
    }
}
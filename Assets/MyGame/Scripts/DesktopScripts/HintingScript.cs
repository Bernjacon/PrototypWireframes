using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HintingScript : MonoBehaviour
{
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float switchInterval = 1f;

    private Button button;
    private Image image;
    private Coroutine blinkRoutine;

    private Sprite originalNormalSprite;
    private Sprite highlightedSprite;
    private Sprite pressedSprite;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = button.targetGraphic as Image;

        if (image == null) return;

        originalNormalSprite = image.sprite;

        SpriteState spriteState = button.spriteState;
        highlightedSprite = spriteState.highlightedSprite;
        pressedSprite = spriteState.pressedSprite;
    }

    public void ActivateBlinkingTimer()
    {
        StopBlinkingOnly();

        if (image == null || highlightedSprite == null || pressedSprite == null)
            return;

        image.overrideSprite = null;
        image.sprite = originalNormalSprite;

        blinkRoutine = StartCoroutine(Blink());
    }

    public void DeactivateBlinking()
    {
        StopBlinkingOnly();

        if (image != null)
        {
            image.overrideSprite = highlightedSprite;
            image.sprite = originalNormalSprite;
        }
    }

    private void StopBlinkingOnly()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(startDelay);

        bool showPressed = false;

        while (true)
        {
            image.overrideSprite = showPressed ? pressedSprite : highlightedSprite;
            showPressed = !showPressed;

            yield return new WaitForSeconds(switchInterval);
        }
    }
}
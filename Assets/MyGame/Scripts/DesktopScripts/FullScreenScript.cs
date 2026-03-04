using UnityEngine;
using UnityEngine.UI;

public class FullScreenScript : MonoBehaviour
{
    [Header("Scaling")]
    [SerializeField] float windowedScale = 0.68f;
    [SerializeField] float fullscreenScale = 1f;

    [Header("Button Visual")]
    [SerializeField] Image toggleButtonImage;
    [SerializeField] Sprite fullscreenSprite;
    [SerializeField] Sprite windowedSprite;

    bool isFullscreen = true;

    public void ToggleScale()
    {
        isFullscreen = !isFullscreen;

        float targetScale = isFullscreen ? fullscreenScale : windowedScale;
        transform.localScale = Vector3.one * targetScale;

        UpdateButtonVisual();
    }

    void UpdateButtonVisual()
    {
        if (toggleButtonImage == null)
            return;

        toggleButtonImage.sprite = isFullscreen
            ? windowedSprite
            : fullscreenSprite;
    }

    private void Awake()
    {
        UpdateButtonVisual();
    }
}
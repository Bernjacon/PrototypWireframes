using UnityEngine;
using UnityEngine.UI;

public class GalleryScript : MonoBehaviour
{
    [SerializeField] private Image imageToShow;
    [SerializeField] private Sprite[] imageCollection;
    [SerializeField] private Button[] buttonsImage;
    [SerializeField] private int index;
    [SerializeField] private GameObject parentGalleryOverview;
    [SerializeField] private GameObject parentViewImage;

    private void Start()
    {
        for (int i = 0; i < buttonsImage.Length; i++)
        {
            if (i < imageCollection.Length && buttonsImage[i] != null)
            {
                Image img = buttonsImage[i].GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = imageCollection[i];
                    img.preserveAspect = true;
                }
            }
        }
    }

    public void CloseDetailView()
    {
        parentGalleryOverview.SetActive(true);
        parentViewImage.SetActive(false);
    }

    public void GoNextOrPrevious(bool goToNext)
    {
        if (imageCollection == null || imageCollection.Length == 0) return;

        if (goToNext)
        {
            index++;
            if (index >= imageCollection.Length)
                index = 0;
        }
        else
        {
            index--;
            if (index < 0)
                index = imageCollection.Length - 1;
        }

        UpdateImage();
    }

    public void OpenThisImage(int x)
    {
        if (x < 0 || x >= imageCollection.Length) return;

        parentViewImage.SetActive(true);
        parentGalleryOverview.SetActive(false);
        index = x;
        UpdateImage();
    }

    void UpdateImage()
    {
        if (imageToShow == null) return;
        if (imageCollection == null || imageCollection.Length == 0) return;
        if (index < 0 || index >= imageCollection.Length) return;

        Sprite sprite = imageCollection[index];
        imageToShow.sprite = sprite;

        if (parentViewImage != null && parentViewImage.activeSelf)
        {
            RectTransform imageRect = imageToShow.rectTransform;
            RectTransform parentRect = imageToShow.transform.parent.GetComponent<RectTransform>();

            if (sprite == null || imageRect == null || parentRect == null) return;

            float targetHeight = parentRect.rect.height;
            float aspect = sprite.rect.width / sprite.rect.height;
            float targetWidth = targetHeight * aspect;

            imageRect.sizeDelta = new Vector2(targetWidth, targetHeight);
        }
    }
}
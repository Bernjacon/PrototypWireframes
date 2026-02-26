using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GalleryScript : MonoBehaviour
{
    [SerializeField] Image imageToShow;
    [SerializeField] Sprite[] imageCollection;
    [SerializeField] Button[] buttonsImage;
    [SerializeField] int index;
    [SerializeField] GameObject parentGalleryOverview;
    [SerializeField] GameObject parentViewImage;
    void Start()
    {
        for (int i = 0; i < buttonsImage.Length; i++)
        {
            if (i < imageCollection.Length)
            {
                Image img = buttonsImage[i].GetComponent<Image>();
                img.sprite = imageCollection[i];
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
        if (goToNext)
        {
            index++;
            if (index > imageCollection.Length - 1)
            {
                index = 0;
            }
            UpdateImage();
        }
        else
        {
            index--;
            if (index < 0)
            {
                index = imageCollection.Length -1;
            }
            UpdateImage();
        }

    }
    public void OpenThisImage(int x)
    {
        parentViewImage.SetActive(true);
        parentGalleryOverview.SetActive(false);
        index = x;
        UpdateImage();
    }

    void UpdateImage()
    {
        imageToShow.sprite = imageCollection[index];
    }
}

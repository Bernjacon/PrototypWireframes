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
    [SerializeField] int index;
    [SerializeField] GameObject parentGalleryOverview;
    [SerializeField] GameObject parentViewImage;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GoNextOrPrevious(bool goToNext)
    {
        if (goToNext)
        {
            index++;
            UpdateImage();
        }
        else
        {
            index--;
            UpdateImage();
        }

    }
    public void OpenThisImage(int x)
    {
        index = x;
        UpdateImage();
    }

    void UpdateImage()
    {
        imageToShow.sprite = imageCollection[index];
    }
}

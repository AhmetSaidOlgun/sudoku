using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite>();

    private void Awake()
    {
        Managers.ImageManager = this;
    }
}

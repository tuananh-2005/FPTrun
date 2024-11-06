using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartImageController : MonoBehaviour
{
    public Image startImage;

    void Start()
    {
        startImage.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && startImage.gameObject.activeSelf)
        {
            startImage.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AdController : MonoBehaviour
{
    public static AdController instance;
    public AdData adData;
    public Canvas ui;
    public GameObject textPrefab;
    public GameObject connectionError;
    public GameObject invalidTemplate;
    public Action DownloadSuccess;
    public Action DownloadFail;
    private bool isError = false;
    // Start is called before the first frame update
    void Awake()
    {
         if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        adData.textLayer = new ContentHolder();
        adData.frameLayer = new ContentHolder();
        DownloadSuccess += CreateAdd;
        DownloadFail += ShowInvalidTemplate;
    }
    public void CreateAdd()
    {
        if (isError)
            return;
        if(adData.frameLayer.frameBg!=null)//Create Add
        {
            connectionError.SetActive(false);
            GameObject frameLayer = new GameObject();
            frameLayer.name = "Frame";
            frameLayer.transform.localPosition = new Vector3(adData.frameLayer.xPosition, adData.frameLayer.yPosition, 0f);
            frameLayer.transform.SetParent(ui.transform,false);
            frameLayer.AddComponent<RectTransform>().sizeDelta = new Vector2(adData.frameLayer.width,adData.frameLayer.height);
            frameLayer.AddComponent<Image>().sprite = adData.frameLayer.frameBg;
            frameLayer.GetComponent<Image>().color=GetColorFromString("#FFFFFF");//JSon color having alpha is 0 so am giving normalValue
            //
            GameObject TextLayer = Instantiate(textPrefab) as GameObject;
            TextLayer.name = "Text";
            TextLayer.transform.localPosition = new Vector3(adData.textLayer.xPosition, adData.textLayer.yPosition, 0f);
            TextLayer.transform.SetParent(frameLayer.transform, false);
            TextLayer.GetComponent<RectTransform>().sizeDelta = new Vector2(adData.textLayer.width, adData.textLayer.height);
            TextLayer.GetComponent<InputField>().textComponent.color = GetColorFromString(adData.textLayer.colorcode);
        }
        else
        {
            Debug.Log("Invalid Template");
            ShowInvalidTemplate();
        }
    }
    private Color GetColorFromString(string hexString)
    {
        Color newCol;
        ColorUtility.TryParseHtmlString(hexString, out newCol);
        return newCol;
    }
    public void ShowInvalidTemplate()
    {
        isError = true;
        connectionError.SetActive(false);
        invalidTemplate.SetActive(true);
    }

}

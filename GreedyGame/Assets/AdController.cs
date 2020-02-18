using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AdController : MonoBehaviour
{
    public static AdController instance;
    public AdData adData;
    public Image bg;
    public Canvas ui;
    public GameObject textPrefab;
    public GameObject connectionError;
    public GameObject invalidTemplate;
    public Action DownloadSuccess;
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
    }
    public void CreateAdd()
    {
        if(adData.frameLayer.frameBg!=null)//Create Add
        {
            connectionError.SetActive(false);
            GameObject frameLayer = new GameObject();
            frameLayer.name = "Frame";
            frameLayer.transform.SetParent(ui.transform,false);
            frameLayer.transform.position = new Vector3(adData.frameLayer.xPosition,adData.frameLayer.yPosition,0f);
            frameLayer.AddComponent<RectTransform>().sizeDelta = new Vector2(adData.frameLayer.width,adData.frameLayer.height);
            frameLayer.AddComponent<Image>().sprite = adData.frameLayer.frameBg;
            frameLayer.GetComponent<Image>().color=GetColorFromString("#FFFFFF");//JSon color having alpha is 0 so am giving normalValue
            //
            GameObject TextLayer = Instantiate(textPrefab) as GameObject;
            TextLayer.name = "Text";
            TextLayer.transform.SetParent(frameLayer.transform, false);
            TextLayer.transform.position = new Vector3(adData.textLayer.xPosition, adData.textLayer.yPosition, 0f);
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
        invalidTemplate.SetActive(true);
    }

}

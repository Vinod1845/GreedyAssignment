using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using System;
using System.IO;

public class InputHandler : MonoBehaviour
{
    UnityWebRequest ureq;
    string textLayerPath = "http://lab.greedygame.com/arpit-dev/unity-assignment/templates/text_color.json";
    string frameLayerPath = "http://lab.greedygame.com/arpit-dev/unity-assignment/templates/frame_color.json";
    // Start is called before the first frame update
    string jsonString = "";
    JsonData data;
    void Start()
    {
        StartCoroutine(DownloadTextJson());
        StartCoroutine(DownloadFrameLayer());
    }
    IEnumerator DownloadTextJson()
    {
        UnityWebRequest ureq = UnityWebRequest.Get(textLayerPath);
        yield return ureq.SendWebRequest();
        if (ureq.isNetworkError || ureq.isHttpError)
        {
            Debug.Log(ureq.error);
        }
        else
        {
            print("File Success");
            string jsonString = ureq.downloadHandler.text;
            data = JsonMapper.ToObject(jsonString);
            AssaignTextLayer(data);
        }
    }
    public void AssaignTextLayer(JsonData data)
    {
        if(data!=null)
        {
            print("Count is"+data["layers"].Count);
            for(int i=0;i<data["layers"].Count;i++)
            {
                if(data["layers"][i]["type"].ToString()=="text")
                {
                    AdController.instance.adData.textLayer.xPosition=(float)data["layers"][i]["placement"][0]["position"]["x"];
                    AdController.instance.adData.textLayer.yPosition = (float)data["layers"][i]["placement"][0]["position"]["y"];
                    AdController.instance.adData.textLayer.width = (float)data["layers"][i]["placement"][0]["position"]["width"];
                    AdController.instance.adData.textLayer.height = (float)data["layers"][i]["placement"][0]["position"]["height"];
                    for (int j = 0; j < data["layers"][i]["operations"].Count; j++)//Making For loop now jus color only given in future if we give more data we can add (eg.Font size)
                    {
                        if (data["layers"][i]["operations"][j]["name"].ToString() == "color")
                        {
                            AdController.instance.adData.textLayer.colorcode = data["layers"][i]["operations"][j]["argument"].ToString();
                        }
                    }
                    //print("Yes it is String");
                }
            }
        }
    }
    IEnumerator DownloadFrameLayer()
    {
        UnityWebRequest ureq = UnityWebRequest.Get(frameLayerPath);
        yield return ureq.SendWebRequest();
        if (ureq.isNetworkError || ureq.isHttpError)
        {
            Debug.Log(ureq.error);
        }
        else
        {
            print("File Success");
            string jsonString = ureq.downloadHandler.text;
            data = JsonMapper.ToObject(jsonString);
            AssaignFrameLayer(data);
        }
    }
    public void AssaignFrameLayer(JsonData data)
    {
        if (data != null)
        {
            print("Count is" + data["layers"].Count);
            for (int i = 0; i < data["layers"].Count; i++)
            {
                if (data["layers"][i]["type"].ToString() == "frame")
                {
                    AdController.instance.adData.frameLayer.bgPath = data["layers"][i]["path"].ToString();
                    AdController.instance.adData.frameLayer.xPosition = (float)data["layers"][i]["placement"][0]["position"]["x"];
                    AdController.instance.adData.frameLayer.yPosition = (float)data["layers"][i]["placement"][0]["position"]["y"];
                    AdController.instance.adData.frameLayer.width = (float)data["layers"][i]["placement"][0]["position"]["width"];
                    AdController.instance.adData.frameLayer.height = (float)data["layers"][i]["placement"][0]["position"]["height"];
                    for (int j = 0; j < data["layers"][i]["operations"].Count; j++)//Making For loop now jus color only given in future if we give more data we can add (eg.Font size)
                    {
                        if (data["layers"][i]["operations"][j]["name"].ToString() == "color")
                        {
                            AdController.instance.adData.frameLayer.colorcode = data["layers"][i]["operations"][j]["argument"].ToString();
                        }
                    }
                    //print("Yes it is String");
                }
            }
            //print("Height is" + AdController.instance.adData.frameLayer.imagePath);
        }
        StartCoroutine(DownloadFrameImage());
    }
    IEnumerator DownloadFrameImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture( AdController.instance.adData.frameLayer.bgPath);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            AdController.instance.adData.frameLayer.frameBg = null;
        }
        else
        {
            var myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
          AdController.instance.adData.frameLayer.frameBg= Sprite.Create(myTexture,new Rect(0.0f, 0.0f,AdController.instance.adData.frameLayer.width, AdController.instance.adData.frameLayer.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        AdController.instance.DownloadSuccess();//Success Action We are Calling
    }
}

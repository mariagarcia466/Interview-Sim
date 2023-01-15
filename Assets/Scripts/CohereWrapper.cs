using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CohereWrapper : MonoBehaviour
{
    private string _url = "localhost:9989/analyze";
    private string _dataToSend = "";
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnalyzeText(string text)
    {
        AnalyzerData data = new AnalyzerData(text);
        var jsonData = JsonConvert.SerializeObject(data);

        _dataToSend = jsonData;
        
        StartCoroutine("MakeAPIRequest");
    }

    private IEnumerator MakeAPIRequest()
    {
        using UnityWebRequest webRequest = new UnityWebRequest(_url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawTextData = Encoding.UTF8.GetBytes(_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawTextData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                var res = webRequest.downloadHandler.text;
                print(res);
                // TODO: something with the result
                break;
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                print(webRequest.error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

[Serializable]
public class AnalyzerData
{
    public string text;

    public AnalyzerData(string message)
    {
        text = message;
    }
}

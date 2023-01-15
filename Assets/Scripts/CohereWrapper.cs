using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CohereWrapper : MonoBehaviour
{
    // call the Flask server
    private const string URL = "localhost:9989/";
    private string _dataToSend = "";
    
    public void TestAPI()
    {
        AnalyzeText("I was born with glass bones and paper skin. Every morning I break my legs, and every afternoon I break my arms. At night, I lie awake in agony until my heart attacks put me to sleep.");
    }

    public void AnalyzeText(string txt)
    {
        AnalyzerData data = new AnalyzerData(txt);
        var jsonData = JsonConvert.SerializeObject(data);

        _dataToSend = jsonData;
        
        StartCoroutine(nameof(MakeAPIRequest));
    }

    // I don't know if any of this works
    private IEnumerator MakeAPIRequest()
    {
        using UnityWebRequest webRequest = new UnityWebRequest(URL + "/analyze", "POST");
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

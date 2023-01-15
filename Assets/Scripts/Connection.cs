using UnityEngine;
using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;

    public string lastValue = "0";

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("ws://ocean.emily.engineer:1989");

        websocket.OnOpen += () => { Debug.Log("Connection open!"); };

        websocket.OnError += (e) => { Debug.Log("Error! " + e); };

        websocket.OnClose += (e) => { Debug.Log("Connection closed!"); };

        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            lastValue = message;
        };

        await websocket.Connect();
    }

    void Update()
    {
        websocket.DispatchMessageQueue();
    }


    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
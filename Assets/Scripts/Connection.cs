using UnityEngine;
using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("ws://192.168.137.1:1989");

        websocket.OnOpen += () => { Debug.Log("Connection open!"); };

        websocket.OnError += (e) => { Debug.Log("Error! " + e); };

        websocket.OnClose += (e) => { Debug.Log("Connection closed!"); };

        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
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
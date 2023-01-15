using System.Net;
using System.Text;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocketDriver : MonoBehaviour
{

    private WebSocketServer server;
    // Start is called before the first frame update
    void Start()
    {
        // Create server and add listener
        Debug.Log("server");
        server = new WebSocketServer(IPAddress.Parse("192.168.137.1"), 1989, false);
        server.AddWebSocketService<WebSocketListener>("/");
        Debug.Log(server.Address.ToString());
        server.Start();
    }

    private float lastCheck = 0F;
    
    void Update()
    {
        float time = Time.time;

        if (time - lastCheck > 1F)
        {
            lastCheck = time;
            Debug.Log(server.IsListening);
        }
    }
    
    public class WebSocketListener : WebSocketBehavior
    {

        protected override void OnOpen()
        {
            Debug.Log("Opened websocket listener");
        }

        protected override void OnError(ErrorEventArgs error)
        {
            Debug.LogError(error.Message);
        }
    
        protected override void OnMessage (MessageEventArgs e)
        {
            Debug.Log(e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.LogError(e.Reason);
        }
    }
}

using System;
using System.Net.WebSockets;
using System.Collections;
using UnityEngine;

/// <summary>
/// WebSocketの通信を行うエージェントクラス。
/// このクラス自体はやりとりするデータの内容を気にしない。
/// </summary>
public class WebSocketReceiver : MonoBehaviour
{
    /// <summary>
    /// WebSocketサーバのURL
    /// </summary>
    [SerializeField] string url;

    /// <summary>
    /// WebSocket通信の主体となるオブジェクト
    /// </summary>
    ClientWebSocket ws;

    private void Awake()
    {
        ReceiveLoop();
    }

    async void ReceiveLoop()
    {
        ws = new ClientWebSocket();
        var uri = new Uri(url);
        await ws.ConnectAsync(uri, System.Threading.CancellationToken.None);
        // 最初にサーバに自分がUnity側のWebSocketクライアントであることを通知する。
        string msg = "Unity";
        var bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        await ws.SendAsync(bytes, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

        var buffer = new byte[1024];
        while (true)
        {
            var segment = new ArraySegment<byte>(buffer);
            var result = await ws.ReceiveAsync(segment, System.Threading.CancellationToken.None);

            var receiveMsg = System.Text.Encoding.ASCII.GetString(buffer);
            Debug.Log(receiveMsg);
        }
    }
}

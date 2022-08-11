using System;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WebSocketのメッセージを受信した際に発行されるイベント
/// </summary>
/// <param name="msg">WebSocketから受信したメッセージ。加工はしないので、受け手側でパース等をすること</param>
public delegate void OnReceiveMessage(string msg);

/// <summary>
/// WebSocketの通信を行うエージェントクラス。
/// このクラス自体はやりとりするデータの内容を気にしない。
/// </summary>
public class WebSocketReceiver : MonoBehaviour
{
    /// <summary>
    /// WebSocketサーバのURL
    /// </summary>
    [Header("websocketサーバのURL。wss://から始める事")]
    [SerializeField] string url;

    /// <summary>
    /// 受信文字列を出力するためのText
    /// </summary>
    [SerializeField] Text debugText;

    /// <summary>
    /// WebSocket通信の主体となるオブジェクト
    /// </summary>
    ClientWebSocket ws;
    OnReceiveMessage onMsg;

    /// <summary>
    /// すでにコネクションがクローズされた事を示すフラグ。
    /// クローズした後にオープンしないように使用する
    /// </summary>
    bool isAlreadyClosed = false;

    private void Awake()
    {
        // 受信メッセージのデバッグ出力。Debug.Logを使用すると重いのでUI.Textを使用する
        onMsg = (x) =>
        {
            if (debugText != null)
            {
                debugText.text = x;
            }
        };

        ReceiveLoop();
    }

    async void ReceiveLoop()
    {
        ws = new ClientWebSocket();
        var uri = new Uri(url);

        if (!isAlreadyClosed)
        {
            await ws.ConnectAsync(uri, System.Threading.CancellationToken.None);

            // 最初にサーバに自分がUnity側のWebSocketクライアントであることを通知する。
            string msg = "Unity";
            var bytes = System.Text.Encoding.ASCII.GetBytes(msg);
            await ws.SendAsync(bytes, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

            var buffer = new byte[1024];
            while (!isAlreadyClosed)
            {
                for (int i = 0; i < 1024; i++)
                {
                    buffer[i] = 0;
                }
                var segment = new ArraySegment<byte>(buffer);
                var result = await ws.ReceiveAsync(segment, System.Threading.CancellationToken.None);

                var receiveMsg = System.Text.Encoding.ASCII.GetString(buffer);

                onMsg(receiveMsg);
            }
        }
    }

    /// <summary>
    /// WebSocketのメッセージを受信した際に発行するイベントを購読する。
    /// </summary>
    /// <param name="e">新たに購読登録するイベント</param>
    public void RegisterOnReceiveMessage(OnReceiveMessage e)
    {
        onMsg += e;
    }

    private void OnDestroy()
    {
        ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close OnDestroy", System.Threading.CancellationToken.None);
        isAlreadyClosed = true;
    }
}

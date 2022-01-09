using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public class HTTPServer : MonoBehaviour
{

    private HttpListener _listener;

    private async void Start(){
        List<String> prefixes = new List<string>();
        prefixes.Add("http://*:4156/map/");
        prefixes.Add("http://*:4156/info/");

        _listener = new HttpListener();
        foreach (string s in prefixes)
        {
            _listener.Prefixes.Add(s);
        }
        _listener.Start();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback),_listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener) result.AsyncState;
        // Call EndGetContext to complete the asynchronous operation.
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;

        HttpListenerResponse response = context.Response;

        // string responseString = "<HTML><BODY>Hello world!</BODY></HTML>";
        string localPath = request.Url.LocalPath;
        string responseString = "Invalid path";
        if(localPath == "/info/"){
            responseString = "Info";
        }else if(localPath == "/map/"){
            responseString = "Map";
        }

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer,0,buffer.Length);
        // You must close the output stream.
        output.Close();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback),_listener);
    }

    private void OnDestroy(){
        _listener.Close();
    }
}

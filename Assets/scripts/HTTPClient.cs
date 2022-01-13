using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPClient : MonoBehaviour
{

    [SerializeField] private const string serverAddress = "http://localhost:8080/";

    public static HTTPClient client = null;

    HTTPClient() {
        if(client != null) {
            Debug.LogError("Attempted to construct second HTTPCLient!");
        } else {
            client = this;
        }
    }

    public void PutRequest(string endpoint, byte[] data)
    {
        StartCoroutine(PutRequestCoroutine(endpoint, data));
    }

    public void PutRequest(string endpoint, string data)
    {
        StartCoroutine(PutRequestCoroutine(endpoint, data));
    }

    private IEnumerator PutRequestCoroutine(string endpoint, byte[] data)
    {
        string uri = serverAddress + endpoint;

        Debug.Log(System.Text.Encoding.Default.GetString(data));

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, data))
        {   
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    private IEnumerator PutRequestCoroutine(string endpoint, string data)
    {
        string uri = serverAddress + endpoint;

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, data))
        {

            webRequest.SetRequestHeader ("Content-Type", "application/json");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}

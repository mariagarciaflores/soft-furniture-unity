using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttRestConnection : MonoBehaviour
{

    TextMesh text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<TextMesh>();
        text.text = "Conectando con la API";
        Debug.Log("Start ************************");
        //WWW myWww = new WWW("http://localhost:63342/api/Orders/1");
        //// ... is analogous to ...
        //UnityWebRequest myWr = UnityWebRequest.Get("http://localhost:63342/api/Orders/1");
        StartCoroutine(GetUpdatedText());

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator GetUpdatedText()
    {

        bool requestFinished = false;
        bool requestErrorOccurred = false;

        UnityWebRequest request = UnityWebRequest.Get("http://localhost:63342/api/Orders/1");
        yield return request.SendWebRequest();

        requestFinished = true;
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
            requestErrorOccurred = true;
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);
            Order order = JsonConvert.DeserializeObject<Order>(request.downloadHandler.text);
            Debug.Log(order.Name);
            text.text = order.Name;
            Debug.Log(request.downloadHandler.data);

            if (request.responseCode == 200)
            {
                Debug.Log("Request finished successfully!");
            }
            else if (request.responseCode == 401) // an occasional unauthorized error
            {
                Debug.Log("Error 401: Unauthorized. Resubmitted request!");
                StartCoroutine(GetUpdatedText());
                requestErrorOccurred = true;
            }
            else
            {
                Debug.Log("Request failed (status:" + request.responseCode + ")");
                requestErrorOccurred = true;
            }

            if (!requestErrorOccurred)
            {
                yield return null;
                // process results
            }
        }
    }
}

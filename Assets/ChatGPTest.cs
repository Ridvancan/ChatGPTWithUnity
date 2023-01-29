using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class ChatGPTest : MonoBehaviour
{
    private string API_KEY = "YOUR API KEY";
    private const string API_URL = "https://api.openai.com/v1/completions";
    public string chatTestQuestion;
    public TextMeshProUGUI testtmp;
    public TMP_InputField questionField;
    public TMP_InputField resultField;
    bool waitForResp;
    [TextArea(15,20)]
    public string resultText;
    void Start()
    {
        
    }
    public void GetAnswer()
    {
        waitForResp = true;
        if (questionField.text!="")
        {
            StartCoroutine(GenerateText(questionField.text));

        }
    }

    private void Update()
    {
        if (waitForResp)
        {
            transform.Rotate(Vector3.one,2);
        }
    }
    public IEnumerator GenerateText(string prompt)
    {
      
        string jsonPayload = "{\"model\": \"text-davinci-003\", \"prompt\": \"" + prompt + "\", \"temperature\": 0, \"max_tokens\": 500}";

        var request = new UnityWebRequest(API_URL, "POST");
        byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        request.uploadHandler = new UploadHandlerRaw(payloadBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);

        // Send the request
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Parse the response as JSON
            string responseJson = request.downloadHandler.text;
            var response = JsonUtility.FromJson<OpenAIResponse>(responseJson);
            // Log the generated text
            resultText= response.choices[0].text;
            resultField.text = resultText;
            waitForResp = false;
            transform.eulerAngles = Vector3.zero;
        }
    }
}

[System.Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public string text;
}
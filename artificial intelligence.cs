using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FigureCreator : MonoBehaviour
{
    void Start()
    {
        // ������ ������� �� �������� �����
        StartCoroutine(SendTextCommand("Create a sphere with radius 3"));
    }

    IEnumerator SendTextCommand(string command)
    {
        // ���������� JSON-�������
        string jsonData = "{\"command\":\"" + command + "\"}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // ������ � ���������� ������� Flask
        UnityWebRequest request = new UnityWebRequest("http://localhost:5000/interpret", "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // �������� ������ �� �������
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ��������� ���������� � �������
            string result = request.downloadHandler.text;
            Debug.Log("Server Response: " + result);

            // ����� ��������� ������� �� ����� �� ������ �������
            ExecuteCommand(result);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    void ExecuteCommand(string command)
    {
        // ������ �������: "Create a sphere with radius 3"
        if (command.Contains("sphere"))
        {
            float radius = ExtractFloatValue(command, "radius");
            CreateSphere(radius);
        }
        // ����� ����� �������� ��������� ��� ������ �����: �������, ��������������� � �.�.
    }

    void CreateSphere(float radius)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(radius, radius, radius);
    }

    float ExtractFloatValue(string command, string parameter)
    {
        // ���������� ������ ��� ���������� ����� �� ������ (������: "radius 3")
        string[] words = command.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].ToLower() == parameter.ToLower())
            {
                return float.Parse(words[i + 1]);
            }
        }
        return 1.0f; // �������� �� ���������
    }
}

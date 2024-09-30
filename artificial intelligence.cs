using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FigureCreator : MonoBehaviour
{
    void Start()
    {
        // Пример команды на создание сферы
        StartCoroutine(SendTextCommand("Create a sphere with radius 3"));
    }

    IEnumerator SendTextCommand(string command)
    {
        // Подготовка JSON-запроса
        string jsonData = "{\"command\":\"" + command + "\"}";
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Запрос к локальному серверу Flask
        UnityWebRequest request = new UnityWebRequest("http://localhost:5000/interpret", "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Ожидание ответа от сервера
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Получение результата с сервера
            string result = request.downloadHandler.text;
            Debug.Log("Server Response: " + result);

            // Вызов генерации объекта на сцене на основе команды
            ExecuteCommand(result);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    void ExecuteCommand(string command)
    {
        // Пример команды: "Create a sphere with radius 3"
        if (command.Contains("sphere"))
        {
            float radius = ExtractFloatValue(command, "radius");
            CreateSphere(radius);
        }
        // Здесь можно добавить обработку для других фигур: капсулы, параллелепипеда и т.д.
    }

    void CreateSphere(float radius)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(radius, radius, radius);
    }

    float ExtractFloatValue(string command, string parameter)
    {
        // Простейшая логика для извлечения числа из текста (пример: "radius 3")
        string[] words = command.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].ToLower() == parameter.ToLower())
            {
                return float.Parse(words[i + 1]);
            }
        }
        return 1.0f; // значение по умолчанию
    }
}

using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Prism : MonoBehaviour
{
    public int sides = 6; // Количество боковых граней
    public float radius = 1f; // Радиус окружности основания
    public float height = 2f; // Высота призмы
    public Color color = Color.white; // Цвет призмы

    private void Start()
    {
        CreatePrism();
    }

    void CreatePrism()
    {
        // Создание вершин призмы
        Vector3[] vertices = new Vector3[sides * 2 + 2]; // Верхние и нижние вершины + центральные точки
        float angleStep = 360f / sides;

        // Нижние вершины
        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            vertices[i] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        // Верхние вершины
        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            vertices[i + sides] = new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
        }

        // Центральные точки
        vertices[sides * 2] = new Vector3(0, 0, 0); // Нижняя центральная точка
        vertices[sides * 2 + 1] = new Vector3(0, height, 0); // Верхняя центральная точка

        // Создание треугольников
        int[] triangles = new int[sides * 12];

        // Боковые грани
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;

            // Нижняя боковая грань
            triangles[i * 6 + 0] = i;
            triangles[i * 6 + 1] = nextIndex;
            triangles[i * 6 + 2] = i + sides;

            // Верхняя боковая грань
            triangles[i * 6 + 3] = nextIndex;
            triangles[i * 6 + 4] = nextIndex + sides;
            triangles[i * 6 + 5] = i + sides;
        }

        // Нижнее основание
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;
            triangles[sides * 6 + i * 3 + 0] = sides * 2; // Центральная точка
            triangles[sides * 6 + i * 3 + 1] = nextIndex;
            triangles[sides * 6 + i * 3 + 2] = i;
        }

        // Верхнее основание
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;
            triangles[sides * 9 + i * 3 + 0] = sides * 2 + 1; // Центральная точка
            triangles[sides * 9 + i * 3 + 1] = i + sides;
            triangles[sides * 9 + i * 3 + 2] = nextIndex + sides;
        }

        // Создание Mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Применение Mesh и цвета
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = color;
    }
}

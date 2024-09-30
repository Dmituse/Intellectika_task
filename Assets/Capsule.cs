using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Capsule : MonoBehaviour
{
    public float radius = 0.5f; // Радиус капсулы
    public float height = 2f;   // Высота капсулы (с учётом полусфер)
    public int segments = 24;   // Количество сегментов для полусфер и цилиндра
    public int rings = 12;      // Количество колец для полусфер
    public Color color = Color.white; // Цвет капсулы

    private void Start()
    {
        CreateCapsule();
    }

    void CreateCapsule()
    {
        Mesh mesh = new Mesh();
        float cylinderHeight = height - 2 * radius; // Высота цилиндрической части

        // Проверка на корректную высоту
        if (cylinderHeight < 0)
        {
            Debug.LogError("Height must be larger than twice the radius!");
            return;
        }

        // Создание списков для вершин и треугольников
        var vertices = new System.Collections.Generic.List<Vector3>();
        var triangles = new System.Collections.Generic.List<int>();

        // Создание вершин для верхней полусферы
        for (int i = 0; i <= rings; i++)
        {
            float ringAngle = Mathf.PI / 2 * i / rings;
            float y = Mathf.Sin(ringAngle) * radius + cylinderHeight / 2;
            float ringRadius = Mathf.Cos(ringAngle) * radius;

            for (int j = 0; j <= segments; j++)
            {
                float segmentAngle = 2 * Mathf.PI * j / segments;
                float x = ringRadius * Mathf.Cos(segmentAngle);
                float z = ringRadius * Mathf.Sin(segmentAngle);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Создание вершин для цилиндра
        for (int i = 0; i <= 1; i++)
        {
            float y = cylinderHeight * (i - 0.5f);

            for (int j = 0; j <= segments; j++)
            {
                float segmentAngle = 2 * Mathf.PI * j / segments;
                float x = radius * Mathf.Cos(segmentAngle);
                float z = radius * Mathf.Sin(segmentAngle);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Создание вершин для нижней полусферы
        for (int i = 0; i <= rings; i++)
        {
            float ringAngle = Mathf.PI / 2 * i / rings;
            float y = -Mathf.Sin(ringAngle) * radius - cylinderHeight / 2;
            float ringRadius = Mathf.Cos(ringAngle) * radius;

            for (int j = 0; j <= segments; j++)
            {
                float segmentAngle = 2 * Mathf.PI * j / segments;
                float x = ringRadius * Mathf.Cos(segmentAngle);
                float z = ringRadius * Mathf.Sin(segmentAngle);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Треугольники для верхней полусферы
        int topOffset = 0;
        for (int i = 0; i < rings; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int current = topOffset + i * (segments + 1) + j;
                int next = current + segments + 1;

                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(current + 1);

                triangles.Add(next);
                triangles.Add(next + 1);
                triangles.Add(current + 1);
            }
        }

        // Треугольники для цилиндра
        int cylinderOffset = topOffset + (rings + 1) * (segments + 1);
        for (int i = 0; i < segments; i++)
        {
            int current = cylinderOffset + i;
            int next = current + segments + 1;

            triangles.Add(current);
            triangles.Add(next);
            triangles.Add(current + 1);

            triangles.Add(next);
            triangles.Add(next + 1);
            triangles.Add(current + 1);
        }

        // Треугольники для нижней полусферы
        int bottomOffset = cylinderOffset + 2 * (segments + 1);
        for (int i = 0; i < rings; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int current = bottomOffset + i * (segments + 1) + j;
                int next = current + segments + 1;

                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(current + 1);

                triangles.Add(next);
                triangles.Add(next + 1);
                triangles.Add(current + 1);
            }
        }

        // Применение вершин и треугольников к mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // Применение Mesh и цвета
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = color;
    }
}

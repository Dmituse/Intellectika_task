using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Sphere : MonoBehaviour
{
    public float radius = 1f; // Радиус сферы
    public int sectors = 24;  // Количество секторов (долгот)
    public int stacks = 16;   // Количество сегментов по высоте (широта)
    public Color color = Color.white; // Цвет сферы

    private void Start()
    {
        CreateSphere();
    }

    void CreateSphere()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices;
        int[] triangles;
        int vertexCount = (sectors + 1) * (stacks + 1);

        // Создание массива вершин
        vertices = new Vector3[vertexCount];

        float sectorStep = 2 * Mathf.PI / sectors;
        float stackStep = Mathf.PI / stacks;
        int vertexIndex = 0;

        for (int i = 0; i <= stacks; ++i)
        {
            float stackAngle = Mathf.PI / 2 - i * stackStep;
            float xy = radius * Mathf.Cos(stackAngle);
            float z = radius * Mathf.Sin(stackAngle);

            for (int j = 0; j <= sectors; ++j)
            {
                float sectorAngle = j * sectorStep;

                float x = xy * Mathf.Cos(sectorAngle);
                float y = xy * Mathf.Sin(sectorAngle);
                vertices[vertexIndex] = new Vector3(x, z, y);
                vertexIndex++;
            }
        }

        // Создание массива треугольников
        int trianglesCount = stacks * sectors * 6;
        triangles = new int[trianglesCount];
        int triangleIndex = 0;

        for (int i = 0; i < stacks; ++i)
        {
            int k1 = i * (sectors + 1);
            int k2 = k1 + sectors + 1;

            for (int j = 0; j < sectors; ++j, ++k1, ++k2)
            {
                if (i != 0)
                {
                    triangles[triangleIndex++] = k1;
                    triangles[triangleIndex++] = k2;
                    triangles[triangleIndex++] = k1 + 1;
                }

                if (i != (stacks - 1))
                {
                    triangles[triangleIndex++] = k1 + 1;
                    triangles[triangleIndex++] = k2;
                    triangles[triangleIndex++] = k2 + 1;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Применение Mesh и цвета
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = color;
    }
}

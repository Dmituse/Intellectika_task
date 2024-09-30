using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Prism : MonoBehaviour
{
    public int sides = 6; // ���������� ������� ������
    public float radius = 1f; // ������ ���������� ���������
    public float height = 2f; // ������ ������
    public Color color = Color.white; // ���� ������

    private void Start()
    {
        CreatePrism();
    }

    void CreatePrism()
    {
        // �������� ������ ������
        Vector3[] vertices = new Vector3[sides * 2 + 2]; // ������� � ������ ������� + ����������� �����
        float angleStep = 360f / sides;

        // ������ �������
        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            vertices[i] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        // ������� �������
        for (int i = 0; i < sides; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            vertices[i + sides] = new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
        }

        // ����������� �����
        vertices[sides * 2] = new Vector3(0, 0, 0); // ������ ����������� �����
        vertices[sides * 2 + 1] = new Vector3(0, height, 0); // ������� ����������� �����

        // �������� �������������
        int[] triangles = new int[sides * 12];

        // ������� �����
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;

            // ������ ������� �����
            triangles[i * 6 + 0] = i;
            triangles[i * 6 + 1] = nextIndex;
            triangles[i * 6 + 2] = i + sides;

            // ������� ������� �����
            triangles[i * 6 + 3] = nextIndex;
            triangles[i * 6 + 4] = nextIndex + sides;
            triangles[i * 6 + 5] = i + sides;
        }

        // ������ ���������
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;
            triangles[sides * 6 + i * 3 + 0] = sides * 2; // ����������� �����
            triangles[sides * 6 + i * 3 + 1] = nextIndex;
            triangles[sides * 6 + i * 3 + 2] = i;
        }

        // ������� ���������
        for (int i = 0; i < sides; i++)
        {
            int nextIndex = (i + 1) % sides;
            triangles[sides * 9 + i * 3 + 0] = sides * 2 + 1; // ����������� �����
            triangles[sides * 9 + i * 3 + 1] = i + sides;
            triangles[sides * 9 + i * 3 + 2] = nextIndex + sides;
        }

        // �������� Mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // ���������� Mesh � �����
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = color;
    }
}

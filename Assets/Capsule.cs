using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Capsule : MonoBehaviour
{
    public float radius = 0.5f; // ������ �������
    public float height = 2f;   // ������ ������� (� ������ ��������)
    public int segments = 24;   // ���������� ��������� ��� �������� � ��������
    public int rings = 12;      // ���������� ����� ��� ��������
    public Color color = Color.white; // ���� �������

    private void Start()
    {
        CreateCapsule();
    }

    void CreateCapsule()
    {
        Mesh mesh = new Mesh();
        float cylinderHeight = height - 2 * radius; // ������ �������������� �����

        // �������� �� ���������� ������
        if (cylinderHeight < 0)
        {
            Debug.LogError("Height must be larger than twice the radius!");
            return;
        }

        // �������� ������� ��� ������ � �������������
        var vertices = new System.Collections.Generic.List<Vector3>();
        var triangles = new System.Collections.Generic.List<int>();

        // �������� ������ ��� ������� ���������
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

        // �������� ������ ��� ��������
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

        // �������� ������ ��� ������ ���������
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

        // ������������ ��� ������� ���������
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

        // ������������ ��� ��������
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

        // ������������ ��� ������ ���������
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

        // ���������� ������ � ������������� � mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // ���������� Mesh � �����
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = color;
    }
}

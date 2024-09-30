using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cuboid : MonoBehaviour
{
    public Vector3 size = new Vector3(1, 1, 1); // �����, ������ � ������
    public Color color = Color.white; // ���� ���������������

    private void Start()
    {
        CreateCuboid();
    }

    void CreateCuboid()
    {
        // �������� ������� ������
        Vector3[] vertices = new Vector3[8];
        vertices[0] = new Vector3(-size.x / 2, -size.y / 2, -size.z / 2);
        vertices[1] = new Vector3(size.x / 2, -size.y / 2, -size.z / 2);
        vertices[2] = new Vector3(size.x / 2, size.y / 2, -size.z / 2);
        vertices[3] = new Vector3(-size.x / 2, size.y / 2, -size.z / 2);
        vertices[4] = new Vector3(-size.x / 2, -size.y / 2, size.z / 2);
        vertices[5] = new Vector3(size.x / 2, -size.y / 2, size.z / 2);
        vertices[6] = new Vector3(size.x / 2, size.y / 2, size.z / 2);
        vertices[7] = new Vector3(-size.x / 2, size.y / 2, size.z / 2);

        // �������� ������� �������������
        int[] triangles = new int[36]
        {
            0, 2, 1, 0, 3, 2, // �������� �����
            4, 5, 6, 4, 6, 7, // ������ �����
            0, 1, 5, 0, 5, 4, // ������ �����
            2, 3, 7, 2, 7, 6, // ������� �����
            0, 4, 7, 0, 7, 3, // ����� �����
            1, 2, 6, 1, 6, 5  // ������ �����
        };

        // �������� ������� Mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // ���������� �����
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;

        // ��������� ����� ���������
        GetComponent<MeshRenderer>().material.color = color;
    }
}

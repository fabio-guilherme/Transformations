using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transformation3D
{
    None,
    Translation3D,
    Scaling3D,
    RotationX3D,
    RotationY3D,
    RotationZ3D
}

public class PyramidObject : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    public Transformation3D transformation3D;
    public bool decrease = false;

    // Start is called before the first frame update
    void Start()
    {
        // Create mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "MyMesh";

        vertices = new Vector3[18];
        //back triangle vertices
        vertices[0] = new Vector3(-10, 0, -10);
        vertices[1] = new Vector3(10, 0, -10);
        vertices[2] = new Vector3(0, 10, 0);
        //front triangle vertices
        vertices[3] = new Vector3(-10, 0, 10);
        vertices[4] = new Vector3(10, 0, 10);
        vertices[5] = new Vector3(0, 10, 0);
        //left triangle vertices
        vertices[6] = new Vector3(-10, 0, 10);
        vertices[7] = new Vector3(-10, 0, -10);
        vertices[8] = new Vector3(0, 10, 0);
        //right triangle vertices
        vertices[9] = new Vector3(10, 0, 10);
        vertices[10] = new Vector3(10, 0, -10);
        vertices[11] = new Vector3(0, 10, 0);
        //botton triangle 1 vertices
        vertices[12] = new Vector3(-10, 0, -10);
        vertices[13] = new Vector3(10, 0, -10);
        vertices[14] = new Vector3(-10, 0, 10);
        //botton triangle 2 vertices
        vertices[15] = new Vector3(10, 0, 10);
        vertices[16] = new Vector3(10, 0, -10);
        vertices[17] = new Vector3(-10, 0, 10);

        int[] triangles = new int[18];
        //back triangle
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        //front triangle
        triangles[3] = 4;
        triangles[4] = 5;
        triangles[5] = 3;
        //left triangle
        triangles[6] = 6;
        triangles[7] = 8;
        triangles[8] = 7;
        //right triangle
        triangles[9] = 10;
        triangles[10] = 11;
        triangles[11] = 9;
        //bottom triangle 1
        triangles[12] = 13;
        triangles[13] = 14;
        triangles[14] = 12;
        //bottom triangle 2
        triangles[15] = 15;
        triangles[16] = 17;
        triangles[17] = 16;

        normals = new Vector3[18];

        normals[0] = Vector3.back + Vector3.up;
        normals[1] = Vector3.back + Vector3.up;
        normals[2] = Vector3.back + Vector3.up;

        normals[3] = Vector3.forward + Vector3.up;
        normals[4] = Vector3.forward + Vector3.up;
        normals[5] = Vector3.forward + Vector3.up;

        normals[6] = Vector3.left + Vector3.up;
        normals[7] = Vector3.left + Vector3.up;
        normals[8] = Vector3.left + Vector3.up;

        normals[9] = Vector3.right + Vector3.up;
        normals[10] = Vector3.right + Vector3.up;
        normals[11] = Vector3.right + Vector3.up;

        normals[12] = Vector3.down;
        normals[13] = Vector3.down;
        normals[14] = Vector3.down;

        normals[15] = Vector3.down;
        normals[16] = Vector3.down;
        normals[17] = Vector3.down;

        //Update mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Add a Mesh Renderer component to the Mesh object
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a new material that uses a grey color
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.grey;

        // Assign the material to the Mesh object
        meshRenderer.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        float factor;
        switch (transformation3D)
        {
            case Transformation3D.Translation3D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Translate3D(0, 0, factor * 2 * Time.deltaTime);
                    break;
                }
            case Transformation3D.Scaling3D:
                {
                    factor = decrease ? 0.99f : 1.01f;
                    Scale3D(factor, 1, 1);
                    break;
                }
            case Transformation3D.RotationX3D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    RotateX3D(factor * 20 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            case Transformation3D.RotationY3D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    RotateY3D(factor * 20 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            case Transformation3D.RotationZ3D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    RotateZ3D(factor * 20 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            default: break;
        }

    }

    void Translate3D(float tx, float ty, float tz)
    {
        Matrix4x4 translation_matrix = new Matrix4x4();
        translation_matrix.SetRow(0, new Vector4(1f, 0f, 0f, tx));
        translation_matrix.SetRow(1, new Vector4(0f, 1f, 0f, ty));
        translation_matrix.SetRow(2, new Vector4(0f, 0f, 1f, tz));
        translation_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = translation_matrix.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void Scale3D(float sx, float sy, float sz)
    {
        Matrix4x4 scale_matrix = new Matrix4x4();
        scale_matrix.SetRow(0, new Vector4(sx, 0f, 0f, 0));
        scale_matrix.SetRow(1, new Vector4(0f, sy, 0f, 0));
        scale_matrix.SetRow(2, new Vector4(0f, 0f, sz, 0));
        scale_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = scale_matrix.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void RotateX3D(float angle)
    {
        Matrix4x4 rxmat = new Matrix4x4();
        rxmat.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
        rxmat.SetRow(1, new Vector4(0f, Mathf.Cos(angle), -Mathf.Sin(angle), 0f));
        rxmat.SetRow(2, new Vector4(0f, Mathf.Sin(angle), Mathf.Cos(angle), 0f));
        rxmat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rxmat.MultiplyPoint(vertices[i]);
            normals[i] = rxmat.MultiplyPoint(normals[i]);
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    void RotateY3D(float angle)
    {
        Matrix4x4 rymat = new Matrix4x4();
        rymat.SetRow(0, new Vector4(Mathf.Cos(angle), 0f, Mathf.Sin(angle), 0f));
        rymat.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
        rymat.SetRow(2, new Vector4(-Mathf.Sin(angle), 0f, Mathf.Cos(angle), 0f));
        rymat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rymat.MultiplyPoint(vertices[i]);
            normals[i] = rymat.MultiplyPoint(normals[i]);
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    void RotateZ3D(float angle)
    {
        Matrix4x4 rzmat = new Matrix4x4();
        rzmat.SetRow(0, new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), 0f, 0f));
        rzmat.SetRow(1, new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0f, 0f));
        rzmat.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
        rzmat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rzmat.MultiplyPoint(vertices[i]);
            normals[i] = rzmat.MultiplyPoint(normals[i]);
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    // Visualise the normals
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

}

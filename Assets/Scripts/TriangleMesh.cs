using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transformation2D
{
    None,
    Translation2D,
    Scaling2D,
    ScalingMatrix2D,
    Rotation2D,
    RotationMatrix2D,
    Translation2DMatrixHM,
    Scaling2DMatrixHM,
    Rotation2DMatrixHM
}

public class TriangleMesh : Transformations
{
    private Mesh mesh;
    private Vector3[] vertices;

    public Transformation2D transformation2D;
    public bool decrease = false;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "MyMesh";
        vertices = new Vector3[3];
        vertices[0] = new Vector3(3, 0, 1);
        vertices[1] = new Vector3(5, 0, 1);
        vertices[2] = new Vector3(4, 2, 1);
        mesh.vertices = vertices;
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        mesh.triangles = triangles;

        // Add a Mesh Renderer component to the Mesh object
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a new material that uses a pink color
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.magenta;

        // Assign the material to the Mesh object
        meshRenderer.material = material;

        // Required for correct lighting when using lit shaders
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void Update()
    {
        float factor;
        switch (transformation2D)
        {
            case Transformation2D.Translation2D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Translate2D(factor * 2 * Time.deltaTime, 0);
                    break;
                }
            case Transformation2D.Scaling2D:
                {
                    factor = decrease ? 0.99f : 1.01f;
                    Scale2D(factor, 1);
                    break;
                }
            case Transformation2D.ScalingMatrix2D:
                {
                    factor = decrease ? 0.99f : 1.01f;
                    Scale2DMat(factor, factor);
                    break;
                }
            case Transformation2D.Rotation2D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Rotate2D(factor * 20 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            case Transformation2D.RotationMatrix2D:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Rotate2DMat(factor * 120 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            case Transformation2D.Translation2DMatrixHM:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Translate2DMatHM(0, factor * 2 * Time.deltaTime);
                    break;
                }
            case Transformation2D.Scaling2DMatrixHM:
                {
                    factor = decrease ? 0.99f : 1.01f;
                    Scale2DMatHM(1, factor);
                    break;
                }
            case Transformation2D.Rotation2DMatrixHM:
                {
                    factor = decrease ? -1.0f : 1.0f;
                    Rotate2DMatHM(factor * 4 * Mathf.Deg2Rad * Time.deltaTime);
                    break;
                }
            default: break;
        }
    }


    #region Basic Transformations

    void Translate2D(float tx, float ty)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x + tx,
                                      vertices[i].y + ty,
                                      vertices[i].z);
        }
        mesh.vertices = vertices;
    }

    void Scale2D(float sx, float sy)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x * sx,
                                      vertices[i].y * sy,
                                      vertices[i].z);
    }
        mesh.vertices = vertices;
    }

    void Scale2DMat(float sx, float sy)
    {
        float[,] mat = new float[2, 2];
        mat[0, 0] = sx; mat[0, 1] = 0;
        mat[1, 0] = 0;  mat[1, 1] = sy;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiply(mat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void Rotate2D(float angle)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x * Mathf.Cos(angle)
                                   - vertices[i].y * Mathf.Sin(angle),
                                     vertices[i].x * Mathf.Sin(angle)
                                   + vertices[i].y * Mathf.Cos(angle));
        }
        mesh.vertices = vertices;
    }

    void Rotate2DMat(float angle)
    {
        float[,] mat = new float[2, 2];
        mat[0, 0] = Mathf.Cos(angle); mat[0, 1] = -Mathf.Sin(angle);
        mat[1, 0] = Mathf.Sin(angle); mat[1, 1] = Mathf.Cos(angle);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiply(mat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    #endregion


    #region Homogeneous Coordinates

    void Translate2DMatHM(float tx, float ty)
    {
        float[,] mat = new float[3, 3];
        mat[0, 0] = 1; mat[0, 1] = 0; mat[0, 2] = tx;
        mat[1, 0] = 0; mat[1, 1] = 1; mat[1, 2] = ty;
        mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = 1;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiplyHM2D(mat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void Scale2DMatHM(float sx, float sy)
    {
        float[,] mat = new float[3, 3];
        mat[0, 0] = sx; mat[0, 1] = 0; mat[0, 2] = 0;
        mat[1, 0] = 0; mat[1, 1] = sy; mat[1, 2] = 0;
        mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = 1;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiplyHM2D(mat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void Rotate2DMatHM(float angle)
    {
        float[,] mat = new float[3, 3];
        mat[0, 0] = Mathf.Cos(angle); mat[0, 1] = -Mathf.Sin(angle); mat[0, 2] = 0;
        mat[1, 0] = Mathf.Sin(angle); mat[1, 1] = Mathf.Cos(angle); mat[1, 2] = 0;
        mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = 1;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiplyHM2D(mat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    #endregion

}
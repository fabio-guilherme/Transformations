using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombinedTransformation
{
    None,
    RotateScale,
    RotateAroundPoint
}

public class SquareObject : Transformations
{
    private Mesh mesh;
    private Vector3[] vertices;

    // Vertices
    static private Coordinate p1 = new Coordinate(0, 0);
    static private Coordinate p2 = new Coordinate(10, 0);
    static private Coordinate p3 = new Coordinate(0, 10);
    static private Coordinate p4 = new Coordinate(10, 10);

    public CombinedTransformation combinedTransformation;
    public float angleInDegrees = 0f;
    public float scaleX = 1;
    public float scaleY = 1;
    public Point point = Point.P1;
    public bool run = false;
    public bool invert = false;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "MyMesh";
        vertices = new Vector3[4];
        Reset();

        int[] triangles = new int[6];
        triangles[0] = 0; // p1
        triangles[1] = 3; // p4
        triangles[2] = 1; // p2
        triangles[3] = 0; // p1
        triangles[4] = 2; // p3
        triangles[5] = 3; // p4
        mesh.triangles = triangles;

        // Add a Mesh Renderer component to the Mesh object
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a new material that uses a green color
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.green;

        // Assign the material to the Mesh object
        meshRenderer.material = material;

        // Required for correct lighting when using lit shaders
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void Update()
    {
        if (run)
        {
            run = false;
            Reset();

            Coordinate p = point switch
            {
                Point.P1 => p1,
                Point.P2 => p2,
                Point.P3 => p3,
                Point.P4 => p4,
                _ => p1
            };
            float angleInRadians;
            switch (combinedTransformation)
            {
                case CombinedTransformation.RotateScale:
                    {
                        angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                        RotateScale(angleInRadians, scaleX, scaleY);
                        break;
                    }
                case CombinedTransformation.RotateAroundPoint:
                    {
                        angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                        RotateAroundPoint(angleInRadians, p.x, p.y);
                        break;
                    }
                default: break;
            }
        }
    }

    void Reset()
    {
        vertices[0] = new Vector3(p1.x, p1.y, 1);
        vertices[1] = new Vector3(p2.x, p2.y, 1);
        vertices[2] = new Vector3(p3.x, p3.y, 1);
        vertices[3] = new Vector3(p4.x, p4.y, 1);
        mesh.vertices = vertices;
    }

    #region Combining Transformations

    void RotateScale(float angle, float sx, float sy)
    {
        float[,] rmat = new float[3, 3];
        rmat[0, 0] = Mathf.Cos(angle);  rmat[0, 1] = -Mathf.Sin(angle); rmat[0, 2] = 0;
        rmat[1, 0] = Mathf.Sin(angle);  rmat[1, 1] = Mathf.Cos(angle);  rmat[1, 2] = 0;
        rmat[2, 0] = 0;                 rmat[2, 1] = 0;                 rmat[2, 2] = 1;

        float[,] smat = new float[3, 3];
        smat[0, 0] = sx;    smat[0, 1] = 0;     smat[0, 2] = 0;
        smat[1, 0] = 0;     smat[1, 1] = sy;    smat[1, 2] = 0;
        smat[2, 0] = 0;     smat[2, 1] = 0;     smat[2, 2] = 1;

        float[,] finalmat = invert ? multiply(rmat, smat) : multiply(smat, rmat);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiplyHM2D(finalmat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    void RotateAroundPoint(float angle, float px, float py)
    {
        float[,] rmat = new float[3, 3];
        rmat[0, 0] = Mathf.Cos(angle);  rmat[0, 1] = -Mathf.Sin(angle); rmat[0, 2] = 0;
        rmat[1, 0] = Mathf.Sin(angle);  rmat[1, 1] = Mathf.Cos(angle);  rmat[1, 2] = 0;
        rmat[2, 0] = 0;                 rmat[2, 1] = 0;                 rmat[2, 2] = 1;
        float[,] t1mat = new float[3, 3];
        t1mat[0, 0] = 1; t1mat[0, 1] = 0; t1mat[0, 2] = -px;
        t1mat[1, 0] = 0; t1mat[1, 1] = 1; t1mat[1, 2] = -py;
        t1mat[2, 0] = 0; t1mat[2, 1] = 0; t1mat[2, 2] = 1;
        float[,] t2mat = new float[3, 3];
        t2mat[0, 0] = 1; t2mat[0, 1] = 0; t2mat[0, 2] = px;
        t2mat[1, 0] = 0; t2mat[1, 1] = 1; t2mat[1, 2] = py;
        t2mat[2, 0] = 0; t2mat[2, 1] = 0; t2mat[2, 2] = 1;
        float[,] finalmat = multiply(multiply(t2mat, rmat), t1mat);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = multiplyHM2D(finalmat, vertices[i]);
        }
        mesh.vertices = vertices;
    }

    #endregion

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Point
{
    P1,
    P2,
    P3,
    P4
}

public class Transformations : MonoBehaviour
{
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Applies a transformation matrix to a vector.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="point"></param>
    /// <returns>The transformed vector.</returns>
    public Vector3 multiply(float[,] matrix, Vector3 point)
    {
        Vector3 result = new Vector3();
        for (int r = 0; r < matrix.GetLength(0); r++)
        {
            float s = 0;
            for (int z = 0; z < matrix.GetLength(1); z++)
                s += matrix[r, z] * point[z];
            result[r] = s;
        }
        // Keeping on the same plane.
        // Without it, z would always become zero as the matrix has only 2 dimensions.
        result.z = point.z;
        return result;
    }

    /// <summary>
    /// Multiplies 2 matrices.
    /// </summary>
    /// <param name="matrix1"></param>
    /// <param name="matrix2"></param>
    /// <returns></returns>
    public float[,] multiply(float[,] matrix1, float[,] matrix2)
    {
        float[,] result = new float[matrix1.GetLength(0), matrix2.GetLength(1)];
        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                float s = 0;
                for (int k = 0; k < matrix1.GetLength(1); k++)
                {
                    s += matrix1[i, k] * matrix2[k, j];
                }
                result[i, j] = s;
            }
        }
        return result;
    }

    /// <summary>
    /// Applies a 2D homogeneous transformation matrix to a point.
    /// 
    /// IMPORTANT:
    /// - The point is treated as (x, y, 1) in homogeneous coordinates,
    ///   so translation works correctly.
    /// - The Vector3.z component is NOT used as the homogeneous w.
    ///   It is preserved as the geometric depth (plane position).
    /// 
    /// This avoids mixing geometric z with homogeneous w, which would
    /// cause translations to stop working when z != 1.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="point"></param>
    /// <returns>The transformed vector.</returns>
    public Vector3 multiplyHM2D(float[,] matrix, Vector3 point)
    {
        float x = point.x, y = point.y;
        float newX = matrix[0, 0] * x + matrix[0, 1] * y + matrix[0, 2] * 1f;
        float newY = matrix[1, 0] * x + matrix[1, 1] * y + matrix[1, 2] * 1f;
        return new Vector3(newX, newY, point.z);
    }

}

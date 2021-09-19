using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcMatrix
{
    public static Matrix4x4 CalcProjectionMatrix (float angle, float aspect, float near, float far)
    {
        return new Matrix4x4(
            new Vector4(1.0f / Mathf.Tan(angle) / aspect, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f / Mathf.Tan(angle), 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 1.0f / (far - near) * far, 1.0f),
            new Vector4(0.0f, 0.0f, -near / (far - near) * far, 0.0f)
            );
    }

    public static Matrix4x4 CalcTransMatrix(Vector3 trans)
    {
        return new Matrix4x4(
            new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(trans.x, trans.y, trans.z, 1.0f)
            );
    }

    public static Vector3 MultVecMat(Matrix4x4 mat, Vector3 vec)
    {
        Vector4 src = new Vector4(vec.x, vec.y, vec.z, 1.0f);
        Vector4 dst = Vector4.zero;

        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                dst[i] += src[j] * mat[i, j];
            }
        }

        Debug.Log("w = " + dst.w);

        return new Vector3(dst.x / dst.w, dst.y / dst.w, dst.z / dst.w);
    }
}

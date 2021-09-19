using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSisuidai : MonoBehaviour
{
    [SerializeField, Range(0.01f, Mathf.PI / 2.0f)]
    float angle;

    [SerializeField]
    float aspect = 1.0f;

    [SerializeField]
    float near = 1.0f, far = 2.0f;

    [SerializeField]
    Color color;

    [SerializeField]
    bool isProj;

    [SerializeField]
    Vector3 testPoint;
    [SerializeField]
    float testBoxSize;

    [SerializeField]
    Vector3 testVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector4 v4(Vector3 v3, float w)
    {
        return new Vector4(v3.x, v3.y, 1.0f, w);
    }

    //Vector3 divw(Vector4 v4)
    //{
    //    return new Vector3(v4.x, v4.y, v4.z) / v4.w;
    //}

    Vector4 divw(Vector4 v4)
    {
        return v4;
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 proj = CalcMatrix.CalcProjectionMatrix(angle, aspect, near, far);

        Vector3 up = new Vector3(0.0f, Mathf.Tan(angle), 1.0f);
        Vector3 down = new Vector3(0.0f, -Mathf.Tan(angle), 1.0f);
        Vector3 left = new Vector3( -Mathf.Tan(angle) * aspect, 0.0f, 1.0f);
        Vector3 right = new Vector3(Mathf.Tan(angle) * aspect, 0.0f, 1.0f);

        Vector4[] nearPoints = new Vector4[] { v4(up + left, 1.0f), v4(up + right, 1.0f), v4(down + right, 1.0f), v4(down + left, 1.0f) };
        Vector4[] farPoints = new Vector4[] { v4(up + left, 1.0f), v4(up + right, 1.0f), v4(down + right, 1.0f), v4(down + left, 1.0f) };

        for(int i = 0; i < 4; i++)
        {
            nearPoints[i] *= near;
            farPoints[i] *= far;

            if (isProj)
            {
                nearPoints[i] = proj.MultiplyPoint(nearPoints[i]);
                // nearPoints[i] = proj * nearPoints[i];
                farPoints[i] = proj.MultiplyPoint(farPoints[i]);
                // farPoints[i] = proj * farPoints[i];
            }
        }

        Gizmos.color = color;
        for(int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(divw(nearPoints[i]), divw(farPoints[i]));
            Gizmos.DrawLine(divw(nearPoints[i]), divw(nearPoints[(i + 1) % 4]));
            Gizmos.DrawLine(divw(farPoints[i]), divw(farPoints[(i + 1) % 4]));
        }

        return;

        Matrix4x4 invProj = Matrix4x4.Inverse(proj);
        for(int i = 0; i <= 7; i++)
        {
            Vector4 n = new Vector4( i / 7.0f * 2.0f - 1.0f, 0.0f, 0.0f, 1.0f);
            Vector4 f = new Vector4(i / 7.0f * 2.0f - 1.0f, 0.0f, 1.0f, 1.0f);

            if (!isProj)
            {
                n = invProj.MultiplyPoint(n);
                f = invProj.MultiplyPoint(f);
            }

            Gizmos.color = new Color(0.0f, i / 7.0f, 0.0f);
            Gizmos.DrawLine(divw(n), divw(f));
        }

        for (int i = 0; i <= 5; i++)
        {
            Vector3 f = Vector3.Lerp(left, right, i / 5.0f) * far;
            Vector3 n = Vector3.Lerp(left, right, i / 5.0f) * near;

            if (isProj)
            {
                f = proj.MultiplyPoint(f);
                n = proj.MultiplyPoint(n);
            }

            Gizmos.color = new Color(0.0f, 0.0f, i / 5.0f);
            Gizmos.DrawLine(divw(f), divw(n));
        }

        for(int i = 0; i <= 5; i++)
        {
            Vector3 l = new Vector3(-1.0f, 0.0f, i / 5.0f);
            Vector3 r = new Vector3(1.0f, 0.0f, i / 5.0f);

            if (!isProj)
            {
                l = invProj.MultiplyPoint(l);
                r = invProj.MultiplyPoint(r);
            }

            Gizmos.color = new Color(0.0f, 0.0f, i / 5.0f);
            Gizmos.DrawLine(divw(l), divw(r));
        }

        {
            Vector3 point = testPoint;

            if (!isProj)
            {
                point = invProj.MultiplyPoint(point);
            }

            Gizmos.color = Color.black;
            Gizmos.DrawCube(point, Vector3.one * testBoxSize);
        }
    }
}

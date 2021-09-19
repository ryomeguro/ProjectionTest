using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [SerializeField, Range(0.01f, Mathf.PI / 2.0f)]
    float angle;

    [SerializeField]
    float aspect = 1.0f;

    [SerializeField]
    float near = 1.0f, far = 2.0f;

    [SerializeField]
    Color color, color2;

    [SerializeField]
    bool isProj;
    
    [SerializeField]
    Vector3 testPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //Matrix4x4 mat = new Matrix4x4(
        //    new Vector4(1.0f, 2.0f, 0.0f, 0.0f),
        //    new Vector4(3.0f, 1.0f, 0.0f, 0.0f),
        //    Vector4.zero,
        //    Vector4.zero
        //    );

        //Debug.Log("(0,1) = " + mat[0, 1]); // = 3
        //Debug.Log("(1, 0) = " + mat[1, 0]); // = 2

        Matrix4x4 proj = CalcMatrix.CalcProjectionMatrix(angle, aspect, near, far);

        Vector3 up = new Vector3(0.0f, Mathf.Tan(angle), 0.5f);
        Vector3 down = new Vector3(0.0f, -Mathf.Tan(angle), 0.5f);
        Vector3 left = new Vector3(-Mathf.Tan(angle) * aspect, 0.0f, 0.5f);
        Vector3 right = new Vector3(Mathf.Tan(angle) * aspect, 0.0f, 0.5f);

        Vector3[] nearPoints = new Vector3[] { up + left, up + right, down + right, down + left };
        Vector3[] farPoints = new Vector3[] { up + left, up + right, down + right, down + left };

        Debug.Log("View -> Proj------------------------");
        for (int i = 0; i < 4; i++)
        {
            nearPoints[i] *= near;
            farPoints[i] *= far;

            if (isProj)
            {
                nearPoints[i] = CalcMatrix.MultVecMat(proj, nearPoints[i]);
                farPoints[i] = CalcMatrix.MultVecMat(proj, farPoints[i]);
            }
        }

        Gizmos.color = color;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(nearPoints[i], farPoints[i]);
            Gizmos.DrawLine(nearPoints[i], nearPoints[(i + 1) % 4]);
            Gizmos.DrawLine(farPoints[i], farPoints[(i + 1) % 4]);
        }

        Debug.Log("Proj -> View------------------------");
        Matrix4x4 invProj = proj.inverse;
        for (int i = 0; i < 4; i++)
        {
            if (isProj)
            {
                nearPoints[i] = CalcMatrix.MultVecMat(invProj, nearPoints[i]);
                farPoints[i] = CalcMatrix.MultVecMat(invProj, farPoints[i]);
            }
        }

        Gizmos.color = color2;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(nearPoints[i], farPoints[i]);
            Gizmos.DrawLine(nearPoints[i], nearPoints[(i + 1) % 4]);
            Gizmos.DrawLine(farPoints[i], farPoints[(i + 1) % 4]);
        }
    }
}

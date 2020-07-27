using UnityEngine;
using UnityEditor;

public class Lesson_3_1 : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public Vector3 m_Point_1 = Vector3.zero;
    public Vector3 m_Point_2 = Vector3.zero;

    public Vector3 m_MoveVector = Vector2.zero;

    public Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMouseVector = Vector3.zero;

    public Vector3 m_NewPoint_1 = Vector3.zero;
    public Vector3 m_NewPoint_2 = Vector3.zero;

    public Matrix4x4 m_RotationMatrix = new Matrix4x4();


    public int m_MaxDegree = 0;
    public int m_Degree = 0;


    private void OnGUI()
    {
        m_Degree++;
        if (m_Degree == m_MaxDegree)
            m_Degree = 0;

        m_RotationMatrix.m00 = Cos(m_Degree);
        m_RotationMatrix.m01 = (-1) * Sin(m_Degree);
        m_RotationMatrix.m02 = 0;
        m_RotationMatrix.m03 = m_MoveVector.x;

        m_RotationMatrix.m10 = Sin(m_Degree);
        m_RotationMatrix.m11 = Cos(m_Degree);
        m_RotationMatrix.m12 = 0;
        m_RotationMatrix.m13 = m_MoveVector.y;

        m_RotationMatrix.m20 = 0;
        m_RotationMatrix.m21 = 0;
        m_RotationMatrix.m22 = 1;
        m_RotationMatrix.m23 = 0;

        m_RotationMatrix.m30 = 0;
        m_RotationMatrix.m31 = 0;
        m_RotationMatrix.m32 = 0;
        m_RotationMatrix.m33 = 1;

        Handles.matrix = Matrix4x4.identity;

        m_NewPoint_1 = m_RotationMatrix.MultiplyPoint(m_Point_1);
        m_NewPoint_2 = m_RotationMatrix.MultiplyPoint(m_Point_2);
        Handles.color = Color.green;
        Handles.DrawLine(m_NewPoint_1, m_NewPoint_2);
        
        m_MouseVector = Input.mousePosition;
        m_TranMouseVector = m_MouseVector;
        m_TranMouseVector.y = 720f - m_MouseVector.y;

        if (Input.GetMouseButton(0))
        {
            m_RotationMatrix.m03 = m_TranMouseVector.x;
            m_RotationMatrix.m13 = m_TranMouseVector.y;
        }
        else
        {
            m_RotationMatrix.m03 = m_MoveVector.x;
            m_RotationMatrix.m13 = m_MoveVector.y;
        }


        Handles.color = Color.red;
        Handles.matrix = m_RotationMatrix;
        Handles.DrawLine(m_Point_1, m_Point_2);
    }

    float GetRadian(int degreed)
    {
        return degreed * Mathf.PI / 180;
    }

    float Cos(int _degree)
    {
        return Mathf.Cos(GetRadian(_degree));
    }
    float Sin(int _degree)
    {
        return Mathf.Sin(GetRadian(_degree));
    }
}

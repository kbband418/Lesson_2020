using UnityEngine;
using UnityEditor;

public class Lesson_3_2 : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    //目前只有m_Point_1 = = new Vector3(0, 0, 0)成立
    public Vector3 m_Point_1 = new Vector3(0, 0, 0);
    public Vector3 m_Point_2 = new Vector3(200, 0, 0);

    private Vector3 m_ScreenBaseVector = new Vector3(640, 360, 0);

    public Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMouseVector = Vector3.zero;

    private Vector3 m_NewPoint_1 = new Vector3(0, 0, 0);
    private Vector3 m_NewPoint_2 = new Vector3(-100, 0, 0);

    public Matrix4x4 m_RotationMatrix = new Matrix4x4();
    

    private bool m_IsPlaying = false;
    private int m_MaxDegree = 360;
    private int m_Degree = 0;

    public int m_OriginalDegree = 0;
    public int m_TargetDegree = 0;
    public int m_AddDegree = 0;


    private void OnGUI()
    {
        m_OriginalDegree = (int)GetAtan2Degree(m_Point_1, m_Point_2);


        Handles.matrix = Matrix4x4.identity;
        m_RotationMatrix = Matrix4x4.identity;

        m_RotationMatrix.m00 = Cos(m_Degree);
        m_RotationMatrix.m03 = m_ScreenBaseVector.x + m_Point_1.x;

        m_RotationMatrix.m10 = Sin(m_Degree);
        m_RotationMatrix.m11 = Cos(m_Degree);
        m_RotationMatrix.m13 = m_ScreenBaseVector.y + m_Point_1.y;

        //Base Line blue
        Handles.color = Color.blue;
        Handles.DrawLine(m_Point_1 + m_ScreenBaseVector, m_Point_2 + m_ScreenBaseVector);



        if (Input.GetMouseButtonDown(0))
        {
            m_Degree = m_OriginalDegree;
            m_IsPlaying = true;

            m_MouseVector = Input.mousePosition;
            m_TranMouseVector = m_MouseVector;
            m_TranMouseVector.y = 720f - m_MouseVector.y;

            m_TargetDegree = (int)GetAtan2Degree(m_Point_1, m_TranMouseVector - m_ScreenBaseVector);
        }

        if(m_IsPlaying)
        {
            m_Degree += m_AddDegree;
            

            if (180f + m_OriginalDegree > m_TargetDegree + m_OriginalDegree && m_TargetDegree + m_OriginalDegree > 0f + m_OriginalDegree)
            {//UNITY 坐標系 順時針
                m_RotationMatrix.m01 = (1) * Sin(m_Degree);
                m_AddDegree = 1;
            }
            else if (-180f + m_OriginalDegree < m_TargetDegree + m_OriginalDegree && m_TargetDegree + m_OriginalDegree < 0f + m_OriginalDegree)
            {//UNITY 坐標系 逆時針
                m_RotationMatrix.m01 = (-1) * Sin(m_Degree);
                m_AddDegree = -1;
            }

            m_NewPoint_1 = m_Point_1 + m_ScreenBaseVector;
            m_NewPoint_2 = m_RotationMatrix.MultiplyPoint(m_Point_2);
            Handles.color = Color.green;
            Handles.DrawLine(m_NewPoint_1, m_NewPoint_2);

            if(m_Degree == m_TargetDegree)
            {
                m_IsPlaying = false;
            }
        }
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

    float GetAtan2Degree(Vector3 _Point1, Vector3 _Point2)
    {
        return (Mathf.Atan2((_Point2.y - _Point1.y), (_Point2.x - _Point1.x))) * 180 / Mathf.PI;
    }
}

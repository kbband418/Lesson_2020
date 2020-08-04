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

    private Vector3 m_ScreenBaseVector = new Vector3(0, 0, 0);

    private Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMouseVector = Vector3.zero;

    private Vector3 m_NewPoint_1 = new Vector3(0, 0, 0);
    private Vector3 m_NewPoint_2 = new Vector3(0, 0, 0);

    public Matrix4x4 m_RotationMatrix = new Matrix4x4();

    public bool m_IsLeft = false;

    private bool m_IsPlaying = false;
    private int m_Degree = 0;

    public int m_OriginalDegree = 0;
    public int m_TargetDegree = 0;
    public int mDifferenceDegree = 0;
    public int m_AddDegree = 0;


    private void OnGUI()
    {
        m_OriginalDegree = (int)GetAtan2Degree(m_Point_1, m_Point_2);
        m_ScreenBaseVector.x = Screen.width / 2;
        m_ScreenBaseVector.y = Screen.height / 2;
        Vector3 m_TempPoint_1 = Vector3.zero;
        Vector3 m_TempPoint_2 = Vector3.zero;

        //Base Line blue
        Handles.matrix = Matrix4x4.identity;
        Handles.color = Color.blue;

        //基準點轉直角坐標系
        m_TempPoint_1 = m_Point_1;
        m_TempPoint_2 = new Vector3(m_Point_2.x, (-1) * m_Point_2.y, m_Point_2.z);
        Handles.DrawLine(m_TempPoint_1 + m_ScreenBaseVector, m_TempPoint_2 + m_ScreenBaseVector);

        if (Input.GetMouseButtonDown(0))
        {
            m_Degree = 0;
            m_IsPlaying = true;

            m_MouseVector = Input.mousePosition;
            m_TranMouseVector = m_MouseVector;
            m_TranMouseVector.x = m_MouseVector.x - m_ScreenBaseVector.x;
            m_TranMouseVector.y = m_MouseVector.y - m_ScreenBaseVector.y;

            m_TargetDegree = (int)GetAtan2Degree(m_Point_1, m_TranMouseVector);
            m_IsLeft = CheckIsLeft(m_Point_1, m_Point_2, m_TranMouseVector);

            if(m_IsLeft)
            {//直角坐標系 逆時針
                if (m_TargetDegree < 0 && m_OriginalDegree < 0)
                    mDifferenceDegree = (-1) * (m_TargetDegree - m_OriginalDegree);
                else
                    mDifferenceDegree = (-1) * (((m_TargetDegree < 0) ? m_TargetDegree + 360 : m_TargetDegree) - m_OriginalDegree);
                m_AddDegree = -1;
            }
            else
            {//直角坐標系 順時針
                if (m_TargetDegree < 0 && m_OriginalDegree < 0)
                    mDifferenceDegree = (m_OriginalDegree - m_TargetDegree);
                else
                    mDifferenceDegree = (((m_OriginalDegree < 0) ? m_OriginalDegree + 360 : m_OriginalDegree) - m_TargetDegree);
                m_AddDegree = 1;
            }
        }

        Handles.matrix = Matrix4x4.identity;
        if (m_IsPlaying)
        {
            m_Degree += m_AddDegree;


            m_RotationMatrix = Matrix4x4.identity;

            m_RotationMatrix.m00 = Cos(m_Degree);
            m_RotationMatrix.m01 = (-1) * Sin(m_Degree);
            m_RotationMatrix.m03 = m_ScreenBaseVector.x;

            m_RotationMatrix.m10 = Sin(m_Degree);
            m_RotationMatrix.m11 = Cos(m_Degree);
            m_RotationMatrix.m13 = m_ScreenBaseVector.y;

            m_NewPoint_1 = m_RotationMatrix.MultiplyPoint(m_TempPoint_1);
            m_NewPoint_2 = m_RotationMatrix.MultiplyPoint(m_TempPoint_2);
            Handles.color = Color.green;
            Handles.DrawLine(m_NewPoint_1, m_NewPoint_2);

            if (m_Degree == mDifferenceDegree)
                m_IsPlaying = false;
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

    bool CheckIsLeft(Vector3 _Point1, Vector3 _Point2, Vector3 _CheckPoint)
    {
        return ((_Point2.x - _Point1.x) * (_CheckPoint.y - _Point1.y) - (_Point2.y - _Point1.y) * (_CheckPoint.x- _Point1.x)) > 0;
    }
}

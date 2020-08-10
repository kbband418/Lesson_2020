using UnityEngine;
using UnityEditor;

public class Lesson_3_2 : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public Vector3 m_BasePoint_1 = new Vector3(0, 0, 0);
    public Vector3 m_BasePoint_2 = new Vector3(200, 0, 0);

    private Vector3 m_ScreenBaseVector = new Vector3(0, 0, 0);

    public Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMousePoint = Vector3.zero;

    private Vector3 m_NewPoint_1 = new Vector3(0, 0, 0);
    private Vector3 m_NewPoint_2 = new Vector3(0, 0, 0);

    public Matrix4x4 m_RotationMatrix = new Matrix4x4();

    public bool m_IsLeft = false;

    public bool m_IsPlaying = false;
    public int m_Degree = 0;

    public float m_OriginalRadian = 0;
    public int m_OriginalDegree = 0;
    private float m_TargetRadian = 0;
    private int m_TargetDegree = 0;
    public float m_TempTargetRadian = 0;
    public int m_TempTargetDegree = 0;
    public int mDifferenceDegree = 0;
    public int m_AddDegree = 0;


    private void OnGUI()
    {
        m_OriginalRadian = Mathf.Atan2(m_BasePoint_2.y - m_BasePoint_1.y, m_BasePoint_2.x - m_BasePoint_1.x);
        m_OriginalDegree = (int)(m_OriginalRadian * 180 / Mathf.PI);
        m_ScreenBaseVector.x = Screen.width / 2;
        m_ScreenBaseVector.y = Screen.height / 2;

        //Base Line blue
        Handles.matrix = Matrix4x4.identity;
        Handles.color = Color.blue;

        //基準點轉直角坐標系
        Vector3 m_TempBasePoint_1 = new Vector3(m_BasePoint_1.x, (-1) * m_BasePoint_1.y, m_BasePoint_1.z);
        Vector3 m_TempBasePoint_2 = new Vector3(m_BasePoint_2.x, (-1) * m_BasePoint_2.y, m_BasePoint_2.z);
        Handles.DrawLine(m_TempBasePoint_1 + m_ScreenBaseVector, m_TempBasePoint_2 + m_ScreenBaseVector);

        if (Input.GetMouseButtonDown(0))
        {
            m_Degree = 0;
            m_IsPlaying = true;

            m_MouseVector = Input.mousePosition;
            //取得滑鼠點擊位置直角坐標系
            m_TranMousePoint = m_MouseVector;
            m_TranMousePoint.x = m_MouseVector.x - m_ScreenBaseVector.x - m_BasePoint_1.x;
            m_TranMousePoint.y = m_MouseVector.y - m_ScreenBaseVector.y - m_BasePoint_1.y;

            m_TargetRadian = Mathf.Atan2(m_TranMousePoint.y, m_TranMousePoint.x);
            m_TargetDegree = (int)(m_TargetRadian * 180 / Mathf.PI);

            //依照滑鼠點擊位置將基礎點轉換成起始點
            Vector3 m_TempTranMousePoint = Vector3.zero;

            m_RotationMatrix = Matrix4x4.identity;

            m_RotationMatrix.m00 = Cos(m_OriginalRadian);
            m_RotationMatrix.m01 = Sin(m_OriginalRadian);
            m_RotationMatrix.m10 = -Sin(m_OriginalRadian);
            m_RotationMatrix.m11 = Cos(m_OriginalRadian);

            m_TempTranMousePoint = m_RotationMatrix.MultiplyPoint(m_TranMousePoint);

            //取得滑鼠點擊位置與基準點相差角度
            m_TempTargetRadian = Mathf.Atan2(m_TempTranMousePoint.y, m_TempTranMousePoint.x);
            m_TempTargetDegree = (int)(m_TempTargetRadian * 180 / Mathf.PI);

            mDifferenceDegree = -m_TempTargetDegree;
            if (m_TempTargetDegree > 0)
            {//直角坐標系 逆時針
                m_AddDegree = -1;
            }
            else
            {//直角坐標系 順時針
                m_AddDegree = 1;
            }
        }

        if (m_IsPlaying)
        {
            Handles.matrix = Matrix4x4.identity;
            m_Degree += m_AddDegree;

            m_RotationMatrix = Matrix4x4.identity;

            m_RotationMatrix.m00 = Cos(m_Degree);
            m_RotationMatrix.m01 = (-1) * Sin(m_Degree);
            m_RotationMatrix.m03 = m_ScreenBaseVector.x + m_BasePoint_1.x;//將點1當為原點，旋轉完後再位移到點1的位置

            m_RotationMatrix.m10 = Sin(m_Degree);
            m_RotationMatrix.m11 = Cos(m_Degree);
            m_RotationMatrix.m13 = m_ScreenBaseVector.y - m_BasePoint_1.y;//將點1當為原點，旋轉完後再位移到點1的位置

            //將點1當成原點
            m_TempBasePoint_1 = Vector3.zero;
            m_TempBasePoint_2 = new Vector3(m_BasePoint_2.x - m_BasePoint_1.x, (-1) * (m_BasePoint_2.y - m_BasePoint_1.y), m_BasePoint_2.z - m_BasePoint_1.z);
            m_NewPoint_1 = m_RotationMatrix.MultiplyPoint(m_TempBasePoint_1);
            m_NewPoint_2 = m_RotationMatrix.MultiplyPoint(m_TempBasePoint_2);
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
    float Cos(float _radian)
    {
        return Mathf.Cos(_radian);
    }
    float Sin(float _radian)
    {
        return Mathf.Sin(_radian);
    }

    float GetAtan2Degree(Vector3 _Point1, Vector3 _Point2)
    {
        return (Mathf.Atan2((_Point2.y - _Point1.y), (_Point2.x - _Point1.x))) * 180 / Mathf.PI;
    }

    bool CheckIsLeft(Vector3 _Point1, Vector3 _Point2, Vector3 _CheckPoint)
    {
        return ((_Point2.x - _Point1.x) * (_CheckPoint.y - _Point1.y) - (_Point2.y - _Point1.y) * (_CheckPoint.x - _Point1.x)) > 0;
    }
}

using UnityEditor;
using UnityEngine;

public class Lesson_4 : MonoBehaviour
{
    public Vector3 m_Point_1 = Vector3.zero;
    public Vector3 m_Point_2 = Vector3.zero;
    public Vector3 m_Point_3 = Vector3.zero;

    private Vector3 m_ScreenBaseVector = new Vector3(0, 0, 0);

    public Vector3 m_MousePoint = Vector3.zero;
    public Vector3 m_TranMousePoint = Vector3.zero;
    
    public bool m_IsLeft_P12 = false;
    public bool m_IsLeft_P23 = false;
    public bool m_IsLeft_P31 = false;

    public bool m_isIntTriangle = false;


    public void Awake()
    {
        Application.targetFrameRate = 30;
        m_ScreenBaseVector.x = Screen.width / 2;
        m_ScreenBaseVector.y = Screen.height / 2;
    }

    private void OnGUI()
    {
        //直角坐標系轉成UNITY繪圖坐標系
        //Handles.color = Color.green;
        Handles.DrawLine(GetCartesianVector(m_Point_1), GetCartesianVector(m_Point_2));
        //Handles.color = Color.red;
        Handles.DrawLine(GetCartesianVector(m_Point_2), GetCartesianVector(m_Point_3));
        //Handles.color = Color.blue;
        Handles.DrawLine(GetCartesianVector(m_Point_3), GetCartesianVector(m_Point_1));

        //取得滑鼠點擊位置直角坐標系
        m_MousePoint = Input.mousePosition;
        m_TranMousePoint = m_MousePoint;
        m_TranMousePoint.x = m_MousePoint.x - m_ScreenBaseVector.x;
        m_TranMousePoint.y = m_MousePoint.y - m_ScreenBaseVector.y;

        //檢驗滑鼠座標是否在向量的左邊
        m_IsLeft_P12 = CheckIsLeft(m_Point_1, m_Point_2, m_TranMousePoint);
        m_IsLeft_P23 = CheckIsLeft(m_Point_2, m_Point_3, m_TranMousePoint);
        m_IsLeft_P31 = CheckIsLeft(m_Point_3, m_Point_1, m_TranMousePoint);

        //如果座標都在三個向量的左邊，則代表在三角形內
        m_isIntTriangle = (m_IsLeft_P12 == true && m_IsLeft_P23 == true && m_IsLeft_P31 == true) ? true : false;

        if (m_isIntTriangle)
        {
            Handles.color = Color.green;
        }
        else
        {
            Handles.color = Color.white;
        }
    }

    private Vector3 GetCartesianVector(Vector3 _OriginVector)
    {
        Vector3 tempVector = new Vector3(_OriginVector.x, (-1) * _OriginVector.y, _OriginVector.z);

        return tempVector + m_ScreenBaseVector;
    }

    private bool CheckIsLeft(Vector3 _Point1, Vector3 _Point2, Vector3 _CheckPoint)
    {
        return ((_Point2.x - _Point1.x) * (_CheckPoint.y - _Point1.y) - (_Point2.y - _Point1.y) * (_CheckPoint.x - _Point1.x)) > 0;
    }
}

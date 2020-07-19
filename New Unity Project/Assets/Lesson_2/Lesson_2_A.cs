using UnityEngine;
using UnityEditor;

public class Lesson_2_A : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public Vector3 m_Point_1 = Vector3.zero;
    public Vector3 m_Point_2 = Vector3.zero;

    public Vector3 m_MoveVector = Vector2.zero;

    public Vector3 m_NewPoint_1 = Vector3.zero;
    public Vector3 m_NewPoint_2 = Vector3.zero;

    public int m_MaxDegree = 0;
    public int m_Degree = 0;


    private void OnGUI()
    {
        m_Degree++;
        if (m_Degree == m_MaxDegree)
            m_Degree = 0;
        m_NewPoint_1.x = m_MoveVector.x + Mathf.Cos(GetRadian(m_Degree)) * m_Point_1.x + (-1) * Mathf.Sin(GetRadian(m_Degree)) * m_Point_1.y;
        m_NewPoint_1.y = (1) * (m_MoveVector.y) + Mathf.Sin(GetRadian(m_Degree)) * m_Point_1.x + Mathf.Cos(GetRadian(m_Degree)) * m_Point_1.y;
        m_NewPoint_2.x = m_MoveVector.x + Mathf.Cos(GetRadian(m_Degree)) * m_Point_2.x + (-1) * Mathf.Sin(GetRadian(m_Degree)) * m_Point_2.y;
        m_NewPoint_2.y = (1) * (m_MoveVector.y) + Mathf.Sin(GetRadian(m_Degree)) * m_Point_2.x + Mathf.Cos(GetRadian(m_Degree)) * m_Point_2.y;
     
        Handles.DrawLine(m_NewPoint_1, m_NewPoint_2);
    }

    float GetRadian(int degreed)
    {
        return degreed * Mathf.PI / 180;
    }
}

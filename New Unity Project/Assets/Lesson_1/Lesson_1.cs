using UnityEngine;
using UnityEditor;

public class Lesson_1 : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public Vector3 m_Point_1 = Vector3.zero;
    public Vector3 m_Point_2 = Vector3.zero;
    public Vector3 m_Point_3 = Vector3.zero;

    public Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMouseVector = Vector3.zero;

    public bool m_isIntTriangle = false;

    private void OnGUI()
    {
        Handles.DrawLine(m_Point_1, m_Point_2);
        Handles.DrawLine(m_Point_2, m_Point_3);
        Handles.DrawLine(m_Point_3, m_Point_1);

        m_MouseVector = Input.mousePosition;
        m_TranMouseVector = m_MouseVector;
        m_TranMouseVector.y = 720f - m_MouseVector.y;

        m_isIntTriangle = InThisTriangle(m_TranMouseVector, m_Point_1, m_Point_2, m_Point_3);

        if (m_isIntTriangle)
        {
            Handles.color = Color.green;
        }
        else
        {
            Handles.color = Color.white;
        }
    }

    bool InThisTriangle(Vector3 _MousePoint, Vector3 _Point1, Vector3 _Point2, Vector3 _Point3)
    {
        float UHat = 0f;
        float VHat = 0f;

        VHat = (_MousePoint.x * (_Point2.y - _Point1.y) - _Point1.x * (_Point2.y - _Point1.y) - _MousePoint.y * (_Point2.x - _Point1.x) + _Point1.y * (_Point2.x - _Point1.x)) / ((_Point3.x - _Point1.x) * (_Point2.y - _Point1.y) - (_Point3.y - _Point1.y) * (_Point2.x - _Point1.x));
        UHat = (_MousePoint.y - _Point1.y - VHat * (_Point3.y - _Point1.y)) / (_Point2.y - _Point1.y);


        if (UHat >= 0f && VHat >= 0f && (UHat + VHat) <= 1f)
        {
            return true;
        }

        return false;
    }
}

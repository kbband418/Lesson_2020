using UnityEngine;
using UnityEditor;

public class Lesson_5 : MonoBehaviour
{
    public void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public Vector3 m_BasePoint_1 = new Vector3(0, 0, 0);


    public Vector3 m_MouseVector = Vector3.zero;
    public Vector3 m_TranMousePoint = Vector3.zero;





    [Header("扇形長度")]
    public float m_fLineLength = 100f;
    [Header("扇形角度")]
    [Range(0, 360)]
    public int m_iCircleAngle = 1;
    [Header("旋轉速度")]
    public int m_iRotateSpeed = 0;

    private Vector3 m_ScreenBaseVector = new Vector3(0, 0, 0);
    private Matrix4x4 m_RotationMatrix = new Matrix4x4();
    private int m_iAddRotateDegree = 0;
    private float m_fDegree = 0;
    private int m_iLineDensity = 3;


    void OnGUI()
    {
        //畫面基準點
        m_ScreenBaseVector.x = Screen.width / 2;
        m_ScreenBaseVector.y = Screen.height / 2;

        //以目標扇形角度算出左右起始及終點角度，表示扇形張開範圍
        int TempDegree = (m_iCircleAngle % 2 != 0) ? (m_iCircleAngle - 1) : m_iCircleAngle;

        //概念以很多條線組成，畫出扇形，依照密度來算出需要使用多少條線來畫扇形
        int LineCount = m_iCircleAngle * m_iLineDensity;

        //每條線間隔的角度是多少
        float DegreePerLine = ((float)TempDegree) / LineCount;

        //依據長度設定求基準點2的座標是多少
        Vector3 BasePoint_2 = Vector3.zero;
        BasePoint_2.x = m_BasePoint_1.x;
        BasePoint_2.y = m_BasePoint_1.y + m_fLineLength;
        BasePoint_2.z = m_BasePoint_1.z;

        //目前旋轉幾度
        m_iAddRotateDegree += m_iRotateSpeed;
        if (m_iAddRotateDegree >= 360 || m_iAddRotateDegree <= -360) m_iAddRotateDegree = 0;

        //起始角度
        int iStartDegree = (-1) * (TempDegree / 2);
        
        //基準點1、2轉成原點的暫存
        Vector3 TempBasePoint_1 = Vector3.zero;
        Vector3 TempBasePoint_2 = Vector3.zero;

        //將基準點1、2旋轉以及位移後的新點1、2
        Vector3 NewPoint_1 = Vector3.zero;
        Vector3 NewPoint_2 = Vector3.zero;

        //扇形起始向量
        Vector3 StartVector = Vector3.zero;
        //扇形結尾向量
        Vector3 EndVector = Vector3.zero;


        for (int index = 0; index <= LineCount; index ++)
        {
            m_RotationMatrix = Matrix4x4.identity;

            //這條線要旋轉幾度
            m_fDegree = iStartDegree + DegreePerLine * index + m_iAddRotateDegree;

            m_RotationMatrix.m00 = Cos(m_fDegree);
            m_RotationMatrix.m01 = (-1) * Sin(m_fDegree);
            //將基準點1當為原點，旋轉完後再位移到(螢幕原點 + 基準點1)的位置
            m_RotationMatrix.m03 = m_ScreenBaseVector.x + m_BasePoint_1.x;

            m_RotationMatrix.m10 = Sin(m_fDegree);
            m_RotationMatrix.m11 = Cos(m_fDegree);
            //將基準點1當為原點，旋轉完後再位移到(螢幕原點 + 基準點1)的位置
            m_RotationMatrix.m13 = m_ScreenBaseVector.y - m_BasePoint_1.y;

            //將基準點1當成原點
            TempBasePoint_1 = Vector3.zero;
            TempBasePoint_2 = new Vector3(BasePoint_2.x - m_BasePoint_1.x, (-1) * (BasePoint_2.y - m_BasePoint_1.y), BasePoint_2.z - m_BasePoint_1.z);
            NewPoint_1 = m_RotationMatrix.MultiplyPoint(TempBasePoint_1);
            NewPoint_2 = m_RotationMatrix.MultiplyPoint(TempBasePoint_2);
            Handles.color = Color.green;
            Handles.DrawLine(NewPoint_1, NewPoint_2);
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_MouseVector = Input.mousePosition;
            //取得滑鼠點擊位置直角坐標系
            m_TranMousePoint = m_MouseVector;
            m_TranMousePoint.x = m_MouseVector.x - m_ScreenBaseVector.x - m_BasePoint_1.x;
            m_TranMousePoint.y = m_MouseVector.y - m_ScreenBaseVector.y - m_BasePoint_1.y;

            float CosValue = CosThetaValue(BasePoint_2 - m_BasePoint_1, m_TranMousePoint - m_BasePoint_1);
            Debug.Log(CosValue);
        }
    }



    float GetRadian(float degreed)
    {
        return degreed * Mathf.PI / 180;
    }

    float Cos(float _degree)
    {
        return Mathf.Cos(GetRadian(_degree));
    }
    float Sin(float _degree)
    {
        return Mathf.Sin(GetRadian(_degree));
    }

    float CosThetaValue(Vector3 _Vec1, Vector3 _Vec2)
    {
        return VectorDot(_Vec1, _Vec2) / (VectorLength(_Vec1) * VectorLength(_Vec2));
    }

    float VectorDot(Vector3 _Vec1, Vector3 _Vec2)
    {
        return Vector3.Dot(_Vec1, _Vec2);
    }

    float VectorLength(Vector3 _Vec)
    {
        return _Vec.magnitude;
    }
}

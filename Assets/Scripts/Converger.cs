using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Converger : MonoBehaviour
{
    public float PositionTime = 0.1f;
    public float AngleTime = 0.1f;
    float m_totalTime;

    Vector3 m_targetPos;
    Vector3 m_startPos;
    float m_startAngle;
    float m_targetAngle;
    float m_time;
    bool m_isConverging = false;
    bool m_wasAngle = false;

    public Vector3 TargetPos { get { return m_targetPos; } }
    public float TargetAngle { get { return m_targetAngle; } }
    public bool IsConverging { get { return m_isConverging; } }

    // Start is called before the first frame update
    void Start()
    {
        m_totalTime = PositionTime + AngleTime;
    }

    void FixedUpdate()
    {
        // TODO keep converging until reached close enough rotation, fixed angle step
        // TODO do not lock position before fixing rotation of object (not just rigidbody)

        if (m_isConverging)
        {
            bool isAngle = m_time < AngleTime;
            if (isAngle)
            {
                float lerped = Mathf.LerpAngle(m_startAngle, m_targetAngle, m_time / AngleTime);
                GetComponent<Rigidbody2D>().MoveRotation(lerped);
            }
            else
            {
                if (m_wasAngle)
                {
                    m_startPos = transform.position;
                    m_targetPos.x = Mathf.Round(m_startPos.x);
                    m_targetPos.y = Mathf.Round(m_startPos.y);
                    m_targetPos.z = m_startPos.y;
                }

                GetComponent<Rigidbody2D>().MoveRotation(m_targetAngle);

                if (m_time >= m_totalTime)
                {
                    m_isConverging = false;
                    GetComponent<Rigidbody2D>().MovePosition(m_targetPos);
                }
                else
                {
                    Vector2 lerped = Vector2.Lerp(m_startPos, m_targetPos, (m_time - AngleTime) / PositionTime);
                    GetComponent<Rigidbody2D>().MovePosition(lerped);
                }

                // TODO keep moving and rotating to target for extra time
            }

            m_wasAngle = isAngle;
        }

        m_time += Time.fixedDeltaTime;
    }

    public void Converge()
    {
        m_startAngle = transform.eulerAngles.z;

        if (m_targetAngle < 0) m_startAngle += 360.0f;

        if (m_startAngle < 45)
            m_targetAngle = 0;
        else if (m_startAngle >= 315)
            m_targetAngle = 0;
        else if (m_startAngle >= 45 && m_startAngle < 135)
            m_targetAngle = 90;
        else if (m_startAngle >= 135 && m_startAngle < 225)
            m_targetAngle = 180;
        else if (m_startAngle >= 225 && m_startAngle < 315)
            m_targetAngle = 270;

        m_isConverging = true;
        m_time = 0.0f;
    }
}

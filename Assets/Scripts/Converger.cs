using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Converger : MonoBehaviour
{
    float PositionTime = 0.05f;
    float AngleTime = 0.05f;
    float Iterations = 4;
    float m_totalTime;

    Vector2 m_targetPos;
    Vector2 m_startPos;
    float m_startAngle;
    float m_targetAngle;
    float m_time;
    bool m_isConverging = false;
    bool m_wasAngle = false;
    bool m_wasConverging = false;
    bool m_isFailed = false;

    public Vector2 TargetPos { get { return m_targetPos; } }
    public float TargetAngle { get { return m_targetAngle; } }
    public bool IsConverging { get { return m_isConverging; } }
    public bool IsFailed { get { return m_isFailed; } }

    // Start is called before the first frame update
    void Start()
    {
        m_totalTime = PositionTime * Iterations + AngleTime * Iterations;
    }

    void FixedUpdate()
    {
        // TODO keep converging until reached close enough rotation, fixed angle step
        // TODO do not lock position before fixing rotation of object (not just rigidbody)

        if (m_isConverging)
        {
            bool isAngle = m_time < AngleTime * Iterations;
            if (isAngle)
            {
                float t = m_time / AngleTime;
                if (t > 1) t = 1;
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
                }

                GetComponent<Rigidbody2D>().MoveRotation(m_targetAngle);

                if (m_time >= m_totalTime)
                {
                    m_isConverging = false;
                }

                float posTime = (m_time - AngleTime * Iterations);
                float t = posTime / PositionTime;
                if (t > 1) t = 1;
                if (t < 1)
                {
                    Vector2 lerped = Vector2.Lerp(m_startPos, m_targetPos, t);
                    GetComponent<Rigidbody2D>().MovePosition(lerped);
                }
                else
                {
                    GetComponent<Rigidbody2D>().MovePosition(m_targetPos);
                }
            }

            m_wasAngle = isAngle;
        }

        if (!m_isConverging && m_wasConverging)
        {
            Vector2 rigidBodyPosition = GetComponent<Rigidbody2D>().position;
            float rigidBodyRotation = GetComponent<Rigidbody2D>().rotation;

            float distanceRemain = Vector2.Distance(rigidBodyPosition, m_targetPos);
            float angleRemain = Mathf.DeltaAngle(rigidBodyRotation, m_targetAngle);

            m_isFailed = distanceRemain > 0.1f || angleRemain > 5;
        }

        m_time += Time.fixedDeltaTime;
        m_wasConverging = m_isConverging;
    }

    public void Converge()
    {
        GetComponent<FrictionJoint2D>().enabled = false;

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

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

    public Vector3 TargetPos { get { return m_targetPos; } }
    public float TargetAngle { get { return m_targetAngle; } }
    public bool IsConverging { get { return m_state != State.FINISHED; } }

    enum State
    {
        INIT,
        ROTATION,
        POSITION,
        FINISHED
    }

    State m_state = State.FINISHED;

    // Start is called before the first frame update
    void Start()
    {
        m_totalTime = PositionTime + AngleTime;
    }

    void ChangeState(State state)
    {
        if (state == State.INIT)
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
        }
        else if (state == State.ROTATION)
        {
        }
        else if (state == State.POSITION)
        {
            gameObject.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.Euler(0, 0, TargetAngle));
            m_targetPos.x = Mathf.Round(gameObject.transform.position.x);
            m_targetPos.y = Mathf.Round(gameObject.transform.position.y);
        }
        else if (state == State.FINISHED)
        {
            gameObject.transform.SetPositionAndRotation(m_targetPos, Quaternion.Euler(0, 0, TargetAngle));
        }
        m_state = state;
    }

    private void Update()
    {
        // control converge state
        if (m_state == State.INIT)
        {
            ChangeState(State.ROTATION);
        }
        else if (m_state == State.ROTATION)
        {
            float currentAngle = gameObject.transform.eulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(currentAngle, m_targetAngle);

            if (deltaAngle < 5)
            {
                ChangeState(State.POSITION);
            }
        }
        else if (m_state == State.POSITION)
        {
            float deltaPosition = Vector2.Distance(gameObject.transform.position, m_targetPos);
            if (deltaPosition < 0.05f)
            {
                ChangeState(State.FINISHED);
            }
        }
    }

    void FixedUpdate()
    {
        // keep converging until reached close enough rotation, fixed angle step

        if (m_state == State.ROTATION)
        {
            float currentAngle = GetComponent<Rigidbody2D>().rotation;
            float deltaAngle = Mathf.DeltaAngle(currentAngle, m_targetAngle);
            float targetAngle = Mathf.LerpAngle(currentAngle, m_targetAngle, 0.1f);

            if (Mathf.Abs(deltaAngle) < 2) targetAngle = m_targetAngle;

            GetComponent<Rigidbody2D>().MoveRotation(targetAngle);
        }
        else if (m_state == State.POSITION)
        {
            Vector2 target = m_targetPos;
            Vector2 diff = target - GetComponent<Rigidbody2D>().position;
            if (diff.magnitude > 0.1f)
            {
                diff = diff.normalized * 0.1f;
                target = GetComponent<Rigidbody2D>().position + diff;
            }
            GetComponent<Rigidbody2D>().MovePosition(target);
        }

        m_time += Time.fixedDeltaTime;
    }

    public void Converge()
    {
        ChangeState(State.INIT);
        m_time = 0.0f;
    }
}

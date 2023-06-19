using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWithTouchOrClick : MonoBehaviour
{
    private Vector2 m_touchStart;
    private Vector2 m_touchPosition;
    private bool m_dragging;
    private bool m_wasDragging;

    public BlockParent m_parent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            m_touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            m_touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Collider2D col = GetComponent<Collider2D>();
        bool overLaps = col.OverlapPoint(m_touchPosition);

        if (!overLaps && !m_dragging)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                m_touchStart = m_touchPosition;
                m_dragging = true;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log(m_touchPosition.x + " " + m_touchPosition.y);
                m_dragging = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_dragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_dragging = false;
        }

        if (m_dragging)
        {
            GetComponent<Rigidbody2D>().MovePosition(m_touchPosition);
        }

        if (m_dragging && !m_wasDragging)
        {
            Stage.GetInstance().OnBlockStartMove(m_parent.gameObject);
        }
        else if (!m_dragging && m_wasDragging)
        {
            Stage.GetInstance().OnBlockEndMove(m_parent.gameObject);
        }

        m_wasDragging = m_dragging;
    }
}

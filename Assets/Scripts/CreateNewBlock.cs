using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewBlock : MonoBehaviour
{
    public Transform m_objectsParent;
    public GameObject[] m_blockPrefab;

    private Vector2 m_touchPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    m_touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        //}
        //else
        //{
        //    m_touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //}

        //Collider2D col = GetComponent<Collider2D>();
        //bool overLaps = col.OverlapPoint(m_touchPosition);

        //if (!overLaps)
        //{
        //    return;
        //}

        //bool create = false;

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        create = true;
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    create = true;
        //}

        //if (create)
        //{
        //    CreateBlock();
        //}
    }

    public void CreateBlock()
    {
        int rndBlock = Random.Range(0, m_blockPrefab.Length);
        GameObject go = Instantiate(m_blockPrefab[rndBlock], new Vector3(0, -2.6f, 0), Quaternion.identity, m_objectsParent);
        ApplyRigidbodySettings(go);
    }

    void ApplyRigidbodySettings(GameObject go)
    {
        Rigidbody2D[] rigidBodies = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D p in rigidBodies)
        {
            p.interpolation = RigidbodyInterpolation2D.Interpolate;
            p.drag = 3;
            p.angularDrag = 3;
        }

        FrictionJoint2D[] frictionJoints = GetComponentsInChildren<FrictionJoint2D>();
        foreach (FrictionJoint2D p in frictionJoints)
        {
            p.maxForce = 1000;
            p.maxTorque = 1000;
        }

    }
}

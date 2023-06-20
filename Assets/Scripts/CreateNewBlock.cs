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

    }

    public void CreateBlock()
    {
        GameObject prefab = AnimalQueue.GetInstance().Pop();
        if (prefab == null)
            return;

        GameObject go = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, m_objectsParent);
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

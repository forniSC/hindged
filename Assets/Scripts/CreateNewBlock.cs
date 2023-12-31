using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewBlock : MonoBehaviour
{
    public Transform m_objectsParent;

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

        GameObject go = Instantiate(prefab, GetEntryPoint(), Quaternion.identity, m_objectsParent);
        ApplyRigidbodySettings(go);
    }

    Vector3 GetEntryPoint()
    {
        return Level.GetInstance().GetEntryPoint();
    }

    void ApplyRigidbodySettings(GameObject go)
    {
        Rigidbody2D[] rigidBodies = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D p in rigidBodies)
        {
            p.interpolation = RigidbodyInterpolation2D.Interpolate;
            p.drag = 10;
            p.angularDrag = 10;
        }

        FrictionJoint2D[] frictionJoints = GetComponentsInChildren<FrictionJoint2D>();
        foreach (FrictionJoint2D p in frictionJoints)
        {
            p.maxForce = Stage.GetInstance().FMaxForce;
            p.maxTorque = Stage.GetInstance().FMaxTorque;
        }

    }
}

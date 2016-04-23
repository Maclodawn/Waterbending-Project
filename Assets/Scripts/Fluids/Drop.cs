using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Drop/*Movement*/ : NetworkBehaviour
{
    public Vector3 m_velocity;
    public float m_initTime = 0.2f;
    [System.NonSerialized]
    public float m_initialSpeed;

    private List<GameObject> m_initCollisions = new List<GameObject>();

    public WaterGroup m_waterGroup;

    private List<MonoBehaviour> m_effectors = new List<MonoBehaviour>();
    [System.NonSerialized]
    public DropGravity m_dropGravity;
    [System.NonSerialized]
    public DropTarget m_dropTarget;
    [System.NonSerialized]
    public RotateEffector m_dropRotate;

    public ParticleSystem particles;

    public float radius { get { return transform.localScale.x/2; } }

    public Vector3 velocity
    {
        get
        {
            return m_velocity;
        }
    }

    [Server]
    public void registerGravity(DropGravity _effector)
    {
        registerEffector(_effector);
        m_dropGravity = _effector;
    }

    [Server]
    public void registerTarget(DropTarget _effector)
    {
        registerEffector(_effector);
        m_dropTarget = _effector;
    }

    [Server]
    public void registerRotate(RotateEffector _effector)
    {
        registerEffector(_effector);
        m_dropRotate = _effector;
    }

    [Server]
    public void registerEffector(MonoBehaviour _effector)
    {
        m_effectors.Add(_effector);
    }

    [Server]
    public void removeEffectors()
    {
        foreach(MonoBehaviour effector in m_effectors)
            Destroy(effector);
        m_effectors.Clear();
        
        m_dropGravity = null;
        m_dropTarget = null;
        m_dropRotate = null;
    }

    void Start()
    {
        if (NetworkClient.active)
        {
            Instantiate(Manager.getInstance().m_dropParticlesPrefab).GetComponent<DropParticles>().drop = this;
        }
        else if (NetworkServer.active)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Used ONLY for initialization, otherwise use AddForce
    [Server]
    public void init(Vector3 _position, WaterGroup _waterGroup, float _speed)
    {
        name += _waterGroup.name;
        transform.position = _position;
        m_waterGroup = _waterGroup;
        m_initialSpeed = _speed;
    }

    // Used ONLY for initialization, otherwise use AddForce
    [Server]
    public void initVelocity(Vector3 _velocity)
    {
        m_velocity = _velocity;
        if (float.IsNaN(m_velocity.x))
        {
            Debug.Break();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!NetworkServer.active)
            return;

        transform.position += m_velocity * Time.fixedDeltaTime;
    }

    [ServerCallback]
    void LateUpdate()
    {
        if (!NetworkServer.active)
            return;

        if (m_initTime > 0)
            m_initTime -= Time.deltaTime;

        if (transform.position.y < -10.0f)
            NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider collider)
    {
        if (!NetworkServer.active)
            return;

		//Demo //UPDATE: removals called from health controller now
        /*FakePlayer fakePlayer = collider.GetComponent<FakePlayer>();
        if (fakePlayer)
        {
            fakePlayer.OnMyCollisionEnter(gameObject);
        }*/

        if (m_initTime > 0)
        {
            m_initCollisions.Add(collider.gameObject);
        }

        if (!m_initCollisions.Contains(collider.gameObject) && collider.GetComponent<Drop>() == null
            && collider.GetComponent<WaterDetector>() == null && collider.gameObject.layer != LayerMask.NameToLayer("Reserve"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!NetworkServer.active)
            return;

        if (m_initCollisions.Count > 0 && m_initCollisions.Contains(collider.gameObject))
            m_initCollisions.Remove(collider.gameObject);
    }

    [ServerCallback]
    void OnDestroy()
    {
        if (!NetworkServer.active)
            return;

        if (m_waterGroup)
            m_waterGroup.m_dropPool.Remove(this);
    }

    [Server]
    public void AddForce(Vector3 _force)
    {
        m_velocity += _force;
        if (float.IsNaN(m_velocity.x))
        {
            Debug.Break();
        }
    }

    //private bool test = false;
    [Server]
    public void split(Vector3 splitDirection, int count, float alpha)
    {
        Vector3 x, y, z = m_velocity.normalized;
        if (Vector3.Dot(z, Vector3.right) == m_velocity.magnitude)
            x = Vector3.Cross(z, Vector3.right);
        else x = Vector3.Cross(z, Vector3.up);
        
        y = Vector3.Cross(z, x);

        float initAngle = Random.value * 2 * Mathf.PI;
        /*for (int i = 0; i < count; i++)
        {
            //Drop drop = instance.GetComponent<Drop>();
            float beta = initAngle;
            Vector3 speedDir = x * Mathf.Sin(alpha) * Mathf.Cos(beta) + y * Mathf.Sin(alpha) * Mathf.Sin(beta) + z * Mathf.Cos(alpha);
            Transform newDrop = Instantiate<Transform>(m_dropPrefab);
            newDrop.position = transform.position;
            Drop drop = newDrop.GetComponent<Drop>();
            drop.initVelocity(speedDir * m_velocity.magnitude);
            drop.gameObject.SetActive(false);
            print(speedDir);
            drop.gameObject.SetActive(true);
        }*/
        //Destroy(gameObject);loat beta = initAngle;
        float beta = initAngle;
        Vector3 speedDir = x * Mathf.Sin(alpha) * Mathf.Cos(beta) + y * Mathf.Sin(alpha) * Mathf.Sin(beta) + z * Mathf.Cos(alpha);
        m_velocity = speedDir * velocity.magnitude; 
    }
}

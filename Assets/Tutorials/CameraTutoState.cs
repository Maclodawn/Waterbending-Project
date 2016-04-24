using UnityEngine;
using System.Collections;

public class CameraTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Vector3 m_cameraPosition;

    float m_time = 0;
    public float m_duration = 2;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter CameraTutoState");
        m_ETutoState = ETutoStates.CameraState;
        m_text.text = "Let's start with some warm up. Try to move the camera around by moving the [Mouse].";

        m_cameraPosition = Camera.main.transform.position;

        m_pakkuAnimator.SetBool("Camera", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration && m_cameraPosition != Camera.main.transform.position)
        {
            exit();
        }

        m_cameraPosition = Camera.main.transform.position;
        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.MovementState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameDoubleBarController : MonoBehaviour
{

    //main camera on scene: should be set if we want health bars to face camera
    public bool face_camera = true;
    private Camera main_camera = null;

    //on screen display of health: health_bar contains the whole bar and health_img only the life bar
    public Image health_img = null;
    public Image power_img = null;

    //health component managing health of player
    public HealthComponent health_component;
    public PowerComponent power_component;

    public Text m_name = null;
    private static string[] colors = { "red", "yellow", "blue", "magenta", "orange", "pink", "white" };

    Teamate m_teammate;

    void Start()
    {
        m_teammate = GetComponentInParent<Teamate>();
    }

    void Update()
    {
        if (m_teammate.team_id < 0 || m_teammate.team_id > 6)
            updateText("black");
        else
            updateText(colors[m_teammate.team_id]);
    }

    public void updateText(string _team_color)
    {
        m_name.text = "<b><color=\"" + _team_color + "\">" + transform.parent.gameObject.name + "</color></b>";
    }

    public void OnGUI()
    {
        //
        //updating orientation towards camera
        //
        if (main_camera != null)
        {
            //checking if the three canvas is referenced
            if (gameObject == null)
            {
                Debug.LogError("Health bar object not found.");
                return;
            }

            gameObject.transform.LookAt(new Vector3(main_camera.transform.position.x,
                main_camera.transform.position.y,
                main_camera.transform.position.z));
        }
        else if (face_camera)
            main_camera = GameObject.FindObjectOfType<Camera>();

        //
        //health update
        //
        //checking if components are properly set by user
        if (health_img == null)
        {
            Debug.LogError("Health image component not found.");
            return;
        }

        if (health_component == null)
        {
            Debug.LogError("Health component not found.");
            return;
        }

        //update of health image from health component
        health_img.transform.localScale = new Vector3(health_component.Health / health_component.MaxHealth,
            health_img.transform.localScale.y,
            health_img.transform.localScale.z);

        //
        //power update
        //
        //checking if components are properly set by user
        if (power_img == null)
        {
            Debug.LogError("Power image component not found.");
            return;
        }

        if (power_component == null)
        {
            Debug.LogError("Power component not found.");
            return;
        }

        //update of health image from health component
        power_img.transform.localScale = new Vector3(power_component.Power / power_component.MaxPower,
            power_img.transform.localScale.y,
            power_img.transform.localScale.z);

        //
        //text update
        //
        int team_id = GetComponentInParent<Teamate>().team_id;
        if (team_id < 0 || team_id > 6)
            updateText("black");
        else
            updateText(colors[team_id]);
    }
}

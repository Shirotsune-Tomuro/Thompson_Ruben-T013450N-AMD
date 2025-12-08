using UnityEngine;

public class Gamemode : MonoBehaviour
{
    [SerializeField] TankCharacter m_PlayerTank;
    TankController m_PlayerController;
    Camera m_MainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (m_PlayerTank != null)
            m_PlayerTank.Init();
        else
            Debug.LogError("Gamemode: No Player Tank assigned!");

        if (m_PlayerController == null)
            m_PlayerController = new TankController();
        m_PlayerController.Init(m_PlayerTank);

        if (m_MainCamera != null)
            m_MainCamera.Init();
        else
            Debug.LogError("Gamemode: No Main Camera assigned!");

    }

    // Update is called once per frame
    void Update()
    {

    }
}

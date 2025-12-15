using UnityEngine;

public class Gamemode : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerTank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (m_PlayerTank != null && m_PlayerTank.GetComponent<TankCharacter>())
            m_PlayerTank.GetComponent<TankCharacter>().Init();
        else
            Debug.LogError("Gamemode: No Player Tank assigned!");

        if (m_PlayerTank.GetComponent<TankController>() == null)
            m_PlayerTank.AddComponent<TankController>();
    }
}

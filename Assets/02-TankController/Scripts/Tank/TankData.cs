using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Scriptable Objects/TankData")]
public class TankData : ScriptableObject
{
    public string TankName;

    public float MaxHealth;
    public int Armour;

    public float Acceleration;
    public float MaxSpeed;
}

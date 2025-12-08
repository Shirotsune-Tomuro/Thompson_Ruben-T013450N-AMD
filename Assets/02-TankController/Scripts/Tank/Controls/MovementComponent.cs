using UnityEngine;

public class MovementComponent : MonoBehaviour, IMove
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(float acceleration, float maxSpeed)
    {
        
    }

    public void Accellerate(float value)
    {
        throw new System.NotImplementedException();
    }

    public void Steer(float value)
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }
}

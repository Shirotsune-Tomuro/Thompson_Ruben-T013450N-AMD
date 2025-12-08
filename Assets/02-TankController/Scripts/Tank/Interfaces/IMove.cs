using UnityEngine;

public interface IMove
{
    public void Accellerate(float value);
    public void Stop();
    public void Steer(float value);
}

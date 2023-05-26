using UnityEngine;

public class SimpleTimer
{
    public float duration;
    public float _elapsedTIme;
    public bool Activated = false;
    public bool timerEndedThisFrame = false;
    public int tickAmount = 0;
    private string Name = string.Empty;
    public SimpleTimer(float durationInSeconds)
    {
        this.duration = durationInSeconds;
        _elapsedTIme = 0;
    }

    public SimpleTimer(string name, float durationInSeconds)
    {
        this.Name = name;
        this.duration = durationInSeconds;
    }

    public void AddTime(float deltaTime)
    {
        _elapsedTIme += deltaTime;
    }

    public void Reset()
    {
        this.tickAmount++;
        this._elapsedTIme = 0;
        Debug.LogError($"just ticked for {this.tickAmount}");

    }

    public bool HasTicked => _elapsedTIme > duration;
}
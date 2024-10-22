
using UnityEngine.Events;

public class ScoreEventManager 
{
    public static event UnityAction ScoreIncrement;


    public static void OnScoreIncrement() => ScoreIncrement?.Invoke();
}

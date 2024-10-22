using UnityEngine.Events;

public class BarEventManager 
{
    // Code inspired by - https://www.youtube.com/watch?v=DH2ZxwRBwwg
    public static event UnityAction SliderStart;
    public static event UnityAction SliderReset;

    public static void OnSliderReset() => SliderReset?.Invoke();
}

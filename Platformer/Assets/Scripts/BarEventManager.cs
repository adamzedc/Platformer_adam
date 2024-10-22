using UnityEngine.Events;

public class BarEventManager 
{
    public static event UnityAction SliderReset;

    public static void OnSliderReset() => SliderReset?.Invoke();
}

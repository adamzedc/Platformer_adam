using UnityEngine.Events;

//Manages the health bar
public class BarEventManager 
{
    public static event UnityAction SliderReset;

    public static void OnSliderReset() => SliderReset?.Invoke();
}

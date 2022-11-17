using UnityEngine;
using UnityEngine.UI;


public class FuelBarCMD : MonoBehaviour
{
    public Image fuelbar;
    [SerializeField]Autopilot mv;
    private void Start()
    {
        fuelbar.fillAmount = mv.fuel/Autopilot.maxfuel;
    }
    public void UpdateFuel(float fraction)
    {
        fuelbar.fillAmount = fraction;
    }


}

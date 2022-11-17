using UnityEngine;
using UnityEngine.UI;


public class FuelBar : MonoBehaviour
{
    public Image fuelbar;
    [SerializeField]Movement mv;
    private void Start()
    {
        fuelbar.fillAmount = mv.fuel/Movement.maxfuel;
    }
    public void UpdateFuel(float fraction)
    {
        fuelbar.fillAmount = fraction;
    }


}

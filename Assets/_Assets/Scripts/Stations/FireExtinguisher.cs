using System.Collections;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    private static string _TAG_PLAYER = "Player";
    
    private BaseStation _station;
    private float _timeFireOff = 3.5f;

    private void Start()
    {
        _station = GetComponentInParent<BaseStation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_station.CheckFire()) return;
        if(!other.CompareTag(_TAG_PLAYER)) return;
        if(other.TryGetComponent<PlayerInteraction>(out PlayerInteraction interaction) && !interaction.CheckNullPickUpObj())
        {
            KitchenType pickObj = interaction.GetPickableObj().GetDataPickableObj().typeObj;
            if(pickObj != KitchenType.Porp)return;
            interaction.GetAnimationController().PlayFireExtinguisher(_timeFireOff);
            StartCoroutine(FireOffTime(_timeFireOff));
        }    
    }

    private IEnumerator FireOffTime(float time)
    {
        yield return new WaitForSeconds(time);
        _station.FireOff();
    }    

}

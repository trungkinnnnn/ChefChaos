using UnityEngine;

public interface IStation
{
    BaseStation GetBaseStation();
    StationType GetTypeStation();   
    Transform GetSelectableTransform();
}

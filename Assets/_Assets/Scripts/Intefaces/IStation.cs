using UnityEngine;

public interface IStation
{
    bool IsEmpty();
    BaseStation GetBaseStation();
    StationType GetTypeStation();   
    Transform GetSelectableTransform();
}

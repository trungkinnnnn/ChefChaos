using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public void LockInput(bool value);
    public bool IsMoving();
}

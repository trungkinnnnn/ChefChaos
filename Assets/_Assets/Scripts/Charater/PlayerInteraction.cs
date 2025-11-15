using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(_playerMovement.GetInfoMovement())
        {
            Debug.Log("Movement true");
        }else
        {
            Debug.Log("Movement false");
        }    
    }

}

using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDisabler : MonoBehaviour
{
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GameObject.Find( "Player" ).GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        _playerController.AllowWalking( false );
    }

    void OnDisable()
    {
        _playerController.AllowWalking( true );
    }
}

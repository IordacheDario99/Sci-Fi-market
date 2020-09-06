using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    private Player _player;
    private UIManager _UI;
    private string _message;
    public bool boughtWeapon = false;
    [SerializeField] private GameObject _weaponForsale;
    [SerializeField] private AudioSource _weaponSold;

    
    private void Start()
    {
        _UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }
        if (_UI == null)
        {
            Debug.Log("UIManager is null");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {


            if (_player.coins >= 1 && Input.GetKey(KeyCode.E))
            {
                _UI.RemoveCoin();
                --_player.coins;
                Debug.Log("Coin removed");
                boughtWeapon = true;
                _UI.CleaarInteractionMessage();
                Destroy(_weaponForsale.gameObject);
                _weaponSold.Play();
            }
            else if (_player.coins == 0 && Input.GetKey(KeyCode.E) && boughtWeapon == false)
            {
                _message = "You don't have any coins!";
                _UI.DisplayText(_message);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _message = "Press \"E\" to buy a weapon ";
            _UI.DisplayText(_message);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _UI.CleaarInteractionMessage();
    }


}

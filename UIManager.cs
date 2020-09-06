using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private  Text _ammoText;
    [SerializeField] private Text _interactiveText;
    [SerializeField] private GameObject _coinSprite;
    public void UpdateAmmo(int ammoInClip) 
    {
        _ammoText.text = "Ammo : " + ammoInClip;
    }

    public void InterctionMessage(string trigger) 
    {
        _interactiveText.text = "Press \"E\" to collect " + trigger;
    }
    public void DisplayText(string message)
    {
        _interactiveText.text = message;
    }

    public void CleaarInteractionMessage() 
    {
        _interactiveText.text = " ";
    }

    public void CollectedCoin() 
    {
        _coinSprite.SetActive(true);
    }

    public void RemoveCoin()
    {
        _coinSprite.SetActive(false);
    }
}

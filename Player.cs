using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    private Camera _cam;
    private GameObject _hitMarkerContainer;
    private UIManager _UI;
    private WeaponShop _wpShop;
    [SerializeField] AudioSource _shootingSoundSource;
    [SerializeField] AudioSource _coinPickUpAudioSource;
    [SerializeField] private ParticleSystem _muzzleFash;
    [SerializeField] private ParticleSystem _laserStreaks;
    [SerializeField] private GameObject _coinParticles;
    [SerializeField] private GameObject _hitMarkerPrefab;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private float _speed = 4;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private int _currentAmmo;
    public int coins;
    private float _reloadingTime = 1.5f; 
    private int _maxAmmo = 100;
    private bool _canFire = true;
    private bool _isReloading = false;
    
    
    
    void Start()
    {
    
        Cursor.lockState = CursorLockMode.Locked;

        _characterController = GetComponent<CharacterController>();
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        _wpShop = GameObject.Find("Sharkman").GetComponent<WeaponShop>();

        if (_wpShop == null)
        {
            Debug.Log("WeaponShop is null");
        }
        if (_UI == null)
        {
            Debug.Log("UIManager is null");
        }
        if (_characterController == null) 
        {
            Debug.Log("CharacterController is null");
        }
        
        if(_cam == null)
        {
            Debug.Log("Camera is null");
        }

        _currentAmmo = _maxAmmo;
        _UI.UpdateAmmo(_currentAmmo);
        _muzzleFash.Stop();
        _laserStreaks.Stop();
    }




    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && _isReloading == false)
        {
            Debug.Log("Reloading gun!");
            _isReloading = true;
            StartCoroutine(ReloadGun());
        }

        if (Input.GetKey(KeyCode.Mouse0) && _currentAmmo != 0 && _canFire == true)
        {
            Shoot();   
        }
        else
        {
            _muzzleFash.Stop();
            _laserStreaks.Stop();
            _shootingSoundSource.Stop();
        }
        GetWeapon();
    }

    private void Shoot()
    {

        --_currentAmmo;
        _UI.UpdateAmmo(_currentAmmo);

        if (_shootingSoundSource.isPlaying == false)
        {
            _shootingSoundSource.Play();
        }

        _muzzleFash.Emit(1);
        _laserStreaks.Emit(1);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // I can also use ScreenPointToRay and pass the mouse position on screen as a parameter 
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            Debug.Log("Raycast hit the Gameobject with a collider box named: " + hitInfo.transform.name);
            _hitMarkerContainer = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(_hitMarkerContainer, 5.0f);

            Crate crate = hitInfo.transform.GetComponent<Crate>();
            
            if (crate != null)
            {
                crate.DestroyCrate();
            }

        }
        else
        {
            Debug.Log("I hit nothing");
        }
    }

    private IEnumerator ReloadGun() 
    {
        _canFire = false;
        yield return new WaitForSeconds(_reloadingTime);
        _currentAmmo = _maxAmmo;
        _UI.UpdateAmmo(_currentAmmo);
        _canFire = true;
        _isReloading = false;
    }

    private void Movement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;

        velocity = transform.TransformDirection(velocity);

        velocity.y -= _gravity;
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Collectible")
        {
            _UI.InterctionMessage(other.gameObject.name);
            Debug.Log("In collision");

            if (Input.GetKeyDown(KeyCode.E))
            {
                _coinPickUpAudioSource.Play();
                _coinParticles.SetActive(false);

                Destroy(other.gameObject.GetComponent<MeshRenderer>());
                Destroy(other.gameObject.GetComponent<SphereCollider>());
                Destroy(other.gameObject, 2.0f);

                coins++;
                _UI.CollectedCoin();
                _UI.CleaarInteractionMessage();
            }

        }
    }

    private void GetWeapon()
    {
        if(_wpShop.boughtWeapon == true)
        {
            _weapon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _UI.CleaarInteractionMessage();
    }


}

using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public static WeaponSwitcher Instance { get; private set; }

    public Gun[] weapons;
    public int currentWeaponIndex = 0;
    private Gun currentWeaponScript;
    private PauseMenu pauseMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        if (weapons.Length > 0)
        {
            EquipWeapon(currentWeaponIndex);
        }
    }

    private void Update()
    {
        if (!pauseMenu.isPaused) 
        {
            HandleWeaponSwitchInput();
        }
        
    }

    private void HandleWeaponSwitchInput()
    {
        if (!currentWeaponScript.isReloading) 
        {
            // 1,2,3
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);

            // MouseScroll
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0) SwitchWeapon((currentWeaponIndex + 1) % weapons.Length); // Scroll up
            if (scroll < 0) SwitchWeapon((currentWeaponIndex - 1 + weapons.Length) % weapons.Length); // Scroll down
        }
        
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length && newWeaponIndex != currentWeaponIndex)
        {
            EquipWeapon(newWeaponIndex);
        }
    }

    private void EquipWeapon(int weaponIndex)
    {
        // Disable current weapon
        if (currentWeaponScript != null)
        {
            currentWeaponScript.isReloading = false;
            currentWeaponScript.gameObject.SetActive(false);
        }

        // Enable the selected weapon
        currentWeaponIndex = weaponIndex;
        currentWeaponScript = weapons[currentWeaponIndex];
        if(currentWeaponScript.fireCoroutine != null) {currentWeaponScript.fireCoroutine = null; }
        
        currentWeaponScript.gameObject.SetActive(true);

    }

    private void UpdateWeaponProperties(Gun weapon)
    {
       //kullanmadım sanırım bunu
        switch (currentWeaponIndex)
        {
            case 0: weapon.fireMode = Gun.FireMode.Single; break;
            case 1: weapon.fireMode = Gun.FireMode.Burst; break;
            case 2: weapon.fireMode = Gun.FireMode.Shotgun; break;
        }
    }

    public  Gun GetCurrentWeapon()
    {
        return currentWeaponScript; 
    }
}

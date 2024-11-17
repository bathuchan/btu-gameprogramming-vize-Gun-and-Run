using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public static WeaponSwitcher Instance { get; private set; } // Singleton instance

    public Gun[] weapons; // Array of weapon scripts (Single, Burst, Shotgun)
    public int currentWeaponIndex = 0; // Current weapon index
    private Gun currentWeaponScript; // Active weapon script

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (weapons.Length > 0)
        {
            EquipWeapon(currentWeaponIndex);
        }
    }

    private void Update()
    {
        HandleWeaponSwitchInput();
    }

    private void HandleWeaponSwitchInput()
    {
        // Check for number key input
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);

        // Check for mouse scroll input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) SwitchWeapon((currentWeaponIndex + 1) % weapons.Length); // Scroll up
        if (scroll < 0) SwitchWeapon((currentWeaponIndex - 1 + weapons.Length) % weapons.Length); // Scroll down
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
            currentWeaponScript.gameObject.SetActive(false);
        }

        // Enable the selected weapon
        currentWeaponIndex = weaponIndex;
        currentWeaponScript = weapons[currentWeaponIndex];
        if(currentWeaponScript.fireCoroutine != null) {currentWeaponScript.fireCoroutine = null; }
        
        currentWeaponScript.gameObject.SetActive(true);

        // Update weapon-specific properties, if needed
        //UpdateWeaponProperties(currentWeaponScript);
    }

    private void UpdateWeaponProperties(Gun weapon)
    {
        // Set weapon-specific properties or modes, if needed
        switch (currentWeaponIndex)
        {
            case 0: weapon.fireMode = Gun.FireMode.Single; break;
            case 1: weapon.fireMode = Gun.FireMode.Burst; break;
            case 2: weapon.fireMode = Gun.FireMode.Shotgun; break;
        }
    }

    public  Gun GetCurrentWeapon()
    {
        return currentWeaponScript; // Return the currently equipped weapon
    }
}

![gameLogo](https://img.itch.zone/aW1nLzE4Njk4NTExLnBuZw==/original/syhrON.png)
>This is a demo for a still worked project called "Gun & Run". This project is being developed for ***Bursa Technic University BLM0364 _Oyun Programlama*** midterm by ***Batuhan Şengül*** and ***Aleks Dulda***.

"Gun & Run" is a fast-paced, action-packed shooter game where you must outmaneuver and eliminate enemies while managing your movement and combat tactics. Test your skills against three unique enemy types and prove you’re the fastest gun in town!

---

> **[Play Gun & Run - itch.io](https://bathuchan.itch.io/gun-run)** 

## Current Features
- **Dynamic Movement**: 
  - Smooth walking, running, jumping, and dashing mechanics.
  - Adaptive camera FOV changes based on player actions.
- **Diverse Enemy Types**:
  - ![pistol-enemy](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/pistol-soldier.png) **Pistol-wielding enemies(Red)**: Balanced and precise shooters.
  -  ![shotgun-enemy](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/shotgun_soldier.png) **Shotgun enemies(Green)**: Deadly at close range with wide-spread shots.
  - ![burst-enemy](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/ar-soldier.png)  **Burst rifle enemies(Blue)**: Delivering high damage in controlled bursts.
- **Realistic Gunplay**: 
  - Recoil mechanics, shooting effects, and weapon sway for an immersive experience.
    - **Player's Guns**: 
    -  ![smg](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/smg.png) **SMG**: Rapid firing shots.
    - ![shotgun](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/shotgun_db.png)  **Shotgun**: Deadly at close range with wide-spread shots.
    - ![ar](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Assets/AllFiles/Textures/ar.png)  **Burst rifle**: Delivering high damage in controlled bursts.
- **Field of View (FOV) Adjustments**:
  - FOV dynamically changes when running or dashing for an engaging visual experience.
- **Particle Effects**:
  - Blood, Bullet holes and more gun specfic effects.
## Gameplay Screenshots
![ss1](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Screenshots/ss1.png)
![ss2](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Screenshots/ss2.png)
![ss3](https://github.com/bathuchan/btu-gameprogramming-vize-Gun-and-Run/blob/main/Screenshots/ss3.png)

## Planned Features
- **Power Ups / Special Abilities**
- **More Guns and Gun Mechanics**
- **More Enemy Types**
- **Damage System and Enemy Spawn Controlling**
---

## 🎮 Controls

| Action           | Key/Control        |
|-------------------|--------------------|
| Move             | `W` `A` `S` `D`    |
| Jump             | `Space`            |
| Run              | `Left Shift(Hold)`       |
| Dash             | `Left Ctrl/Right Mouse Button`|
| Shoot            | `Left Mouse Button(Hold)`|
| Reload            | `R`|
| Inspect            | `F`|

---
## Action List(For grading our project)
- **Batuhan Şengül - 20360859008**: 
    - Düşman hareketi EnemyBehavior.cs:51
    - Düşmanın önündeki engeli algılaması EnemyBehavior.cs:61
    - Düşmanın Oyuncuyu fark etmesi/ ona doğru ateş etmeye ve harekete başlaması EnemyBehavior.cs:114
    - Düşmanın Oyuncuyu kaybetmesi/ ona doğru ateş etmeyi ve hareketi durdurması EnemyBehavior.cs:123
    - Oyuncunun silahları arasında değişim yapması (1,2,3 ya da MouseScroll) WeaponSwitcher.cs:34
    - Düşman mermisinin başka nesnelerle teması(trigger) ve oluşturulan parçacık efektleri(kan, mermi deliği) BulletCollision.cs:36
    - Oyuncu mermisinin başka nesnelerle teması(trigger değil) ve oluşturulan parçacık efektleri(kan, mermi deliği) BulletCollision.cs:15
    - Mouse ile kamera kontrolleri(gerekli sınırlandırmalar) PlayerCameraController.cs:55
    - Silah ile ateş etmek PlayerCameraController.cs:74
    - Silah ateş etme geri tepme efekti PlayerCameraController.cs:101
    - Mouse ya da oyuncunun girdisine bağlı silahın yalpalaması WeaponSway.cs:36
    - Ateş edilen merminin fiziği(rastgelelik, rotasyon, hareketi) Gun.cs:190,194,203
        
- **Aleks Dulda - 21360859025**: 
    - Oyuncunun hareketi PlayerMovement.cs:106
    - Koşma tuşu kontrolü (Left Shift) PlayerMovement.cs:72
    - Zemin kontrolü (raycast) PlayerMovement.cs:61
    - Zıplama kontrolü PlayerMovement.cs:64
    - Dash PlayerMovement.cs:111
---

## Sources of assets used in the project
- The sources of all assets used in this project were created by the development team.
  
---

##  Görev Dağılımı

| **Batuhan Şengül - 20360859008** | **Aleks Dulda - 21360859025** |
|-------------------|--------------------|
| Animasyonlar (30x) | Ana Menü (30x) |
| Asenkronizasyon (10x) | Duraklama Menüsü (20x) |
| Ses ve Müzik (20x) | Ses ve Müzik (10x) |

---



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon {
    public string name { get; set; }
    public int damage { get; set; }
    public int magazine { get; set; }
    public int maxMagazineSize { get; set; }
    public float delay { get; set; }
    public int rounds { get; set; }
    public int reloadDelay { get; set; }
    public bool infinite { get; set; }

    public AudioSource sound { get; set; }
}

[System.Serializable]
public class WeaponAudio {
    public string name;
    public AudioSource sound;
}

public class ShootingCtrl : MonoBehaviour {
    public Transform firePoint;
    public GameObject bulletPrefab;
    public WeaponAudio[] audios;
    public AudioSource emptyClip;
    public AudioSource reloading;
    public float force = 20f;
    public Text selectedWeaponText;
    public Text selectedWeaponBulletsText;

    private Dictionary<string, AudioSource> weaponSoundsMap = new Dictionary<string, AudioSource>();
    private float isAbleToShootAgain = 0;
    private Weapon selectedWeapon;
    private Weapon pistol = new Weapon {
        name = "Pistol",
        damage = 3,
        magazine = 15,
        maxMagazineSize = 15,
        delay = 0.5f,
        rounds = 0,
        reloadDelay = 1,
        infinite = true,
    };
    private Weapon machinegun = new Weapon {
        name = "Machine gun",
        damage = 4,
        magazine = 50,
        maxMagazineSize = 50,
        delay = 0.1f,
        rounds = 150,
        reloadDelay = 1,
        infinite = false,
    };
    private List<Weapon> weapons;

    private void Awake() {
        foreach (WeaponAudio audio in audios) {
            Debug.Log(audio.name);
            weaponSoundsMap.Add(audio.name, audio.sound);
        }
        weapons = new List<Weapon> { pistol, machinegun };
        selectedWeapon = pistol;
        UpdateWeaponsUI();
    }

    void Update() {
        // if (Input.GetButtonDown("Fire1")) {
        if (Input.GetKey(KeyCode.Mouse0)) { 
            Shoot();    
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeWeapon(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeWeapon(1);
        }
    }

    void ChangeWeapon(int index) {
        selectedWeapon = weapons[index];
        UpdateWeaponsUI();
    }

    void UpdateWeaponsUI() {
        selectedWeaponText.text = selectedWeapon.name;
        selectedWeaponBulletsText.text = selectedWeapon.magazine + "/" + (selectedWeapon.infinite ? "∞" : selectedWeapon.rounds.ToString());
    }

    void Reload() {
        if (selectedWeapon.infinite == true || selectedWeapon.rounds > 0) {
            int reload = selectedWeapon.rounds >= selectedWeapon.maxMagazineSize
                ? selectedWeapon.maxMagazineSize
                : selectedWeapon.rounds;

            if (selectedWeapon.infinite == true) {
                reload = selectedWeapon.maxMagazineSize;
            } else {
                selectedWeapon.rounds = selectedWeapon.rounds - reload;
            }

            selectedWeapon.magazine = reload;

            isAbleToShootAgain = Time.time + selectedWeapon.reloadDelay;
            reloading.Play();
            UpdateWeaponsUI();
        }
    }

    void Shoot() {
        if (Time.time < isAbleToShootAgain) {
            Debug.Log("Fire cooldown");
            return;
        }

        if (selectedWeapon.magazine == 0) {
            Debug.Log("Out of rounds");

            if (selectedWeapon.rounds > 0 || selectedWeapon.infinite == true) {
                Reload();
            } else {
                if (emptyClip.isPlaying == false) {
                    emptyClip.Play();
                }
            }
            
            return;
        }


        if (weaponSoundsMap.ContainsKey(selectedWeapon.name)) {
            weaponSoundsMap[selectedWeapon.name].Play();
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletCtrl>().SetDamage(selectedWeapon.damage);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * force, ForceMode2D.Impulse);

        selectedWeapon.magazine--;
        
        UpdateWeaponsUI();

        isAbleToShootAgain = Time.time + selectedWeapon.delay;
    }

    public Weapon getSelectedWeapon() {
        return selectedWeapon;
    }
}

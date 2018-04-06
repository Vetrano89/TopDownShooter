using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    // is an invisible object that represents where the gun is on the player
    public Transform weaponHold;
    // Can be set in the player object
    public Gun startingGun;
	// currently equipped gun
	Gun equippedGun;

    private bool HasEquippedWeapon() {
        return (equippedGun != null);
    }

    private void Start() {
        if (startingGun != null) {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip) {
        if (HasEquippedWeapon()) {
            // If a gun is equipped, destroy it before
            // equipping new gun
            Destroy(equippedGun.gameObject);
        }
        // Create new gun with position and rotation of weaponHold
        // (so it rotates with the player)
        // cast "as Gun" because Instantiate returns a variable of type Gun
		equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        // make the equippedGun's parent the weaponHold object
        // lets it moves with the player
        equippedGun.transform.parent = weaponHold;
		
	}

    public void Shoot() {
        if (HasEquippedWeapon()) {
            equippedGun.Shoot();
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    private int initialAmmoCapacity = 5;
    public int ammoCapacity = 5;

    [SerializeField] private int ammoCollectionCooldown = 1;
    private bool canCollectAmmo = true;

    [SerializeField] private Canvas ammoHoverCanvas;

    [SerializeField] private Text ammoHoverText;

    void Start()
    {

        ammoHoverCanvas.transform.rotation = Quaternion.Euler(113.731f, 5.5629f, 0);
        ammoCapacity = 5;

        transform.rotation = Quaternion.Euler(270, 77.56f, 0);

        ammoHoverText.text = ammoCapacity + "/" + initialAmmoCapacity;
    }

    public bool collectAmmo()
    {
        if (canCollectAmmo)
        {
            StartCoroutine(collectAmmoCoroutine());
            return true;
        }

        return false;
    }

    IEnumerator collectAmmoCoroutine()
    {
        canCollectAmmo = false;

        GameManager.singleton.rockCollectingSounds[0].Play();
        ammoCapacity--;

        ammoHoverText.text = ammoCapacity + "/" + initialAmmoCapacity;

        if (ammoCapacity <= 0)
        {
            GameManager.singleton.numberOfAmmoStockpiles--;
            Destroy(this.gameObject);
        }

        //Waits for the given cooldown time before allowing collection again.
        yield return new WaitForSeconds(ammoCollectionCooldown);

        canCollectAmmo = true;
    }

}

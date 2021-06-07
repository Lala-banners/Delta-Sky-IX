using DeltaSky.Controllers;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float fireRate = 0.25f;
    public float weaponRange;
    public float hitForce = 100f;
    public Transform firePoint;
    public Camera weaponCam;
    
    private WaitForSeconds shotDuration = new WaitForSeconds(0.7f);
    private AudioSource gunAudio; //Shooting sound effect
    private float nextFire; //time between shots
    
    // Start is called before the first frame update
    void Start() {
        gunAudio = GetComponent<AudioSource>();
        weaponCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        //Debug.DrawLine(weaponCam.transform.position, weaponCam.transform.forward * 15, Color.blue);
        
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            ShootGun();
        }
    }

    public void ShootGun() {
        Cursor.visible = false;
        
        nextFire = Time.time + fireRate;
        StartCoroutine(ShotSoundEffect());
        Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            //Debug.Log(hit.collider.name);
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                
            if(enemy != null)
                enemy.TakeDamage(damage);
        }
    }

    private IEnumerator ShotSoundEffect() {
        gunAudio.Play();
        yield return shotDuration;
    }
}

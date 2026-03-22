    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(LineRenderer))]
    public class RaycastGun : MonoBehaviour
    {
        public Camera playerCamera;
        public Transform laserOrigin;
        public float gunRange = 50f;
        public float fireRate = 0.2f;
        public float laserDuration = 0.5f;
        private bool isShooting = false;

        public ParticleSystem muzzleFlash;
        public AudioSource gunSound;
        LineRenderer laserLine;
        float fireTimer;
        void Awake()
        {
            laserLine = GetComponent<LineRenderer>();
        }

        public void Shoot()
        {
            if (fireTimer < fireRate) return;
            fireTimer = 0f;

            muzzleFlash.Play();
            gunSound.Play();

            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, 0f)
            );
            laserLine.SetPosition(0, laserOrigin.position);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                if (hit.transform.CompareTag("Enemy"))
                {
                    Destroy(hit.transform.gameObject);
                }

            }
            else
            {
                laserLine.SetPosition(1,
                    rayOrigin + playerCamera.transform.forward * gunRange);
            }
            StartCoroutine(ShootLaser());
        }

        IEnumerator ShootLaser()
        {
            laserLine.enabled = true;
            yield return new WaitForSeconds(laserDuration);
            laserLine.enabled = false;
        }

        void Update()
        {
            fireTimer += Time.deltaTime;

            if (isShooting)
            {
                Shoot();
            }
        }

        public void StartShooting()
        {
            isShooting = true;
        }

        public void StopShooting()
        {
            isShooting = false;
        }
    }

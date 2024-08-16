using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameControllerScript gc;

    public GameObject[] projectiles;

    public GameObject[] AIPoints;

    public float spawnCooldown;

    public float maxObjects;

    public float objects;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (this.gc.BossFight)
        {
            if (this.objects > maxObjects)
            {
                this.objects = maxObjects;
            }
            if (this.spawnCooldown > 0f)
            {
                this.spawnCooldown -= Time.deltaTime;
            }
            if (this.spawnCooldown < 0f)
            {
                if (this.objects < maxObjects)
                {
                    GameObject AIPoint = AIPoints[Random.Range(0, AIPoints.Length)];
                    GameObject projectile = Instantiate(projectiles[Random.Range(0, projectiles.Length)], AIPoint.transform.position, AIPoint.transform.rotation);
                    projectile.transform.position += Vector3.up * 4f;
                    this.objects += 1f;
                }
                this.spawnCooldown = UnityEngine.Random.Range(5f, 25f);
            }
        }
    }
}

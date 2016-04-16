using UnityEngine;
using System.Collections;

public class PlayerSpawnAreas : MonoBehaviour {

    Vector3 offset;
    public GameObject spawnObject1;
    public GameObject spawnObject2;


    //Generate spawn points for both teams. The spawn point of each team should be
    //relative to the spawn point of the other

    public Vector3 generateSpawnPoint(int team)
    {

        Vector3 spawnPoint1;
        Vector3 spawnPoint2;
        float radius = 5f;

        offset.x += Random.Range(-radius, radius);
        offset.y += Random.Range(-radius, radius);
        offset.z += Random.Range(-radius, radius);

        //spawnPoint = Random.insideUnitCircle * 5;
        spawnPoint1.x = spawnObject1.transform.position.x + Random.Range(-1, 1);
        spawnPoint1.y = spawnObject1.transform.position.x + Random.Range(-1, 1);
        spawnPoint1.z = spawnObject1.transform.position.x + Random.Range(-1, 1);


        spawnPoint2.x = spawnObject2.transform.position.x + Random.Range(-1, 1);
        spawnPoint2.y = spawnObject2.transform.position.x + Random.Range(-1, 1);
        spawnPoint2.z = spawnObject2.transform.position.x + Random.Range(-1, 1);

        if (team == 0) {
            return spawnPoint1;
        }
        else {
            return spawnPoint2;
           //return spawnPoint += offset;
        }

    }
}

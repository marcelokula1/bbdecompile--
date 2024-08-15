using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodEventMgr : MonoBehaviour
{
    public AudioSource audioDevice;
    public AILocationSelectorScript Selector;
    public Transform Water;
    public float WaterHeight = 3;
    public Material water;
    public float UpSpeed = 10;
    public float MoveSpeed = 0.5f;
    public WhirlPool whirlPoolPre;
    public int whirlPoolCountMin = 10;
    public int whirlPoolCountMax = 20;
    public List<WhirlPool> whirlpools = new List<WhirlPool>();
    // Update is called once per frame
    void Update()
    {
        water.mainTextureOffset = new Vector2(Time.time, Time.time) * MoveSpeed;
    }
    public void StartEvent()
    {
        audioDevice.Play();
        base.StartCoroutine(SpawnWaterWithWhirlPool());
    }
    public void StopEvent()
    {
        audioDevice.Pause();
        for (int i = 0; i < whirlpools.Count; i++)
        {
            Destroy(whirlpools[i].gameObject);
        }
        whirlpools.Clear();
        base.StartCoroutine(DeSpawnWater());
    }
    private IEnumerator DeSpawnWater()
    {
        while (Water.position.y > -WaterHeight)
        {
            Water.position -= Vector3.up * UpSpeed * Time.deltaTime;
            yield return null;
        }
        yield break;
    }
    private IEnumerator SpawnWaterWithWhirlPool()
    {
        while (Water.position.y < WaterHeight)
        {
            Water.position += Vector3.up * UpSpeed * Time.deltaTime;
            yield return null;
        }
        List<Transform> list = new List<Transform>();
        list.AddRange(Selector.newLocation);
        for (int i = 0; i < Selector.newLocation.Length - UnityEngine.Random.Range(whirlPoolCountMin, whirlPoolCountMax); i++)
        {
            list.RemoveAt(UnityEngine.Random.Range(0, list.Count - 1));
        }
        for (int j = 0; j < list.Count; j++)
        {
            int val = UnityEngine.Random.Range(0, list.Count - 1);
            WhirlPool loadingPool = Instantiate(whirlPoolPre, Selector.newLocation[val].transform.position, Quaternion.identity, base.transform);
            loadingPool.mgr = this;
            whirlpools.Add(loadingPool);
            list.RemoveAt(val);
        }
        yield break;
    }
    public WhirlPool GetRandomWhirlPool(WhirlPool pool)
    {
        List<WhirlPool> list = new List<WhirlPool>();
        list.AddRange(whirlpools);
        list.Remove(pool);
        return list[UnityEngine.Random.Range(0, list.Count - 1)];
    }
}

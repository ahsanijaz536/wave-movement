using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public float curveAmount;

    public GameObject wavePart;

    [SerializeField] private int spawnCount;

    private Vector3 nextWavePartPosition = Vector3.zero;

    [SerializeField] private List<Transform> wavePartsContainer = new List<Transform>();

    [SerializeField] private Transform waveStartPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        AddBodyPart(spawnCount);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 400, 200), "Add Part"))
            AddBodyPart(1);
        if (GUI.Button(new Rect(600, 10, 400, 200), "Restart"))
            SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        MovePartsInWave();
    }
    private void LateUpdate()
    {
        wavePartsContainer[0].transform.localPosition = new Vector3(waveStartPoint.localPosition.x,wavePartsContainer[0].transform.localPosition.y,wavePartsContainer[0].transform.localPosition.z);
    }
    
    public void AddBodyPart(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (wavePartsContainer.Count == 0)
            {
                var wavepart = Instantiate(wavePart, this.transform);
                wavepart.transform.localPosition = nextWavePartPosition;
                nextWavePartPosition = wavepart.transform.localPosition;
                nextWavePartPosition.z += .2f;
                wavepart.AddComponent<WavePointsManager>();
                wavePartsContainer.Add(wavepart.transform);
                wavepart.GetComponent<MeshRenderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);
            }
            else
            {
                WavePointsManager wavePoint = wavePartsContainer[wavePartsContainer.Count - 1].GetComponent<WavePointsManager>();
                wavePoint.ClearWayPointsContainer();
                var wavepart = Instantiate(wavePart, this.transform);
                nextWavePartPosition = wavePoint.wayPointsContainer[0].pointPosition;
                nextWavePartPosition.z += .2f;
                wavepart.transform.localPosition = nextWavePartPosition;
                wavepart.AddComponent<WavePointsManager>();
                wavePartsContainer.Add(wavepart.transform);
                wavepart.GetComponent<WavePointsManager>().ClearWayPointsContainer();
                wavepart.GetComponent<MeshRenderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);
            }
        }
    }

    void MovePartsInWave()
    {
        if (wavePartsContainer.Count > 0)
        {
            for (int i = 1; i < wavePartsContainer.Count; i++)
            {
                 WavePointsManager points = wavePartsContainer[i-1].GetComponent<WavePointsManager>();
                                 
                 Vector3 partPosition = points.wayPointsContainer[0].pointPosition;
                 Quaternion partRotation = points.wayPointsContainer[0].pointRotation;
                
                 wavePartsContainer[i].transform.position = Vector3.Lerp(wavePartsContainer[i].transform.position, new Vector3(partPosition.x, wavePartsContainer[i].transform.position.y, wavePartsContainer[i].transform.position.z), Time.fixedDeltaTime * curveAmount);
                 wavePartsContainer[i].transform.rotation = Quaternion.Euler(wavePartsContainer[i].transform.rotation.x, partRotation.y, wavePartsContainer[i].transform.rotation.z);
                                
                 points.wayPointsContainer.RemoveAt(0);
            }
        }
    }
    
}

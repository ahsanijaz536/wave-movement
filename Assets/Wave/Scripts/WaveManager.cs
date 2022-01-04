using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[HideLabel]
[System.Serializable]
public class Waypoints
{
    public Transform  m_transform;
    [HideInInspector]
    public Vector3    m_position;
    [HideInInspector]
    public Quaternion m_rotation;
    [HideInInspector]
    public float OffsetZ = 0.2f;

    public Waypoints(Transform transform)
    {
        m_transform = transform;
        m_position  = Vector3.zero;
        m_rotation  = Quaternion.identity;
    }//Constructor() end

    public void SetPosRot(Vector3 pos, Quaternion rot) => m_transform.SetPositionAndRotation(pos, rot);

    public void CacheValues()
    {
        m_position = m_transform.position;
        m_rotation = m_transform.rotation;
    }//SetValues() end

}//class end

public enum UpdateMode{Update, FixedUpdate, LateUpdate}

[HideMonoScript]
public class WaveManager : MonoBehaviour
{
    [SerializeField] UpdateMode updateMode;
    [SerializeField] float curveAmount;
    [Space]
    [SerializeField] GameObject wavePart;
    [SerializeField] int spawnCount;
    [SerializeField] List<Waypoints> wavePartsContainer = new List<Waypoints>();

    private Vector3 nextWavePartPosition = Vector3.zero;
    
    void Start() => AddBodyPart(spawnCount);

    private void Update()
    {
        if(updateMode.Equals(UpdateMode.Update))
            MovePartsInWave(Time.deltaTime);
    }//Update() end

    private void FixedUpdate()
    {
        if(updateMode.Equals(UpdateMode.FixedUpdate))
            MovePartsInWave(Time.fixedDeltaTime);
    }//FixedUpdate() end

    private void LateUpdate()
    {
        if(updateMode.Equals(UpdateMode.LateUpdate))
            MovePartsInWave(Time.deltaTime);
    }//LateUpdate() end

    void MovePartsInWave(float DeltaTime)
    {
        if(wavePartsContainer.Count > 0)
        {
            for (int i = 1; i < wavePartsContainer.Count; i++)
            {     
                Vector3 TargetPos = new Vector3(wavePartsContainer[i-1].m_position.x, wavePartsContainer[i-1].m_position.y, wavePartsContainer[i-1].m_transform.position.z + wavePartsContainer[i].OffsetZ);

                wavePartsContainer[i].SetPosRot(Vector3.Lerp(wavePartsContainer[i].m_transform.position, TargetPos, Time.deltaTime * curveAmount), 
                                                Quaternion.Lerp(wavePartsContainer[i].m_transform.rotation, wavePartsContainer[i-1].m_transform.rotation, curveAmount * Time.deltaTime));
                wavePartsContainer[i-1].CacheValues();    
            }//loop end
        }//if end
    }//MovePartsInWave() end

    public void AddBodyPart(int count)
    {
        wavePartsContainer = new List<Waypoints>();
        for (int i = 0; i < count; i++)
        {
            if(i == 0)
            {
                var wavepart = Instantiate(wavePart, this.transform);
                wavepart.transform.localPosition = nextWavePartPosition;
                nextWavePartPosition = wavepart.transform.localPosition;
                nextWavePartPosition.z += .2f;
                wavePartsContainer.Add(new Waypoints(wavepart.transform));
                wavepart.GetComponent<MeshRenderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);
            }//if end
            else
            {
                wavePartsContainer[wavePartsContainer.Count - 1].CacheValues();
                var wavepart = Instantiate(wavePart, this.transform);
                nextWavePartPosition = wavePartsContainer[wavePartsContainer.Count - 1].m_position;
                nextWavePartPosition.z += .2f;
                wavepart.transform.localPosition = nextWavePartPosition;
                wavePartsContainer.Add(new Waypoints(wavepart.transform));
                wavePartsContainer[wavePartsContainer.Count - 1].CacheValues();
                wavepart.GetComponent<MeshRenderer>().material.color =
                    new Color(Random.value, Random.value, Random.value);
            }//else end
        }//loop end
    }//AddBodyPart() end

    [Button("Add New Part")]
    private void AddNewPart()
    {
        wavePartsContainer[wavePartsContainer.Count - 1].CacheValues();
        var wavepart = Instantiate(wavePart, this.transform);
        nextWavePartPosition = wavePartsContainer[wavePartsContainer.Count - 1].m_position;
        nextWavePartPosition.z += .2f;
        wavepart.transform.localPosition = nextWavePartPosition;
        wavePartsContainer.Add(new Waypoints(wavepart.transform));
        wavePartsContainer[wavePartsContainer.Count - 1].CacheValues();
        wavepart.GetComponent<MeshRenderer>().material.color =
            new Color(Random.value, Random.value, Random.value);
    }//AddNewPart() end
    
}//class end
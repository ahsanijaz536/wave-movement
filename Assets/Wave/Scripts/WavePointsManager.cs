using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePointsManager : MonoBehaviour
{
    [System.Serializable]
        public class  Waypoints
        {
            public Vector3 pointPosition;
            public Quaternion pointRotation;

            public Waypoints(Vector3 pos, Quaternion rot)
            {
                pointPosition = pos;
                pointRotation = rot;
            }
        }
    
        public List<Waypoints> wayPointsContainer = new List<Waypoints>();
    
        public void AddWayPointsPerUpdate()
        {
            wayPointsContainer.Add(new Waypoints(transform.position, transform.rotation));
        }
    
        public void ClearWayPointsContainer()
        {
            wayPointsContainer.Clear();
            wayPointsContainer.Add(new Waypoints(transform.position, transform.rotation));
        }
        
        // Update is called once per frame
        void Update()
        {
            AddWayPointsPerUpdate();
        }
}

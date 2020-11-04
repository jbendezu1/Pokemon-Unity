using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private class DoorData {
        public string sceneName;
        public Vector3 entrancePosition;
        public Vector3 exitPosition;

        public DoorData(string sceneName,Vector3 entrancePosition, Vector3 exitPosition)
        {
            this.sceneName = sceneName;
            this.entrancePosition = entrancePosition;
            this.exitPosition = exitPosition;
        }
    }

    private class Doors {
       
    }

    private void f() {
        Doors doors = new Doors();
    }
}

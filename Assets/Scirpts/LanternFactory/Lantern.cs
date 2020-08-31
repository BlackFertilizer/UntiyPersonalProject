    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class Lantern
    {
        int id;
        string headObjectName;
        string endObjectName;
        
        Color haloColor;

        public Lantern(GameObject prefab,Transform parentTran)
        {
            GameObject lanternObject = GameObject.Instantiate(prefab);

            lanternObject.transform.position = parentTran.position;
            lanternObject.AddComponent<LanternScript>().init();
        }


        private void randomType()
        {
            
        }

        public void setHeadObjectName(string name)
        {
            headObjectName = name;
        }

        public void setEndObjectName(string name)
        {
            endObjectName = name;
        }

        public void setHaloColor(Color color)
        {
            haloColor = color;
        }



    }
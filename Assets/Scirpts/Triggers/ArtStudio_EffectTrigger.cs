using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;

namespace Art.Studio
{
    //[RequireComponent(typeof(BoxCollider))]  
     [AddComponentMenu("ArtStudio/SceneTriggle/EffectTriggle")]  
    public class ArtStudio_EffectTrigger : ArtStudio_BaseTrigger
    {
        public List<GameObject> _gameObjectList;

        private bool isPlaying;
        // public delegate void EffectActionHandle();
        // private EffectActionHandle action_handle_;
        // public event EffectActionHandle OnTriggerClicked
        // {
        //     add
        //     {
        //         action_handle_ += value;
        //     }

        //     remove
        //     {
        //         action_handle_ -= value;
        //     }
        // }

        public void init()
        {
            if(_gameObjectList == null)
                _gameObjectList = new List<GameObject>();
        }

        void Start()
        {
            isPlaying = false;
            init();
            stopAllEffect();
        }

        public void addEmptyEffect()
        {
            _gameObjectList.Add(null);
        }

        public void removeEffect(GameObject gameObject)
        {
            _gameObjectList.Remove(gameObject);
        }

        public override void ReceiveRay(Ray pCameraRay)
        {
            //Debug.Log("Effect ReceiveRay");
            base.ReceiveRay(default);
            if(!isPlaying)
            {
                playAllEffect();
            }
            else
            {
                stopAllEffect();
            }
            
        }

        public override void UnReceiveRay()
        {
            //Debug.Log("Effect UnReceiveRay");
            base.UnReceiveRay();
            stopAllEffect();
        }

        private void playAllEffect()
        {
            if(_gameObjectList != null)
            {
                for(int i=0; i< _gameObjectList.Count; i++)
                {
                    if(_gameObjectList[i]!= null)
                    {
                        _gameObjectList[i].SetActive(true);
                    }
                }
            }

            isPlaying = true;
        }

        private void stopAllEffect()
        {
            if(_gameObjectList != null)
            {
                for(int i=0; i< _gameObjectList.Count; i++)
                {
                    if(_gameObjectList[i]!= null)
                    {
                        _gameObjectList[i].SetActive(false);
                    }
                }
            }
            isPlaying = false;
        }

    }
}

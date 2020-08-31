namespace Art.Studio
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    [AddComponentMenu("ArtStudio/SceneTriggle/AudioTriggle")]
    public class ArtStudio_AudioTrigger: ArtStudio_BaseTrigger
    {
        public AudioClip AudioClipRes;
        public bool is3D;
        public GameObject _CurrAudio;
        private AudioSource _CurrAudioResources;
        private Transform _transform;

        public void SetupTransform(Transform pTran)
        {
            
        }

        private void Init()
        {
            if(_CurrAudio==null)
            {
                _transform = transform;
                _CurrAudio = new GameObject("XRStudio_AudioTrigger");
                _CurrAudio.transform.parent = _transform;
                _CurrAudioResources = _CurrAudio.AddComponent<AudioSource>();
            }
            
        }

        private void StopPlay()
        {
            if (_CurrAudioResources != null)
            {
                _CurrAudioResources.Stop();
            }
        }

        private void PlayAudio()
        {
            if (_CurrAudioResources != null)
            {
                _CurrAudioResources.Play();
            }
        }

        public override void ReceiveRay(Ray pCameraRay)
        {
            //Debug.Log("Audio receive Ray");
            base.ReceiveRay(default);
            StopPlay();
            Init();
            if (AudioClipRes != null && _CurrAudioResources != null)
            {
                _CurrAudioResources.clip = AudioClipRes;
            }
            PlayAudio();
        }

        public override void UnReceiveRay()
        {
            //Debug.Log("Audio UnReceiveRay");
            base.UnReceiveRay();
            StopPlay();
        }

        void OnDestroy()
        {
            _CurrAudio = null;
        }

      

        //private void addTrigger()
        //{
        //    if (!GetComponent<EventTrigger>())
        //    {
        //        trigger = gameObject.AddComponent<EventTrigger>();
        //    }
        //    else
        //    {
        //        trigger = gameObject.GetComponent<EventTrigger>();
        //    }

        //    UnityAction<BaseEventData> click = new UnityAction<BaseEventData>(playAudio);

        //    EventTrigger.Entry myclick = new EventTrigger.Entry();
        //    myclick.eventID = EventTriggerType.PointerClick;
        //    myclick.callback.AddListener(click);
        //    trigger.triggers.Add(myclick);



        //    UnityAction<BaseEventData> click1 = new UnityAction<BaseEventData>(stopAudio);
        //    EventTrigger.Entry myclick1 = new EventTrigger.Entry();
        //    myclick1.eventID = EventTriggerType.Deselect;
        //    myclick1.callback.AddListener(click1);
        //    trigger.triggers.Add(myclick1);
        //}


        //private void playAudio(BaseEventData eventData)
        //{
        //    Debug.Log("Clicked");
        //    audioSource.Play();
        //}


        //private void stopAudio(BaseEventData eventData)
        //{
        //    Debug.Log("stopAudio");
        //    if (audioSource.isPlaying)
        //    {
        //        audioSource.Stop();
        //    }
        //}


    }
}
  


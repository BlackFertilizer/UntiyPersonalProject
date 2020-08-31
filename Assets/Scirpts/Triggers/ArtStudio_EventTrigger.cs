namespace Art.Studio
{
    using UnityEngine;

    [AddComponentMenu("ArtStudio/SceneTriggle/eventTriggle")]
    public class ArtStudio_EventTrigger : ArtStudio_BaseTrigger
    {
        public override void ReceiveRay(Ray pCameraRay)
        {
          base.ReceiveRay(default);
        }

        public override void UnReceiveRay()
        {
            base.UnReceiveRay();
        } 
    }
}

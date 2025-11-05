using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class HapticFeedbackCustom : MonoBehaviour
{
    [Range(0, 1)]
    public float intensity;
    public float duration;
    [SerializeField] XRBaseController _RightController;
    [SerializeField] XRBaseController _leftController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SendHaptic(XRBaseController controller, float intensity, float duration)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(intensity, duration);
        }

    }
    public void LeftControllerVibration(float intensity, float duration)
    {
        SendHaptic(_leftController, intensity, duration);   
    }

    public void RightControllerVibration(float intensity, float duration)
    {
        SendHaptic(_RightController, intensity, duration);
    }


    public void Vibrate()
    {
        LeftControllerVibration(intensity, duration);
        RightControllerVibration(intensity, duration);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CustomController : MonoBehaviour
{
    public InputDeviceCharacteristics characteristics;
    [SerializeField]
    private List<GameObject> controllerModels;
    private GameObject controllerInstance;
    private InputDevice availableDevice;

    public bool renderController;
    public GameObject handModel;
    private GameObject handInstance;

    private Animator handModelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!availableDevice.isValid)
        {
            TryInitialize();
            return;
        }

        if(renderController)
        {
            handInstance.SetActive(false);
            controllerInstance.SetActive(true);
        }
        else
        {
            handInstance.SetActive(true);
            controllerInstance.SetActive(false);
            UpdateHandAnimation();
        }

    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        foreach(var device in devices)
        {
            Debug.Log($"Aailable Device Name : {device.name}, Characteristic: { device.characteristics}");
            Debug.Log(devices.Count);
        }
        if(devices.Count > 0)
        {
            availableDevice = devices[0];
            string name = "";
            if("Oculs Touch COntroller - Left" == availableDevice.name)
            {
                name = "Oculus Quest Controller - Left";
            }
            else if ("Oculus Touch Controller - Right" == availableDevice.name)
            {
                name = "Oculus Quest Controller - Right";
            }

            GameObject currentControllerModel = controllerModels.Find(controller => controller.name == name);
 
            if (availableDevice.name.Contains("Left"))
            {
                currentControllerModel = controllerModels[1];
            }
            else if (availableDevice.name.Contains("Right"))
            {
                currentControllerModel = controllerModels[2];
            }
            else
            {
                currentControllerModel = null;
            }

            if (currentControllerModel)
            {
                controllerInstance = Instantiate(currentControllerModel, transform);
            }
            else
            {
                Debug.LogError("Didn't get suitable controller model");
                controllerInstance = Instantiate(controllerModels[0], transform);
            }

            handInstance = Instantiate(handModel, transform);
            handModelAnimator = handInstance.GetComponent<Animator>();

        }
    }

    void UpdateHandAnimation()
    {
        if(availableDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handModelAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handModelAnimator.SetFloat("Trigger", 0);
        }

        if(availableDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handModelAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handModelAnimator.SetFloat("Grip", 0);
        }
    }
}

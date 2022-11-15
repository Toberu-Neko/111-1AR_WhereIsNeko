using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackerManager : MonoBehaviour
{
    [Header("The length of this list must match the number of images in Reference Image Library")]
    [SerializeField]
    private List<GameObject> ObjectsToPlace;
    private int refImageCount;
    private Dictionary<string, GameObject> allObjects;

    //create the “trackable” manager to detect 2D images
    private ARTrackedImageManager arTrackedImageManager;
    private IReferenceImageLibrary refLibrary;

    void Awake()
    {
        //initialized tracked image manager  
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }


    //when the tracked image manager is enabled add binding to the tracked 
    //image changed event handler by calling a method to iterate through 
    //image reference’s changes 
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    //when the tracked image manager is disabled remove binding to the 
    //tracked image changed event handler by calling a method to iterate 
    //through image reference’s changes
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void Start()
    {
        refLibrary = arTrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectsToPlace();
        LoadObjectDictionary();
    }

    void LoadObjectsToPlace()
    {
        ImageTargetController[] components = GameObject.FindObjectsOfType<ImageTargetController>();
 
        foreach (ImageTargetController comp in components)
        {
            ObjectsToPlace.Add(comp.gameObject);
        }
    }

    void LoadObjectDictionary()
    {
        allObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < refImageCount; i++)
        {
            GameObject newOverlay = new GameObject();
            for(int j=0;j< ObjectsToPlace.Count;j++)
            {
                if(refLibrary[i].name == ObjectsToPlace[j].GetComponent<ImageTargetController>().trackImageName)
                {
                    newOverlay = ObjectsToPlace[j];
                    allObjects.Add(refLibrary[i].name, newOverlay);
                    newOverlay.SetActive(false);
                }
            }
        }
    }


    void ActivateTrackedObject(string imageName)
    {
        Debug.Log("Tracked the target: " + imageName);
        allObjects[imageName].SetActive(true);
    }

    private void UpdateTrackedObject(ARTrackedImage trackedImage)
    {
        //if tracked image tracking state is comparable to tracking
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            //set the image tracked ar object to active 
            allObjects[trackedImage.referenceImage.name].SetActive(true);
            allObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            allObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        }
        else //if tracked image tracking state is limited or none 
        {
            //deactivate the image tracked ar object 
            allObjects[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        // for each tracked image that has been added
        foreach (var addedImage in args.added)
        {
            ActivateTrackedObject(addedImage.referenceImage.name);

        }

        // for each tracked image that has been updated
        foreach (var updated in args.updated)
        {
            //throw tracked image to check tracking state
            UpdateTrackedObject(updated);
        }

        // for each tracked image that has been removed  
        foreach (var trackedImage in args.removed)
        {
            // destroy the AR object associated with the tracked image
            Destroy(trackedImage.gameObject);
        }
    }

}

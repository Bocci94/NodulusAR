using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class AR_Moove : MonoBehaviour
{
    [SerializeField] private GameObject _prefabGameObject; // mi permette di utilizzare il prefab che ho creat

    private GameObject _spawnedGameObject;

    private ARRaycastManager _arRaycastManager;

    private Vector2 _touchPosition;

    private static List<ARRaycastHit> _hits;
    

    private void Awake()
    {
        _hits = new List<ARRaycastHit>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    /*bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        var tapRecognizer = new TKTapRecognizer();
        tapRecognizer.gestureRecognizedEvent +=OnTouch;
        TouchKit.addGestureRecognizer(tapRecognizer);     
    }

    private void OnTouch(TKTapRecognizer recognizer)
    {
        //**** VALUTARE SE TOGLIERE IL CAST A VECTOR 2
        Vector2 touchPosition = (Vector2) Camera.main.ScreenToViewportPoint(recognizer.touchLocation()) - Vector2.one / 2f;

        /* VALUTARE SE SERVE
         const float maxTouchDistance = 0.4f;
        if (touch_position.magnitude > maxTouchDistance) {
            return;
        }
        
        var velocity = (_panVelocity + _magnetVelocity).magnitude;
			
		// TODO: make configurable
		const float threshold = 0.20f;
		
		if (velocity > threshold) {
			return;
		}
		*/
        
        if (_arRaycastManager.Raycast(touchPosition,_hits,TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;
            if (_spawnedGameObject is null)
            {
                _spawnedGameObject = Instantiate(_prefabGameObject, hitPose.position, hitPose.rotation);
            }else
            {
                _spawnedGameObject.transform.position = hitPose.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}

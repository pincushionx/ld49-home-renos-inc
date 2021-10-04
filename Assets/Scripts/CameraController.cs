using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD49
{
    public class CameraController : MonoBehaviour
    {
        public SceneController scene;
        public GameObject globalCameraPosition;
        public GameObject globalCameraContainer;

        public bool moving = false;

        /*public void MoveCameraToRoom(RoomController room)
        {
            if (transform.parent != globalCameraContainer.transform)
            {
                MoveCamera(globalCameraContainer);
            }

            StartCoroutine(MoveCameraToRoomCoroutine(room.transform.position));
        }*/
        public void MoveCamera(GameObject cameraContainer)
        {
            if (!moving)
            {
                transform.parent = cameraContainer.transform;
                transform.localScale = Vector3.one;

                StartCoroutine(MoveCameraCoroutine());
            }
        }

        IEnumerator MoveCameraCoroutine()
        {
            moving = true;

            float moveSpeed = .05f; // seconds per unit distance
            float progress = 0;

            Vector3 initialPosition = transform.localPosition;
            Quaternion initialRotation = transform.localRotation;

            Vector3 desiredPosition = Vector3.zero;
            Quaternion desiredRotation = Quaternion.identity;

            float distance = (desiredPosition - initialPosition).magnitude;

            while (progress < 1)
            {
                yield return null;

                float frameProgress = Time.deltaTime / moveSpeed / distance;
                progress += frameProgress;

                transform.localPosition = Vector3.Slerp(initialPosition, desiredPosition, progress);
                transform.localRotation = Quaternion.Slerp(initialRotation, desiredRotation, progress);
            }

            moving = false;
        }

        IEnumerator MoveCameraToRoomCoroutine(Vector3 position)
        {
            float moveSpeed = .05f; // seconds per unit distance
            float progress = 0;

            Vector3 initialPosition = globalCameraPosition.transform.position;
            //Quaternion initialRotation = globalCameraPosition.transform.localRotation;

            Vector3 desiredPosition = position;
            //Quaternion desiredRotation = Quaternion.identity;

            float distance = (desiredPosition - initialPosition).magnitude;

            while (progress < 1)
            {
                yield return null;

                float frameProgress = Time.deltaTime / moveSpeed / distance;
                progress += frameProgress;

                globalCameraPosition.transform.localPosition = Vector3.Slerp(initialPosition, desiredPosition, progress);
                //globalCameraPosition.transform.localRotation = Quaternion.Slerp(initialRotation, desiredRotation, progress);
            }
        }
    }
}
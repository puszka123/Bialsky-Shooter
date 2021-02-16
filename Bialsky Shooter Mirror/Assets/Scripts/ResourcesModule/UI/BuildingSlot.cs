using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BialskyShooter.ResourcesModule.UI
{
    public class BuildingSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static event Action<Guid, Vector3> clientOnBuildingPreviewEnd;
        [SerializeField] LayerMask layerMask = new LayerMask();
        [SerializeField] Image image;
        BuildingPreview buildingPreview;
        GameObject buildingPreviewInstance;


        public void SetBuildingSlot(BuildingPreview buildingPreview)
        {
            this.buildingPreview = buildingPreview;
            Sprite icon = Resources.Load<Sprite>(buildingPreview.iconPath);
            SetBuildingSlotIcon(icon);
        }

        void SetBuildingSlotIcon(Sprite icon)
        {
            image.color = new Color(1,1,1,1);
            image.sprite = icon;
        }

        void ClearBuildingSlotIcon()
        {
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (buildingPreview == null) return;
            var prefab = (GameObject)Resources.Load(buildingPreview.buildingPreviewPrefabPath);
            buildingPreviewInstance = Instantiate(prefab);
            buildingPreviewInstance.transform.position = MousePositionToWorldPosition();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (buildingPreviewInstance == null) return;
            buildingPreviewInstance.transform.position = MousePositionToWorldPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (buildingPreviewInstance == null) return;
            clientOnBuildingPreviewEnd?.Invoke(buildingPreview.buildingId, buildingPreviewInstance.transform.position);
            Destroy(buildingPreviewInstance);
        }

        Vector3 MousePositionToWorldPosition()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return Vector3.zero;
            return hit.point;
        }
    }
}

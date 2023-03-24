using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StarForce
{
    public class CursorManager : IReference
    {
        private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
        private bool canClick;

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            canClick = ObjectAtMousePosition();

            if (canClick && Input.GetMouseButtonDown(0))
            {
                ClickAction(ObjectAtMousePosition().gameObject);
            }
        }

        private void ClickAction(GameObject clickObject)
        {
            switch (clickObject.tag)
            {
                case Constant.Tag.Teleport:
                    var teleport = clickObject.GetComponent<Teleport>();
                    teleport?.TeleportToScene();
                    break;
                case Constant.Tag.Work:
                    var work = clickObject.GetComponent<Work>();
                    work?.StarWork();
                    break;
                default:
                    break;
            }
        }

        private Collider2D ObjectAtMousePosition()
        {
            // 点击UI屏蔽场景内物品点击
            if (EventSystem.current.IsPointerOverGameObject())
                return null;

            return Physics2D.OverlapPoint(mouseWorldPos, Constant.Layer.GameLayerId);
        }

        public CursorManager()
        {

        }

        public static CursorManager Create()
        {
            CursorManager cursor = ReferencePool.Acquire<CursorManager>();
            return cursor;
        }

        public void Clear()
        {

        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		Vector3 m_CurrentPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void Start()
		{
			//m_StartPos.x = Screen.width * (GetComponent<RectTransform>().anchorMin.x + GetComponent<RectTransform>().anchorMax.x)/2;
			//m_StartPos.y = Screen.height * (GetComponent<RectTransform>().anchorMin.y + GetComponent<RectTransform>().anchorMax.y)/2;
			m_StartPos = GetComponent<RectTransform>().anchoredPosition;
			m_CurrentPos = m_StartPos;
			GetComponent<Image>().transform.rotation = Quaternion.AngleAxis((float)90.0, Vector3.forward);
			//Debug.Log(GetComponent<RectTransform>().anchoredPosition);
			CreateVirtualAxes();
		}

		Vector3 getStartPos(){
			//return new Vector3(Screen.width * (GetComponent<RectTransform>().anchorMin.x + GetComponent<RectTransform>().anchorMax.x)/2,
			//	Screen.height * (GetComponent<RectTransform>().anchorMin.y + GetComponent<RectTransform>().anchorMax.y)/2,
			//	(float)0);
			return new Vector3 (GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y, (float)0);
		}

		//float actualScreenPos

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
			//Debug.Log(GetComponent<RectTransform>().anchoredPosition);
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			//Vector3 newPos = Vector3.zero;
			Vector3 anchorPoint = (GetComponent<RectTransform>().anchorMax + GetComponent<RectTransform>().anchorMin) / 2;
			anchorPoint.x = anchorPoint.x * Screen.width;
			anchorPoint.y = anchorPoint.y * Screen.height;
			Vector3 newPos = anchorPoint;
			if (m_UseX)
			{
				newPos.x = (data.position.x - m_StartPos.x - anchorPoint.x);
			}

			if (m_UseY)
			{
				newPos.y = (data.position.y - m_StartPos.y - anchorPoint.y);
			}
			GetComponent<RectTransform>().anchoredPosition = Vector3.ClampMagnitude (new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
			float angle = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
			GetComponent<Image>().transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			//GetComponent<RectTransform>().anchoredPosition = new Vector3(data.position.x, data.position.y, newPos.z);
			UpdateVirtualAxes(Vector3.ClampMagnitude (new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos);
		}


		public void OnPointerUp(PointerEventData data)
		{
			GetComponent<RectTransform>().anchoredPosition = m_StartPos;
			GetComponent<Image>().transform.rotation = Quaternion.AngleAxis((float)90.0, Vector3.forward);
			UpdateVirtualAxes(m_StartPos);
		}


		public void OnPointerDown(PointerEventData data) { }

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SMES : InputTrigger, INodeControl
{
	//public override bool isOnForInterface { get; set; } = false;
	//public override bool PassChangeToOff { get; set; } = false;
	public bool ResistanceChange = false;

	[SyncVar]
	public int currentCharge; // 0 - 100

	//Sprites:
	public Sprite offlineSprite;
	public Sprite onlineSprite;
	public Sprite[] chargeIndicatorSprites;
	public Sprite statusCriticalSprite;
	public Sprite statusSupplySprite;

	//Renderers:
	public SpriteRenderer statusIndicator;
	public SpriteRenderer OnOffIndicator;
	public SpriteRenderer chargeIndicator;

	public ElectricalNodeControl ElectricalNodeControl;
	public BatterySupplyingModule BatterySupplyingModule;

	[SyncVar(hook = "UpdateState")]
	public bool isOn = false;

	public override void OnStartClient()
	{
		base.OnStartClient();
		UpdateState(isOn);
	}

	public override bool Interact(GameObject originator, Vector3 position, string hand)
	{
		if (!isServer)
		{
			InteractMessage.Send(gameObject, hand);
		}
		else
		{
			isOn = !isOn;
			UpdateServerState(isOn);
		}
		//ConstructionInteraction(originator, position, hand);
		return true;
	}

	public void UpdateServerState(bool _isOn)
	{
		if (isOn)
		{
			ElectricalNodeControl.TurnOnSupply();
		}
		else
		{
			ElectricalNodeControl.TurnOffSupply();
		}
	}

	public void PowerNetworkUpdate() { }


	public void UpdateState(bool _isOn)
	{
		isOn = _isOn;
		if (isOn)
		{
			OnOffIndicator.sprite = onlineSprite;
			chargeIndicator.gameObject.SetActive(true);
			statusIndicator.gameObject.SetActive(true);
			int chargeIndex = (currentCharge / 100) * 4;
			chargeIndicator.sprite = chargeIndicatorSprites[chargeIndex];
			if (chargeIndex == 0)
			{
				statusIndicator.sprite = statusCriticalSprite;
			}
			else
			{
				statusIndicator.sprite = statusSupplySprite;
			}
		}
		else
		{
			OnOffIndicator.sprite = offlineSprite;
			chargeIndicator.gameObject.SetActive(false);
			statusIndicator.gameObject.SetActive(false);
		}
	}

	//[ContextMethod("Toggle Charge", "Power_Button")]
	//public void ToggleCharge()
	//{
	//	//ToggleCanCharge = !ToggleCanCharge;
	//}

	//[ContextMethod("Toggle Support", "Power_Button")]
	//public void ToggleSupport()
	//{
	//	//ToggleCansupport = !ToggleCansupport;
	//}

}
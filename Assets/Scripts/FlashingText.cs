using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashingText : MonoBehaviour 
{
	[SerializeField] private Text textComponent;
	private Shadow shadowComponent;
	private float defaultShadowOpacity;

	[SerializeField] private float minOpacity = 0f;
	[SerializeField] private float maxOpacity = 1f;
	[SerializeField] private float flashPerSecond = 1f;
	private float time = 0f;

	[SerializeField] private bool fadeInOnEnable = true;

	void OnEnable()
	{
		if (textComponent != null)
		{
			if (fadeInOnEnable)
				StartCoroutine(FadeIn());
			else
				StartCoroutine(Flash());
		}
	}

	void Awake()
	{
		if (this.GetComponent<Shadow>() != null)
		{
			shadowComponent = this.GetComponent<Shadow>();
			defaultShadowOpacity = shadowComponent.effectColor.a;
			shadowComponent.useGraphicAlpha = true;
		}
	}

	IEnumerator FadeIn()
	{
		time = 0f;

		textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0f);

		if (shadowComponent != null)
			shadowComponent.effectColor = new Color(shadowComponent.effectColor.r, shadowComponent.effectColor.g, shadowComponent.effectColor.b, 0f);

		while (textComponent.color.a < minOpacity)
		{
			time += Time.unscaledDeltaTime;
			float opacity = Mathf.Clamp(((maxOpacity - minOpacity) * (flashPerSecond * 2f)) * time, 0f, maxOpacity);
			textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, opacity);

			if (shadowComponent != null)
				shadowComponent.effectColor = new Color(shadowComponent.effectColor.r, shadowComponent.effectColor.g, shadowComponent.effectColor.b, defaultShadowOpacity * (opacity + (1f - maxOpacity)));

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(Flash());
	}

	IEnumerator Flash()
	{
		time = 0f;

		textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, maxOpacity);

		if (shadowComponent != null)
			shadowComponent.effectColor = new Color(shadowComponent.effectColor.r, shadowComponent.effectColor.g, shadowComponent.effectColor.b, defaultShadowOpacity);

		while (true)
		{
			time += Time.unscaledDeltaTime;
			float opacity = minOpacity + Mathf.PingPong(((maxOpacity - minOpacity) * (flashPerSecond * 2f)) * time, maxOpacity - minOpacity);
			textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, opacity);

			if (shadowComponent != null)
				shadowComponent.effectColor = new Color(shadowComponent.effectColor.r, shadowComponent.effectColor.g, shadowComponent.effectColor.b, defaultShadowOpacity * (opacity + (1f - maxOpacity)));
			
			yield return new WaitForEndOfFrame();
		}
	}
}

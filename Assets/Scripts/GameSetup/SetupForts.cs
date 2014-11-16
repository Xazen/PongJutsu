using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class SetupForts : MonoBehaviour
	{
	
		public GameObject fortPrefab;

		public int numberOfForts = 5;
		public float width = 1f;
		public float offset = 1f;

		public bool autoFlip = true;


		void Awake()
		{
			float offsetX = GetComponent<SetupStage>().width - offset;
			float offsetY = GetComponent<SetupStage>().height;

			GameObject Container = new GameObject("Forts");

			// Forts Player 1
			for (int i = 0; i < numberOfForts; i++)
			{
				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(-offsetX - -width / 2, size.y * i - offsetY + size.y / 2);
				spawnFort(position, size, Container, "FortLeft", "FortLeft");
			}

			// Forts Player 2
			for (int i = 0; i < numberOfForts; i++)
			{
				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(offsetX - width / 2, size.y * i - offsetY + size.y / 2);
				spawnFort(position, size, Container, "FortRight", "FortRight").GetComponent<Fort>().flip = autoFlip;
			}
		}

		private GameObject spawnFort(Vector2 position, Vector2 size, GameObject parent, string name, string tag)
		{
			GameObject instance = (GameObject)Instantiate(fortPrefab);
			instance.name = name;
			instance.tag = tag;
			instance.transform.parent = parent.transform;
			instance.GetComponent<BoxCollider2D>().size = size;
			instance.transform.position = position;

			return instance;
		}

		void OnDrawGizmos()
		{
			float offsetX = GetComponent<SetupStage>().width - offset;
			float offsetY = GetComponent<SetupStage>().height;

			for (int i = 0; i < numberOfForts; i++)
			{
				if (i % 2 == 0)
					Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
				else
					Gizmos.color = new Color(0.15f, 0.15f, 0.8f, 0.3f);

				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(-offsetX - -width / 2, size.y * i - offsetY + size.y / 2);
				Gizmos.DrawCube(position, size);
			}

			for (int i = 0; i < numberOfForts; i++)
			{
				if (i % 2 == 0)
					Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
				else
					Gizmos.color = new Color(0.15f, 0.15f, 0.8f, 0.3f);

				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(offsetX - width / 2, size.y * i - offsetY + size.y / 2);
				Gizmos.DrawCube(position, size);
			}
		}
	}
}

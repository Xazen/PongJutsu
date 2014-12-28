using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class SetupBoundaries : GameSetup
	{

		public GameObject boundaryPrefab;

		private float boundaryThickness = 1;


		public override void run()
		{
			base.run();

			float width = GetComponent<SetupStage>().width;
			float height = GetComponent<SetupStage>().height;

			GameObject Container = new GameObject("Boundaries");

			// Boundary Top
			spawnBoundary(new Vector2(0, height + boundaryThickness / 2), new Vector2(width * 2, boundaryThickness), Container, "BoundaryTop", "BoundaryTop");

			// Boundary Bottom
			spawnBoundary(new Vector2(0, -height - boundaryThickness / 2), new Vector2(width * 2, boundaryThickness), Container, "BoundaryBottom", "BoundaryBottom");

			// Boundary Left
			spawnBoundary(new Vector2(-width - boundaryThickness / 2, 0), new Vector2(boundaryThickness, height * 2), Container, "BoundaryLeft", "BoundaryLeft");

			// Boundary Right
			spawnBoundary(new Vector2(width + boundaryThickness / 2, 0), new Vector2(boundaryThickness, height * 2), Container, "BoundaryRight", "BoundaryRight");
		}

		private GameObject spawnBoundary(Vector2 position, Vector2 size, GameObject parent, string name, string tag)
		{
			GameObject instance = (GameObject)Instantiate(boundaryPrefab);
			instance.name = name;
			instance.tag = tag;
			instance.transform.parent = parent.transform;
			instance.GetComponent<BoxCollider2D>().size = size;
			instance.transform.position = position;

			return instance;
		}

		void OnDrawGizmos()
		{
			float width = GetComponent<SetupStage>().width;
			float height = GetComponent<SetupStage>().height;

			Gizmos.color = new Color(0.35f, 0.1f, 0.35f, 0.4f);

			Gizmos.DrawCube(new Vector2(0, height + boundaryThickness / 2), new Vector2(width * 2, boundaryThickness));
			Gizmos.DrawCube(new Vector2(0, -height - boundaryThickness / 2), new Vector2(width * 2, boundaryThickness));
			Gizmos.DrawCube(new Vector2(-width - boundaryThickness / 2, 0), new Vector2(boundaryThickness, height * 2));
			Gizmos.DrawCube(new Vector2(width + boundaryThickness / 2, 0), new Vector2(boundaryThickness, height * 2));
		}
	}
}

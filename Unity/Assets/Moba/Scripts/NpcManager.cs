using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{

	public Transform[] spawnPoints;
	public GameObject[] npcPrefabs;
	public Transform[] waypoints;
	
	private float timer = 15f;
	private const int npcSpawnTime = 30;
	
	// Use this for initialization
	void Start () {
		SpawnNpcs();
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*timer += Time.deltaTime;

		if (timer >= npcSpawnTime)
		{
			SpawnNpcs();
		}*/
	}

	private void SpawnNpcs()
	{
		int direction = 1;
		
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (i == spawnPoints.Length / 2)
			{
				direction = -1;
			}

			float zAxis = 1.5f;
			
			NpcGroup group = new NpcGroup(direction == 1 ? 0 : waypoints.Length - 1, direction);

			for (int j = 0; j < 5; j++)
			{
				Vector3 spawnPos = spawnPoints[i].position;

				if (j < 3)
				{
					spawnPos.x += direction;
					spawnPos.z += zAxis;
					GameObject npc = Instantiate(npcPrefabs[0]);
					npc.transform.position = spawnPos;
					zAxis -= 1.5f;
					if (j == 2)
						zAxis = 1;

					group.AddNpc(new MeleeNpcBehaviour(npc));
				}
				else
				{
					spawnPos.x += direction * -1;
					spawnPos.z += zAxis;
					GameObject npc = Instantiate(npcPrefabs[1]);
					npc.transform.position = spawnPos;
					zAxis -= 2;
				}
			}
		}
	}

	private class NpcGroup
	{
		private List<AbstractNpcBehaviour> npcList;
		private int currentWaypoint;
		private int waypointModifier;

		public int CurrentWaypoint {
			get
			{
				return currentWaypoint;
			}
		}

		public NpcGroup(int currentWaypoint, int waypointModifier)
		{
			this.currentWaypoint = currentWaypoint;
			this.waypointModifier = waypointModifier;
			npcList = new List<AbstractNpcBehaviour>();
		}

		public void AddNpc(AbstractNpcBehaviour npc)
		{
			npcList.Add(npc);
		}

		public void StopNavigation() {
			for (int i = 0; i < npcList.Count; i++) {
				npcList[i].Stop();
			}
		}

		public void SetNextWaypoint(Vector3 pos)
		{
			for (int i = 0; i < npcList.Count; i++) {
				npcList[i].Move(pos);
			}
		}
	}
}

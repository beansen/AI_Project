
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractNpcBehaviour
{
	protected float range;
	protected float timer;
	protected float attackTime;
	protected GameObject npcGameObject;
	protected Animator npcAnimator;
	protected NavMeshAgent navAgent;

	public AbstractNpcBehaviour(GameObject npcGameObject)
	{
		this.npcGameObject = npcGameObject;
		this.npcAnimator = npcGameObject.GetComponent<Animator>();
		navAgent = npcGameObject.GetComponent<NavMeshAgent>();
	}
	
	public abstract void Attack();

	public bool CanAttack()
	{
		return timer >= attackTime;
	}

	public void Update()
	{
		timer += Time.deltaTime;
	}
	
	public void Reset()
	{
		timer = 0;
	}
}
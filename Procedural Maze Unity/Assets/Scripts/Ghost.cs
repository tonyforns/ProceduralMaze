using System;
using UnityEngine;

public class Ghost : Enemy
{
    private Transform character;
    [SerializeField] private float speed = 5;
    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        Wander();
    }
    private void Spawn()
    {
        transform.position =  Dungeon.Instance.GetRandomRoomWorldPosition();
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }

    private void Wander()
    {
        if(IsCharacterOnSight())
        {
            ChaseCharacter();
        }

        if(IsObstacleClose())
        {
            Spawn();
            return;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }


    private void ChaseCharacter()
    {
        transform.LookAt(new Vector3(character.position.x, 0 , character.position.z));
    }
    private bool IsCharacterOnSight()
    {
        Collider[] hitList = Physics.OverlapSphere(transform.position + Vector3.up, 15f, DungeonSettings.CharacterMask.value);
        if (hitList.Length > 0)
        {
            
            Transform testCharacter = hitList[0].gameObject.transform;
            if(Vector3.Angle(testCharacter.position - transform.position, transform.forward) < 45)
            {

                if(!Physics.Raycast(transform.position + Vector3.up, transform.forward, 15f, DungeonSettings.ObstacleMask))
                {
                    if (character == null)
                    {
                        OnLookingAtCharacter?.Invoke(this, EventArgs.Empty);
                    }
                    character = testCharacter;
                    
                    return true;
                }
            }
        }
        if(character != null)
        {
            OnStopLookingAtCharacter?.Invoke(this, EventArgs.Empty);
            character = null;
        }
        return false;
    }

    private bool IsObstacleClose()
    {
        return Physics.Raycast(transform.position + Vector3.up, transform.forward, 2f, DungeonSettings.ObstacleMask);

    }
}

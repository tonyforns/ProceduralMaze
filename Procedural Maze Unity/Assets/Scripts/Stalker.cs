using System;
using UnityEngine;

public class Stalker : Enemy
{
    [SerializeField] private Character character;

    [SerializeField] private string cornerId = "010011000";

    private float timer;
    private float spawnTimerAmount = 5;
    private bool isLooking;


    private void Start()
    {
        isLooking = false;
        Spawn();
        timer = spawnTimerAmount;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Spawn();
            timer = spawnTimerAmount;
        }
        LookAtCharacter();


    }
    public void Spawn()
    {
        Vector2Int characterDugeonPosition = Dungeon.Instance.WorldPositionToDungeon(character.transform.position);
        Vector2Int characterForward = new Vector2Int(Mathf.RoundToInt(character.transform.forward.x), Mathf.RoundToInt(character.transform.forward.z));

        Vector2Int stalkerDungeonPosition = Dungeon.Instance.GetNextDungeonSegmentWithId(cornerId, characterDugeonPosition, characterForward);

        if (stalkerDungeonPosition != Vector2Int.one * -1)
        {
            stalkerDungeonPosition = Dungeon.Instance.GetNextDungeonSegment(stalkerDungeonPosition, characterForward);
            Vector3 stalkerNewPosition = Dungeon.Instance.DungeonPositionToWorld(stalkerDungeonPosition);
            transform.position = stalkerNewPosition;
        }
    }

    public void LookAtCharacter()
    {
        transform.LookAt(new Vector3(character.transform.position.x, 0, character.transform.position.z));
        Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward * 15f, Color.red);

        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, 15f, DungeonSettings.CharacterMask))
        {
            if(!Physics.Raycast(transform.position + Vector3.up, transform.forward, Vector3.Distance(transform.position, character.transform.position), DungeonSettings.ObstacleMask)) {
                if (!isLooking)
                {
                    isLooking = true;
                    OnLookingAtCharacter?.Invoke(this, EventArgs.Empty);
                }
                return;
            } 
        }
        if (isLooking)
        {
            isLooking = false;
            OnStopLookingAtCharacter?.Invoke(this, EventArgs.Empty);
        }
    }
}

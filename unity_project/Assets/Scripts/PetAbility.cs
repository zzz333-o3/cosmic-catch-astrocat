using UnityEngine;
using System.Collections.Generic;

public class PetAbility : MonoBehaviour
{
    public enum PetType { ApplePicker, BananaCatcher, OrangeSeeker, GrapeGatherer, SlimeHunter, DiamondMiner }
    
    [Header("Main Settings")]
    public PetType type;
    public float detectionRadius = 15f; 
    public float petSpeed = 25f; 

    [Header("Movement")]
    public Vector3 followOffset = new Vector3(2f, 2f, 0f);
    public float smoothSpeed = 6f;

    private Transform player;
    private string targetTag;
    private Transform currentTarget; 

    void Start()
    {
        FindPlayer();
        SetTargetTag();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void SetTargetTag()
    {
        switch (type)
        {
            case PetType.ApplePicker: targetTag = "apple"; break;
            case PetType.BananaCatcher: targetTag = "banana"; break;
            case PetType.OrangeSeeker: targetTag = "orange"; break;
            case PetType.GrapeGatherer: targetTag = "grape"; break;
            case PetType.DiamondMiner: targetTag = "diamond"; break;
            case PetType.SlimeHunter: targetTag = "slime"; break;
        }
    }

    void Update()
    {
        if (player == null) { FindPlayer(); return; }

        // 1. ИЩЕМ ЦЕЛЬ 🍎
        if (currentTarget == null)
        {
            FindNextTarget();
        }

        // 2. ОХОТА ИЛИ ВОЗВРАТ 🚀🏠
        if (currentTarget != null)
        {
            float distToTarget = Vector3.Distance(transform.position, currentTarget.position);
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            // ЛЕТИМ К ЯБЛОКУ 🛸👟🍎
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, petSpeed * Time.deltaTime);
            
            // Если мы уже "подобрали" яблоко (совсем рядом)
            if (distToTarget < 0.6f)
            {
                 if (type == PetType.SlimeHunter)
                 {
                     Destroy(currentTarget.gameObject);
                     currentTarget = null;
                 }
                 else
                 {
                     // НЕСЕМ К КОТИКУ! 🍎🏠
                     currentTarget.position = Vector3.MoveTowards(currentTarget.position, player.position, petSpeed * 3 * Time.deltaTime);
                     
                     // ВАЖНО: Если до Котика осталось меньше 1 метра — МЫ ВЫПОЛНИЛИ РАБОТУ! ✨🎁
                     if (distToPlayer < 1.0f)
                     {
                         // Бросаем яблоко (оно само упадет в триггер) и ищем следующее! 🍎🧺💨
                         currentTarget = null; 
                     }
                 }
            }
        }
        else
        {
            // СЛЕДУЕМ ЗА ИГРОКОМ ✨🐈
            Vector3 targetPosition = player.position + followOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

        transform.rotation = Quaternion.identity; 
    }

    void FindNextTarget()
    {
        FallingItem[] items = FindObjectsOfType<FallingItem>();
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (FallingItem item in items)
        {
            if (item == null) continue;

            if (item.gameObject.name.ToLower().Contains(targetTag))
            {
                float dist = Vector3.Distance(transform.position, item.transform.position);
                if (dist <= detectionRadius && dist < closestDist)
                {
                    closestDist = dist;
                    bestTarget = item.transform;
                }
            }
        }

        currentTarget = bestTarget;
    }
}

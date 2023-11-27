using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance; // Singleton pattern to access it globally
    [Header("Objective system"),SerializeField]
    public Objective mainObjective;
    public Objective sideObjective;
    public TextMeshProUGUI mainObjectiveText; // Reference to the TextMeshPro field for main objectives
    public TextMeshProUGUI sideObjectiveText; // Reference to the TextMeshPro field for side objectives

    private Dictionary<CropType, int> collectiblesCollected = new ();

    private int stars = 0;
    private bool giveStarMain1 = true;
    private bool giveStarSide2 = true;
    private bool giveStarSide3 = true;

    public bool UnlockDoor()
    {
        return ObjectiveCompleted(mainObjective);
    }

    private bool ObjectiveCompleted(Objective obj)
    {
        if (collectiblesCollected[obj.collectibleType] >= obj.requiredAmount)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        Instance = this;
        InitializeCollectibles();
        UpdateObjectiveText();
        stars = 0;
    }

    private void InitializeCollectibles()
    {
        if(mainObjective.collectibleType == null)
            Debug.LogError("Main objective collectible type is null!");
        else
        collectiblesCollected.Add(mainObjective.collectibleType, 0);

        if(sideObjective.collectibleType == null)
            Debug.LogError("Side objective collectible type is null!");
        else
        collectiblesCollected.Add(sideObjective.collectibleType, 0);
    }

    public void HandleCropDestroyed(CropType destroyedCropType)
    {
        if (collectiblesCollected.ContainsKey(destroyedCropType))
        {
            collectiblesCollected[destroyedCropType]++;

            CheckObjectiveCompletion(mainObjective, ref giveStarMain1);
            CheckObjectiveCompletion(sideObjective, ref giveStarSide2);
            CheckSideObjectiveHalf(sideObjective, ref giveStarSide3);
            UpdateObjectiveText();
        }
    }

    private void CheckObjectiveCompletion(Objective objective, ref bool giveStar)
    {
        if (collectiblesCollected[objective.collectibleType] >= objective.requiredAmount)
        {

            Debug.Log($"Objective completed: Gathered {objective.requiredAmount} / {objective.requiredAmount} {objective.collectibleType.name} ");

            if (giveStar)
            {
                stars++;
                giveStar = false;
            }
        }
    }
    private void CheckSideObjectiveHalf(Objective objective, ref bool giveStar)
    {
        if ((collectiblesCollected[objective.collectibleType] >= (objective.requiredAmount)/2))
        {
            if (giveStar)
            {
                stars++;
                giveStar = false;
            }
        }
    }

    private void UpdateObjectiveText()
    {
        if (ObjectiveCompleted(mainObjective))
        {
            mainObjectiveText.text = "Main objective completed!";
        }
        else
        {
            UpdateTextForObjective(mainObjective, mainObjectiveText);
        }

        if (ObjectiveCompleted(sideObjective))
        {
            sideObjectiveText.text = "Side objective completed!";
        }
        else
        {
            UpdateTextForObjective(sideObjective, sideObjectiveText);
        }
    }

    private void UpdateTextForObjective(Objective objective, TextMeshProUGUI textMesh)
    {
        string collectibleName = objective.collectibleType.name;
        int collectedAmount = collectiblesCollected[objective.collectibleType];
        int requiredAmount = objective.requiredAmount;
        textMesh.text = $"{collectibleName}: {collectedAmount}/{requiredAmount}\n";
    }

    public int StarCount()
    {
        return stars;
    }
}

[System.Serializable]
public class Objective
{
    public CropType collectibleType;
    public int requiredAmount;
}

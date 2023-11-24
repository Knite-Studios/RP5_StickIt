using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance; // Singleton pattern to access it globally
    [Header("Objectives"),SerializeField]
    List<Objective> mainObjectives = new ();
    public List<Objective> sideObjectives = new ();
    public TextMeshProUGUI mainObjectiveText; // Reference to the TextMeshPro field for main objectives
    public TextMeshProUGUI sideObjectiveText; // Reference to the TextMeshPro field for side objectives

    private Dictionary<CropType, int> collectiblesCollected = new ();

    private int stars = 0;
    private bool getstars = true;
    public bool UnlockDoor()
    {
        return ObjectiveCompleted(mainObjectives[0]);
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
        // Initialize the dictionary with all collectibles
        foreach (Objective objective in mainObjectives)
        {
            collectiblesCollected.Add(objective.collectibleType, 0);
        }
        foreach (Objective objective in sideObjectives)
        {
            collectiblesCollected.Add(objective.collectibleType, 0);
        }
    }
    public void HandleCropDestroyed(CropType destroyedCropType)
    {
        if (collectiblesCollected.ContainsKey(destroyedCropType))
        {
            collectiblesCollected[destroyedCropType]++;
            // Check if objectives are completed
            foreach (Objective objective in mainObjectives)
            {
                if (objective.collectibleType == destroyedCropType)
                {
                    CheckObjectiveCompletion(objective);
                    UpdateObjectiveText();
                    break;
                }
            }
            foreach (Objective objective in sideObjectives)
            {
                if (objective.collectibleType == destroyedCropType)
                {
                    CheckObjectiveCompletion(objective);
                    if ((collectiblesCollected[objective.collectibleType] >= (objective.requiredAmount/2)) && getstars)
                    {
                        stars++;
                    }
                    UpdateObjectiveText();
                    break;
                }
            }
        }
    }

    private void CheckObjectiveCompletion(Objective objective)
    {
        if (collectiblesCollected[objective.collectibleType] >= objective.requiredAmount)
        {
            stars++;
            // Objective completed
            Debug.Log($"Objective completed: Gathered {objective.requiredAmount} {objective.collectibleType.name}");

            // Handle UI or other actions for completed objective
        }
    }

    private void UpdateObjectiveText()
    {
        // Update the text for the main and side objectives
        if (ObjectiveCompleted(mainObjectives[0]))
        {
            mainObjectiveText.text = "Main objective completed!";

        }

        else
        UpdateTextForObjectiveType(mainObjectives, mainObjectiveText);

        if (ObjectiveCompleted(sideObjectives[0]))
            sideObjectiveText.text = "Side objective completed!";
        else
        UpdateTextForObjectiveType(sideObjectives, sideObjectiveText);
    }

    private void UpdateTextForObjectiveType(List<Objective> objectives, TextMeshProUGUI textMesh)
    {
        // Update the text for the given objective type
        string objectivesString = "";
        foreach (Objective objective in objectives)
        {
            string collectibleName = objective.collectibleType.name;
            int collectedAmount = collectiblesCollected[objective.collectibleType];
            int requiredAmount = objective.requiredAmount;
            objectivesString += $"{collectibleName}: {collectedAmount}/{requiredAmount}\n";
        }
        textMesh.text = objectivesString;
 
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

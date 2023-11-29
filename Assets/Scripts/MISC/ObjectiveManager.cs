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
    public GameObject CheckBoxMainMenu;
    public GameObject CheckBoxSideMenu;

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
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        if(CheckBoxMainMenu != null)
        {
            CheckBoxMainMenu.SetActive(false);
        }
        if(CheckBoxSideMenu != null)
        {
            CheckBoxSideMenu.SetActive(false);
        }
        Instance = this;
        InitializeCollectibles();
        UpdateObjectiveText();
        stars = 0;
    }

    private void InitializeCollectibles()
    {
        collectiblesCollected.Add(mainObjective.collectibleType, 0);
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
            if (collectiblesCollected[mainObjective.collectibleType] >= mainObjective.requiredAmount)
            {
                mainObjectiveText.color = Color.green;
                mainObjectiveText.text = $"{mainObjective.collectibleType.name}\n";
                if (CheckBoxMainMenu != null)
                    CheckBoxMainMenu.SetActive(true);
            }
            else
            {
                UpdateTextForObjective(mainObjective, mainObjectiveText);
            }
        }
        else
        {
            UpdateTextForObjective(mainObjective, mainObjectiveText);
        }

        if (ObjectiveCompleted(sideObjective))
        {
            if (collectiblesCollected[sideObjective.collectibleType] >= sideObjective.requiredAmount)
            {

                sideObjectiveText.color = Color.green;
                sideObjectiveText.text = $"{sideObjective.collectibleType.name}\n";
                if (CheckBoxSideMenu != null)
                    CheckBoxSideMenu.SetActive(true);
            }
            else
            {
                UpdateTextForObjective(sideObjective, sideObjectiveText);
            }
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
        int requiredAmount = objective.requiredAmount - collectedAmount; 

        textMesh.text = $"{collectibleName}: {requiredAmount}\n";
        
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

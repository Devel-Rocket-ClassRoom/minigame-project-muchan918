using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialStep
{
    Move,
    Attack,
    ChopTree,
    PickupItem,
    MineStone,
    PickupStone,
    OpenInventory,
    CheckItemInfo,
    PassiveAnimal,
    PickupPassiveAnimalItem,
    HostileAnimal,
    PickupHostileAnimalItem,
    GoToWorkbench,
    Craft,
    GoToAltar,
    SubmitTribute,
    GoToCauldron,
    Cook,
    EatFood,
    HungerExplain,
    StartDay,
    Storage,
    Cauldron,
    Upgrade,
    Complete,
}

public class TutorialSceneManager : MonoBehaviour
{
    public static TutorialSceneManager Instance { get; private set; }

    [Header("Debug")]
    public TutorialStep startStep = TutorialStep.Move;

    [Header("References")]
    public DayNightCycle dayNightCycle;
    public PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    public UiItemInfo uiItemInfo;
    public TutorialDirectionArrow directionArrow;
    public TributeEvent tributeEvent;
    public CraftInteraction craftInteraction;
    public CauldronInteraction cauldronInteraction;

    [Header("Explain Panels")]
    public GameObject altarExplainPanel;

    [Header("Guide Panels")]
    public GameObject movePanel;
    public GameObject attackPanel;
    public GameObject chopTreePanel;
    public GameObject pickupItemPanel;
    public GameObject mineStonePanel;
    public GameObject pickupStonePanel;
    public GameObject openInventoryPanel;
    public GameObject checkItemInfoPanel;
    public GameObject passiveAnimalPanel;
    public GameObject pickupPassiveAnimalItemPanel;
    public GameObject hostileAnimalPanel;
    public GameObject pickupHostileAnimalItemPanel;
    public GameObject goToWorkbenchPanel;
    public GameObject craftPanel;
    public GameObject goToAltarPanel;
    public GameObject submitTributePanel;
    public GameObject goToCauldronPanel;
    public GameObject cookPanel;
    public GameObject eatFoodPanel;
    public GameObject hungerExplainPanel;
    public GameObject startDayPanel;

    [Header("Targets")]
    public Transform chopTreeTarget;
    public Transform mineStoneTarget;
    public Transform passiveAnimalTarget;
    public Transform hostileAnimalTarget;
    public Transform craftTarget;
    public Transform altarTarget;
    public Transform cauldronTarget;
    public Transform cabinTarget;

    [Header("Tutorial Objects")]
    public GameObject chopTreeObject;
    public GameObject mineStoneObject;
    public GameObject passiveAnimalObject;
    public GameObject hostileAnimalObject;

    [Header("Facilities")]
    public GameObject cabinInteraction;
    public GameObject workbench;
    public GameObject storage;
    public GameObject cauldron;
    public GameObject altar;
    public GameObject upgradeBuilding;

    [Header("Tribute")]
    public TributeRequirement tutorialTributeRequirement;

    private int initialInventoryCount = 0;

    public TutorialStep CurrentStep { get; private set; } = TutorialStep.Move;

    private void Awake()
    {
        Instance = this;

        chopTreeObject.SetActive(false);
        mineStoneObject.SetActive(false);
        passiveAnimalObject.SetActive(false);
        hostileAnimalObject.SetActive(false);

        cabinInteraction.SetActive(false);
        workbench.SetActive(false);
        storage.SetActive(false);
        cauldron.SetActive(false);
        altar.SetActive(false);
        upgradeBuilding.SetActive(false);
    }

    private void Update()
    {
        switch (CurrentStep)
        {
            case TutorialStep.Move:
                if (playerMovement.MoveInput.sqrMagnitude > 0.01f)
                    CompleteStep(TutorialStep.Move);
                break;
            case TutorialStep.Attack:
                if (PlayerAction.Instance.IsActing)
                    CompleteStep(TutorialStep.Attack);
                break;
            case TutorialStep.PickupItem:
                if (playerInventory.SlotList.SlotDataList.Count > initialInventoryCount)
                    CompleteStep(TutorialStep.PickupItem);
                break;
            case TutorialStep.PickupStone:
                if (playerInventory.SlotList.SlotDataList.Count > initialInventoryCount)
                    CompleteStep(TutorialStep.PickupStone);
                break;
            case TutorialStep.OpenInventory:
                if (playerInventory.IsOpen)
                    CompleteStep(TutorialStep.OpenInventory);
                break;
            case TutorialStep.CheckItemInfo:
                if (uiItemInfo.gameObject.activeSelf)
                    CompleteStep(TutorialStep.CheckItemInfo);
                break;
            case TutorialStep.PickupPassiveAnimalItem:
                if (playerInventory.SlotList.SlotDataList.Count > initialInventoryCount)
                    CompleteStep(TutorialStep.PickupPassiveAnimalItem);
                break;
            case TutorialStep.PickupHostileAnimalItem:
                if (playerInventory.SlotList.SlotDataList.Count > initialInventoryCount)
                    CompleteStep(TutorialStep.PickupHostileAnimalItem);
                break;
            case TutorialStep.SubmitTribute:
                if (tributeEvent.tributeSlotList.IsAllComplete())
                    CompleteStep(TutorialStep.SubmitTribute);
                break;
            case TutorialStep.EatFood:
                if (PlayerHunger.Instance.CurrentHunger > 0)
                    CompleteStep(TutorialStep.EatFood);
                break;
        }
    }

    public void CompleteStep(TutorialStep step)
    {
        if (CurrentStep != step)
            return;

        CurrentStep = (TutorialStep)((int)CurrentStep + 1);
        OnStepStart(CurrentStep);
    }

    private void OnStepStart(TutorialStep step)
    {
        directionArrow.gameObject.SetActive(false);
        movePanel.SetActive(false);
        attackPanel.SetActive(false);
        chopTreePanel.SetActive(false);
        pickupItemPanel.SetActive(false);
        mineStonePanel.SetActive(false);
        pickupStonePanel.SetActive(false);
        openInventoryPanel.SetActive(false);
        checkItemInfoPanel.SetActive(false);
        passiveAnimalPanel.SetActive(false);
        pickupPassiveAnimalItemPanel.SetActive(false);
        hostileAnimalPanel.SetActive(false);
        pickupHostileAnimalItemPanel.SetActive(false);
        goToWorkbenchPanel.SetActive(false);
        craftPanel.SetActive(false);
        altarExplainPanel.SetActive(false);
        goToAltarPanel.SetActive(false);
        submitTributePanel.SetActive(false);
        goToCauldronPanel.SetActive(false);
        cookPanel.SetActive(false);
        eatFoodPanel.SetActive(false);
        hungerExplainPanel.SetActive(false);
        startDayPanel.SetActive(false);

        switch (step)
        {
            case TutorialStep.Move:
                movePanel.SetActive(true);
                break;
            case TutorialStep.Attack:
                attackPanel.SetActive(true);
                break;
            case TutorialStep.ChopTree:
                chopTreePanel.SetActive(true);
                chopTreeObject.SetActive(true);
                directionArrow.SetTarget(chopTreeTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.PickupItem:
                pickupItemPanel.SetActive(true);
                initialInventoryCount = playerInventory.SlotList.SlotDataList.Count;
                break;
            case TutorialStep.MineStone:
                mineStonePanel.SetActive(true);
                mineStoneObject.SetActive(true);
                directionArrow.SetTarget(mineStoneTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.PickupStone:
                pickupStonePanel.SetActive(true);
                initialInventoryCount = playerInventory.SlotList.SlotDataList.Count;
                break;
            case TutorialStep.OpenInventory:
                openInventoryPanel.SetActive(true);
                break;
            case TutorialStep.CheckItemInfo:
                checkItemInfoPanel.SetActive(true);
                break;
            case TutorialStep.PassiveAnimal:
                passiveAnimalPanel.SetActive(true);
                passiveAnimalObject.SetActive(true);
                directionArrow.SetTarget(passiveAnimalTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.PickupPassiveAnimalItem:
                pickupPassiveAnimalItemPanel.SetActive(true);
                initialInventoryCount = playerInventory.SlotList.SlotDataList.Count;
                break;
            case TutorialStep.HostileAnimal:
                hostileAnimalPanel.SetActive(true);
                hostileAnimalObject.SetActive(true);
                directionArrow.SetTarget(hostileAnimalTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.PickupHostileAnimalItem:
                pickupHostileAnimalItemPanel.SetActive(true);
                initialInventoryCount = playerInventory.SlotList.SlotDataList.Count;
                break;
            case TutorialStep.GoToWorkbench:
                goToWorkbenchPanel.SetActive(true);
                workbench.SetActive(true);
                directionArrow.SetTarget(craftTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.Craft:
                craftPanel.SetActive(true);
                break;
            case TutorialStep.GoToAltar:
                craftInteraction.OnClickClose();
                altarExplainPanel.SetActive(true);
                altar.SetActive(true);
                tributeEvent.SetTutorialRequirement(tutorialTributeRequirement);
                break;
            case TutorialStep.SubmitTribute:
                submitTributePanel.SetActive(true);
                break;
            case TutorialStep.GoToCauldron:
                goToCauldronPanel.SetActive(true);
                cauldron.SetActive(true);
                directionArrow.SetTarget(cauldronTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.Cook:
                cookPanel.SetActive(true);
                break;
            case TutorialStep.EatFood:
                eatFoodPanel.SetActive(true);
                break;
            case TutorialStep.HungerExplain:
                if (playerInventory.IsOpen)
                    playerInventory.Toggle();
                PlayerHunger.Instance.AddFullHunger();
                hungerExplainPanel.SetActive(true);
                break;
            case TutorialStep.StartDay:
                startDayPanel.SetActive(true);
                dayNightCycle.StartTutorialDay();
                cabinInteraction.SetActive(true);
                directionArrow.SetTarget(cabinTarget);
                directionArrow.gameObject.SetActive(true);
                break;
            case TutorialStep.Storage:
                storage.SetActive(true);
                break;
            case TutorialStep.Cauldron:
                cauldron.SetActive(true);
                break;
            case TutorialStep.Upgrade:
                upgradeBuilding.SetActive(true);
                break;
            case TutorialStep.Complete:
                OnTutorialComplete();
                break;
        }
    }

    private void OnTutorialComplete()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnMapReady()
    {
        CurrentStep = startStep;
        OnStepStart(CurrentStep);
    }

    public void OnCloseAltarExplain()
    {
        altarExplainPanel.SetActive(false);
        goToAltarPanel.SetActive(true);
        directionArrow.SetTarget(altarTarget);
        directionArrow.gameObject.SetActive(true);
    }

    public void OnCloseHungerExplain()
    {
        hungerExplainPanel.SetActive(false);
        CompleteStep(TutorialStep.HungerExplain);
    }
}

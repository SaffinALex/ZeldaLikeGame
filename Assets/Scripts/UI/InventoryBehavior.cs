using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehavior : MonoBehaviour
{
    public List<BaseWeapon> weapons;
    public GameObject selector;
    private int index_x, index_y;
    public float offset_x, offset_y;
    public int columns, lines;
    private List<List<BaseWeapon>> weaponsInventory;
    public List<Slot> slots;
    public MainSlotBehavior slotA, slotB;
    private bool canMoveSelector;
    private Vector2 beginSelectorPos;
    public AudioClip openMenu;
    public AudioClip moveSelector;
    public AudioClip selectItem;
    public AudioClip closeMenu;
    public GameObject inventoryUI;
    private bool canOpenMenu;
    // Start is called before the first frame update
    void Awake()
    {
        index_x = 0;
        index_y = 0;
        weaponsInventory = new List<List<BaseWeapon>>();
        Debug.Assert(weapons.Count >= columns * lines, "Weapons to big or small");
        int index = 0;
        for (int i = 0; i < lines; i++)
        {
            List<BaseWeapon> list1 = new List<BaseWeapon>();
            for(int j = 0; j<columns; j++)
            {
                list1.Add(weapons[index]);
                index++;
            }
            weaponsInventory.Add(list1);
            canMoveSelector = false;
            beginSelectorPos = selector.transform.position;
        }
        canOpenMenu = true;
       
    }
    private void Start()
    {
        RefreshInventory();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        inventoryUI.SetActive(false);
        enabled = false;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                index_y += 1;
                canMoveSelector = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                index_x += 1;
                canMoveSelector = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                index_x -= 1;
                canMoveSelector = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                index_y -= 1;
                canMoveSelector = true;
            }

            if (index_x >= lines)
            {
                index_x = 0;
            }
            else if (index_x < 0)
            {
                index_x = lines - 1;
            }
            if (index_y >= columns)
            {
                index_y = 0;
            }
            else if (index_y < 0)
            {
                index_y = columns - 1;
            }

            if (canMoveSelector)
            {
                selector.transform.position = new Vector3(beginSelectorPos.x + index_x * offset_x, beginSelectorPos.y + -offset_y * index_y);
                GameVariables.Instance.gameAudioSource.PlayOneShot(moveSelector);
                canMoveSelector = false;
            }
            Debug.Log(" " + index_x + " " + index_y);
            if (Input.GetKeyDown(KeyCode.A))
            {
                BaseWeapon tmp = slotA.weapon;
                slotA.weapon = (weaponsInventory[index_y][index_x]);
                slotA.Refresh();
                weaponsInventory[index_y][index_x] = tmp;
                GameVariables.Instance.gameAudioSource.PlayOneShot(selectItem);
                RefreshInventory();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                BaseWeapon tmp = slotB.weapon;
                slotB.weapon = (weaponsInventory[index_y][index_x]);
                slotB.Refresh();
                weaponsInventory[index_y][index_x] = tmp;
                GameVariables.Instance.gameAudioSource.PlayOneShot(selectItem);
                RefreshInventory();
            }
            
        
    }

    public void RefreshInventory()
    {
        int index = 0;
        for(int i = 0; i<lines; i++)
        {
            for(int j = 0; j<columns; j++)
            {
                slots[index].weapon = weaponsInventory[i][j];
                slots[index].Refresh();
                index++;
            }
        }
        slotA.Refresh();
        slotB.Refresh();
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        canOpenMenu = true;
        if (gameState == GameStateManager.GameState.Pause)
        {
            enabled = true;
            inventoryUI.SetActive(true);
            selector.transform.position = beginSelectorPos;
            GameVariables.Instance.gameAudioSource.PlayOneShot(openMenu);
        }
        else if (gameState == GameStateManager.GameState.Playing)
        {
            GameVariables.Instance.gameAudioSource.PlayOneShot(closeMenu);
            inventoryUI.SetActive(false);
            enabled = false;
        }
        else if (gameState == GameStateManager.GameState.Talking) canOpenMenu = false;
    }
}

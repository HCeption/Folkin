using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconsStage : MonoBehaviour
{
    public Card Card;
    public SillhuetteStage SillhuetteStage;
    public List<Button> iconButtons = new List<Button>();
    public List<string> AllIconPaths = new List<string>();
    private Canvas thisCanvas;
    public List<string> IconValues = new List<string>();
    public TextMeshProUGUI iconvalueCount;
    public bool buttonHighlighted = false;
    public GameObject CardIconPrefab;
    public bool IconsMade = false;
    public List<List<Button>> buttonPages = new List<List<Button>>();
    private int currentPage;
    private int pageCount;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        thisCanvas = gameObject.GetComponentInChildren<Canvas>();
        pageCount = 0;
        FindAllIconPaths();
        AddButtons();
        OrderButtons();
        AddAllButtonListeners();
        SetPage(0);
        currentPage = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        iconvalueCount.text = StringFromList(IconValues);

    }
    public void FindAllIconPaths()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("ICONS");
        
        int i = 0;
        List<string> iconPaths = new List<string>();
        foreach (Texture2D texture in textures)
        {
            string s = texture.name;
            string plusS = "ICONS/" + s;
            iconPaths.Add(plusS);
            i++;
        }
        AllIconPaths = iconPaths;
    }
    //"Assets/Resources/ICONS/Asset 706logo.png"

    public void AddButtons()
    {

        
        List<Button> buttons = new List<Button>();
        List<Button> buttonPage = new List<Button>();
        foreach (string iconPath in AllIconPaths)
        {
            GameObject ButtonObj = new GameObject("ButtonObj");
            Button button = ButtonObj.gameObject.AddComponent<Button>();
            Image buttonImage = ButtonObj.gameObject.AddComponent<Image>();

            Texture2D imgTexture = (Texture2D)Resources.Load(iconPath);
            buttonImage.sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));
            ButtonObj.transform.SetParent(thisCanvas.transform, false);
            button.targetGraphic = buttonImage;

            buttons.Add(button);



        }
        createIconPageLists();
        void createIconPageLists()
        {
            for (int i = 0; i < AllIconPaths.Count; i += 8)
            {
                buttonPages.Add(new List<Button>());

                for (int y = i; y < (i + 8); y++)
                {
                    if (AllIconPaths.Count - y > 0)
                    buttonPages[pageCount].Add(buttons[y]);
                }
                pageCount++;
            }
        }
        iconButtons = buttons;
    }
    public void SaveInputToCard()
    {
        if (IconsMade)
        {

            Card.iconValues = IconValues.ToArray();

            gameObject.SetActive(false);
            SillhuetteStage.gameObject.SetActive(true);
        }
        else
        {

        }

    }
    public void OrderButtons()
    {
        foreach (List<Button> buttonList in buttonPages)
        {
            int horizontalButtons = 0;
            int buttonPositionX = -407;
            int buttonPositionY = -117;
            foreach (Button item in buttonList)
            {
                item.transform.localPosition = new Vector2(buttonPositionX, buttonPositionY);
                buttonPositionX += 100;
                horizontalButtons++;
                if (horizontalButtons == 4)
                {
                    buttonPositionX = -407;
                    buttonPositionY -= 100;
                    horizontalButtons = 0;
                }
                item.gameObject.SetActive(false);
            }
        }
    }
    public void SetPage(int currentPage)
    {
        List<Button> currentButtons = new List<Button>();

        currentButtons = buttonPages[currentPage];
        foreach (Button item in currentButtons)
        {
            item.gameObject.SetActive(true);
        }

    }
    public void UnloadPage(int currentPage)
    {
        List<Button> currentButtons = new List<Button>();

        currentButtons = buttonPages[currentPage];
        foreach (Button item in currentButtons)
        {
            item.gameObject.SetActive(false);
        }
    }
    public void nextPage()
    {
        if (currentPage + 1 < pageCount)
        {
            UnloadPage(currentPage);
            currentPage++;
            SetPage(currentPage);
        }

    }
    public void lastPage()
    {
        if (currentPage - 1 >= 0)
        {
            UnloadPage(currentPage);
            currentPage--;
            SetPage(currentPage);
        }
    }
    public void AddButtonListener(Button button, string filePath)
    {
        button.onClick.AddListener(() =>
        {

            if (!IconValues.Contains(filePath))
            {
                //ColorBlock colorVar = button.colors;
                //colorVar.normalColor = Color.blue;
                //button.colors = colorVar;
                button.GetComponent<Image>().color = Color.blue;
                AddIconValueToList(filePath);
            }
            else
            {
                //ColorBlock colorVar = button.colors;
                //colorVar.normalColor = Color.red;
                //button.colors = colorVar;
                button.GetComponent<Image>().color = Color.white;
                RemoveIconValueFromList(filePath);
            }
        }
        );
    }
    public void AddAllButtonListeners()
    {
        int index = 0;
        foreach (Button item in iconButtons)
        {
            AddButtonListener(item, AllIconPaths[index]);
            index++;
        }
    }
    string StringFromList(List<string> filepaths)
    {
        var result = new StringBuilder();
        foreach (string i in IconValues)
        {
            result.Append(i);
        }
        return result.ToString();
    }
    public void AddIconValueToList(string filePath)
    {
        IconValues.Add(filePath);
    }
    public void RemoveIconValueFromList(string filePath)
    {
        var indexToRemove = IconValues.Find(x => x == filePath);
        IconValues.Remove(indexToRemove);
    }
    public GameObject[] Icons
    {
        get
        {

            GameObject[] _icons = new GameObject[IconValues.Count];
            int iconIndex = 0;
            foreach (string var in IconValues)
            {
                GameObject gameObject = new GameObject();
                Texture2D imgTexture = (Texture2D)Resources.Load(var);
                Image iconIMG = gameObject.gameObject.AddComponent<Image>();
                iconIMG.sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0.5f, 0.5f));

                _icons[iconIndex] = gameObject;
                iconIndex++;
            }
            return _icons;
        }
    }
    public void CreateIcons()
    {
        var cardCanvas = Card.GetComponentInChildren<Canvas>();
        var cardPaper = cardCanvas.GetComponentInChildren<Image>();
        if (IconsMade)
        {
            foreach (Transform icon in cardPaper.transform)
            {
                if (icon.name == "CardIcon(Clone)")
                {
                    Destroy(icon.gameObject);
                }
            }
            IconsMade = false;
        }
        //int paperLeft = (int)cardPaper.transform.position.x - (); -2 / 354, -2 / 531
        //int paperBottom = (int)cardPaper.transform.position.y;
        int iconOffsetX = -141;
        int iconOffsetY = -230;
        int iconCount = 0;

        foreach (var icon in Icons)
        {
            var cardIcon = Instantiate(CardIconPrefab, Vector2.zero, Quaternion.identity);


            cardIcon.transform.SetParent(cardPaper.transform, false);
            cardIcon.transform.localScale = new Vector2(0.7f, 0.7f);
            cardIcon.transform.localPosition = new Vector2(iconOffsetX, iconOffsetY);
            cardIcon.GetComponent<Image>().sprite = icon.GetComponent<Image>().sprite;

            iconOffsetX += 76;
            iconCount++;
            if (iconCount >= 2)
            {
                iconOffsetX = -142;
                iconOffsetY += 76;
                iconCount = 0;
            }
            Destroy(icon);
            IconsMade = true;
        }



    }
}
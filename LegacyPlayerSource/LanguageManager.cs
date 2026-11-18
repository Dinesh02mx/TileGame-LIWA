using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LanguageTool;


namespace LanguageSet
{

    public class LanguageManager : MonoBehaviour
    {
        [SceneName]
        public string EnterScenceName = "FirstSelect";

        [SerializeField]
        private GameObject mChooseGo;
        private string m_WindowsXmlConfigPath = string.Empty;

        //Unicode
        private static LanguageManager instance;

        public static LanguageManager Instance
        {
            get { return instance; }
        }

        public int m_Lanager = -1;
        private bool m_initFirst = true;
        private string m_LanguagePath = String.Empty;
        private Text m_TextTips;
        private Text m_TextLanguage;
        private GameObject m_ChooseGo;
        private Button m_definiteButton;
        private Dropdown m_Dpdown;
        public delegate void LanguageChangeEvent();
        private event LanguageChangeEvent m_Event;
        private Dictionary<string, string> m_Dictionarystr = new Dictionary<string, string>();
        private bool _updateValue = false;

        public void AddEventListener(LanguageChangeEvent m_Eventadd)
        {
            m_Event += m_Eventadd;
        }

        public void RemoveEventListener(LanguageChangeEvent m_Eventadd)
        {
            m_Event -= m_Eventadd;
        }

        public void DispatchEvent()
        {
            m_Dictionarystr.Clear();
            if (m_Event != null)
            {
                try
                {
                    m_Event.Invoke();
                }
                catch (Exception exception)
                {
                    string[] textArray = new string[]
                        {"Error dispatching language event : ", exception.Message, " ", exception.StackTrace};
                    throw new Exception(string.Concat(textArray), exception);
                }
            }
        }

        void Start()
        {
            instance = this;
            Init();
            InitComponentChildren();
        }
        
        void Init()
        {
            DontDestroyOnLoad(gameObject);
            m_LanguagePath = Application.streamingAssetsPath + "/language.txt";
            m_WindowsXmlConfigPath = Application.dataPath + "/../res/Config.xml";
            if (File.Exists(m_WindowsXmlConfigPath))
            {
                
            }
            else
            {
                SceneManager.LoadScene(EnterScenceName);
            }

            if (Landll.SetLanFilePath(m_LanguagePath) != 0)
            {
                mChooseGo.SetActive(true);
                m_TextTips = transform.Find("Canvas/BG/Tips").GetComponent<Text>();
                m_TextTips.gameObject.SetActive(true);
                m_TextTips.text = Landll.Lan_GetLastErrow();
                return;
            }
            m_Dictionarystr = new Dictionary<string, string>();
            string str = string.Empty;
            m_Lanager = XmlManager.ReadXml<int>("Language");
            if (m_Lanager == -1)
            {
                m_initFirst = false;
                _updateValue = true;
                mChooseGo.SetActive(true);
            }
            else if (m_Lanager == 0)
            {
                m_Lanager = Landll.GetCurLanguage();
            }

            if (!m_initFirst)
            {
                m_Lanager = Landll.GetCurLanguage();
            }
        }

        void InitComponentChildren()
        {
             m_TextTips = transform.Find("Canvas/BG/Tips").GetComponent<Text>();
             m_ChooseGo = transform.Find("Canvas/BG/ChooseGo").gameObject;
             m_TextLanguage = transform.Find("Canvas/BG/ChooseGo/LanguageText").GetComponent<Text>();
             m_definiteButton = m_ChooseGo.transform.Find("definiteButton").GetComponent<Button>();
             m_definiteButton.onClick.AddListener(ButtonClick);
             m_Dpdown = m_ChooseGo.transform.Find("LanguageDrop").GetComponent<Dropdown>();
             string[] LanguageType = Landll.GetLanTypeArrBuff();
             for (int i = 0; i < LanguageType.Length; i++)
             {
                 m_Dpdown.options.Add(new Dropdown.OptionData(LanguageType[i]));
             }
            
             if (m_Lanager - 1 >= LanguageType.Length)
             {
                 m_Lanager = 2;
             }
            
             m_ChooseGo.SetActive(false);
             m_Dpdown.value = m_Lanager - 1;
             m_Dpdown.transform.GetChild(0).GetComponent<Text>().text = m_Dpdown.options[m_Dpdown.value].text;
             ReadyEnter();
        }
        
        public void ReadyEnter()
        { 
            ChooseEnterOrInit();
        }

        private void ChooseEnterOrInit()
        {
            if (m_initFirst)
            {
                EnterGame();
            }
            else
            {
                InitChoose();
            }
        }

        private void InitChoose()
        {
            switch (m_Lanager)
            {
                case 0:
                    m_TextLanguage.text = "语言设置:";
                    break;
                case 1:
                    m_TextLanguage.text = "语言设置:";
                    break;
                case 2:
                    m_TextLanguage.text = "Language:";
                    break;
                default:
                    m_TextLanguage.text = "Language:";
                    break;
            }
            m_ChooseGo.SetActive(true);
        }

        private void ButtonClick()
        {
            m_Lanager = m_Dpdown.value + 1;
            DispatchEvent();
            EnterGame();
        }

        void QuitGame()
        {
#if !UNITY_EDITOR
            Application.Quit();
#endif
        }
        
        public string GetStrText(string str,string strName = "")
        {
            return Landll.GetLanStr(str,strName);
        }

        private int success = -1;

        public void EnterGame()
        {
            success = Landll.InitLanguageFile(m_Lanager);
            if (File.Exists(m_LanguagePath))
            {
                print(success + "Language" + m_Lanager);
            }
            if (success == 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                if (_updateValue)
                {
                    XmlManager.updateXML("Language", m_Lanager);
                }
                SceneManager.LoadScene(EnterScenceName);
            }
            else
            {
                m_TextTips.text = Landll.Lan_GetLastErrow();
                m_TextTips.gameObject.SetActive(true);
                Invoke("QuitGame", 5f);
            }
        }
    }
}


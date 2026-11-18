using System;
using UnityEngine;
using UnityEngine.UI;

namespace LanguageSet
{
    [AddComponentMenu("多语言/Text")]
    public class LanguageText : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("默认值")]
        private string defaultValue = String.Empty;
        [Tooltip("读取值的ID")]
        [SerializeField]
        private string getValue = String.Empty;

        private Text m_Text;
        // Use this for initialization
        void Start()
        {
            InitText();
            AddListen();
        }

        void AddListen()
        {
            LanguageManager.Instance.AddEventListener(ChangeLanguage);
        }

        void InitText()
        {
            if (m_Text == null) m_Text = GetComponent<Text>();
            if (string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = m_Text.text;
            }
            string strValue = LanguageManager.Instance.GetStrText(getValue);
            if (!string.IsNullOrEmpty(strValue))
            {
                m_Text.text = strValue;
            }
            else
            {
                m_Text.text = defaultValue;
            }
        }

        void ChangeLanguage()
        {
            InitText();
        }

        void OnDestroy()
        {
            LanguageManager.Instance.RemoveEventListener(ChangeLanguage);
        }
    }
}


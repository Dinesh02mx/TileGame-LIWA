using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class SetLan : MonoBehaviour
{
    enum MyEnum
    {
        System,
        中文,
        English
    }

    [SerializeField]
    private MyEnum _currentLanguage;
    [SerializeField]
    private Text m_controlLightsTip;
    [SerializeField]
    private Text m_nControlCount;
    [SerializeField]
    private Text m_nControlLightCount;
    [SerializeField]
    private Text m_timeInterval;
    [SerializeField]
    private Text m_timeIntervalTip;
    private int Lan = 0;

    // Start is called before the first frame update
    void Start()
    {
        ChangeLan();
    }

    private void OnValidate()
    {
        ChangeLan();
    }

    public void ChangeLan()
    {
        switch (_currentLanguage)
        {
            case MyEnum.System:
                Lan = LanguageTool.Landll.GetCurLanguage();
                break;
            case MyEnum.中文:
                Lan = 1;
                break;
            case MyEnum.English:
                Lan = 2;
                break;
        }
        if (Lan == 1)
        {
            m_controlLightsTip.text = "改变灯的颜色，请查看m_ListControllights 相关的代码";
            m_nControlCount.text = "控制器数量";
            m_nControlLightCount.text = "控制灯的数量";
            m_timeInterval.text = "灯熄灭后过多久后自动亮";
            m_timeIntervalTip.text = "单位:毫秒";
        }
        else
        {
            m_controlLightsTip.text = "Change the color of the light, please check the code related to m_ListControllights";
            m_nControlCount.text = "Number of controllers";
            m_nControlLightCount.text = "Control the number of lights";
            m_timeInterval.text = "How long does it take for the light to go off";
            m_timeIntervalTip.text = "Unit: millisecond";
        }
    }

}

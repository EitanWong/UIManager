using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIData
{
    private static UIData Instence = new UIData();

    public static UIData GetData
    {
        get
        {
            return Instence;
        }
    }

    public List<BaseUIPanel> UIPanleList = new List<BaseUIPanel>();    //UI面板缓存，所有创建的Panel都会缓存在此数组中



   protected readonly string UIPanlePath = "UIPanel/";    //UI面板存放的路径，所有的UI面板都会从此路径创建


    /// <summary>
    /// 检查所有UI面板的状态
    /// </summary>
    private void CheckPanelState()
    {
        if (PanelStack.Count <= 0) return;
        TopPanel = PanelStack.First();
        TopPanel.gameObject.SetActive(true);
        TopPanel.Panel_State = PanelState.Show;
        TopPanel.StartCheck();
        foreach (var VARIABLE in UIPanleList)
        {
            if (VARIABLE.Panel_Index != TopPanel.Panel_Index)
            {
                if (VARIABLE.Panel_State != PanelState.Pause)
                {
                    VARIABLE.Panel_State = PanelState.Pause;
                    VARIABLE.OnPause();
                }
            }
        }
    }

    #region 创建Panel模块

    /// <summary>
    /// 创建一个UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    /// <param name="type">UI面板的类型</param>
    public void CreateUIPanel(string PanelName, PanelType type)
    {
        var PanelObj = Resources.Load<GameObject>(GetData.UIPanlePath + PanelName);

        BaseUIPanel Panel;

        Panel = PanelObj.GetComponent<BaseUIPanel>();
        if (!Panel) Panel = PanelObj.AddComponent<BaseUIPanel>();
        //if (!UIPanleList.Contains(Panel))
        {
            Panel.OnInit(type, UIPanleList.Count, PanelName);
        }
    }

    /// <summary>
    /// 创建一个UI面板
    /// </summary>
    /// <param name="panel">UI面板的名称</param>
    public void CreateUIPanel(BaseUIPanel panel)
    {
        var Panel = Resources.Load<GameObject>(GetData.UIPanlePath + panel.name).GetComponent<BaseUIPanel>();
        //if (!UIPanleList.Contains(Panel))
        {
            Panel.OnInit(Panel.Panel_Type, UIPanleList.Count, panel.Panel_Name);
        }
    }

    #endregion 创建Panel模块

    #region 显示Panel模块

    /// <summary>
    /// 显示UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    /// <param name="type">UI面板的类型</param>
    public void ShowPanel(string PanelName, PanelType type)
    {
        switch (type)
        {
            case PanelType.Subclass:
                CreateUIPanel(PanelName, type);
                break;

            default:
                var UIPanel = GetPanel(PanelName) ?? null;
                if (UIPanel == null) CreateUIPanel(UIPanel);
                if (!PanelStack.Contains(UIPanel)) PanelStack.Push(UIPanel);
                TakePanelToTop(UIPanel);
                //UIPanel.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 显示UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    public void ShowPanel(string PanelName)
    {
        var UIPanel = GetPanel(PanelName);
        if (!UIPanel)
        {
            CreateUIPanel(PanelName, PanelType.Singleton);
            return;
        }

        if (!PanelStack.Contains(UIPanel))
            PanelStack.Push(UIPanel);
        TakePanelToTop(UIPanel);
        //if (UIPanel) UIPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 显示UI面板
    /// </summary>
    /// <param name="UIPanel">指定被显示的UI面板</param>
    public void ShowPanel(BaseUIPanel UIPanel)
    {
        if (!UIPanel)
        {
            Debug.LogError("错误！,缓存中找不到该Panel");
            return;
        }
        //PanelStack.Push(UIPanel);
        if (!PanelStack.Contains(UIPanel)) PanelStack.Push(UIPanel);
        TakePanelToTop(UIPanel);
        //UIPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 显示最顶部的UI面板
    /// </summary>
    public void ShowPanel()
    {
        var UIPanel = PanelStack.Peek();
        if (UIPanel.Panel_Type == PanelType.Subclass)
        {
            if (!PanelStack.Contains(UIPanel)) PanelStack.Push(UIPanel);
            TakePanelToTop(UIPanel);
        }
    }

    #endregion 显示Panel模块

    #region 关闭Panel模块

    /// <summary>
    /// 关闭UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    public void ClosePanel(string PanelName)
    {
        var Panel = GetPanel(PanelName);
        if (!Panel) return;
        RemovePanel_in_Stack(Panel);
        Panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭UI面板
    /// </summary>
    /// <param name="panelindex">UI面板的序号</param>
    public void ClosePanel(int panelindex)
    {
        var Panel = GetPanel(panelindex);
        if (!Panel) return;
        RemovePanel_in_Stack(Panel);
        Panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭UI面板
    /// </summary>
    /// <param name="panel">指定被关闭的UI面板</param>
    public void ClosePanel(BaseUIPanel panel)
    {
        RemovePanel_in_Stack(panel);
        panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭当前显示的UI面板
    /// </summary>
    public void ClosePanel()
    {
        var panel = TopPanel;
        RemovePanel_in_Stack(panel);
        panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭UI面板
    /// </summary>
    /// <param name="panelName">UI面板的名称</param>
    /// <param name="type">UI面板类型</param>
    public void ClosePanel(string panelName, PanelType type)
    {
        var Panel = GetPanel(panelName, type);
        RemovePanel_in_Stack(Panel);
        Panel.gameObject.SetActive(false);
    }

    #endregion 关闭Panel模块

    #region 销毁Panel模块

    /// <summary>
    /// 销毁UI面板
    /// </summary>
    /// <param name="panel">销毁指定的UI面板</param>
    public void DestoryPanel(BaseUIPanel panel)
    {
        UIPanleList.Remove(panel);
        if (PanelStack.Contains(panel))
            RemovePanel_in_Stack(panel);
        Object.Destroy(panel);
    }

    #endregion 销毁Panel模块

    #region 获取Panel模块

    /// <summary>
    /// 获取UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    /// <returns></returns>
    public BaseUIPanel GetPanel(string PanelName)
    {
        if (UIPanleList.Count <= 0) return null;
        BaseUIPanel Find = null;
        foreach (var item in UIPanleList)
        {
            if (item.Panel_Name == PanelName)
            {
                Find = item;
                break;
            }
        }
        return Find;
    }
    /// <summary>
    /// 获取UI面板
    /// </summary>
    /// <param name="PanelIndex">UI面板序号</param>
    /// <returns></returns>
    public BaseUIPanel GetPanel(int PanelIndex)
    {
        BaseUIPanel Find = null;
        foreach (var item in UIPanleList)
        {
            if (item.Panel_Index == PanelIndex)
            {
                Find = item;
                break;
            }
        }
        return Find;
    }
    /// <summary>
    /// 获取UI面板
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    /// <param name="type">UI面板类型</param>
    /// <returns></returns>
    public BaseUIPanel GetPanel(string PanelName, PanelType type)
    {
        if (UIPanleList.Count <= 0) return null;
        BaseUIPanel Find = null;
        foreach (var item in UIPanleList)
        {
            if (item.Panel_Name == PanelName && item.Panel_Type == type)
            {
                Find = item;
                break;
            }
        }
        return Find;
    }

    #endregion 获取Panel模块

    #region 放置Panel到最顶部模块
    /// <summary>
    /// 放置UI面板到最顶部
    /// </summary>
    /// <param name="PanelName">UI面板的名称</param>
    public void TakePanelToTop(string PanelName)
    {
        var Panel = GetPanel(PanelName);
        TakePanelOnTop_in_StackTop(Panel);

        Panel.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 放置UI面板到最顶部
    /// </summary>
    /// <param name="Panelindex">UI面板的序号</param>
    public void TakePanelToTop(int Panelindex)
    {
        var Panel = GetPanel(Panelindex);
        TakePanelOnTop_in_StackTop(Panel);
        Panel.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 放置UI面板到最顶部
    /// </summary>
    /// <param name="Panel">指定被放置的UI面板</param>
    public void TakePanelToTop(BaseUIPanel Panel)
    {
        TakePanelOnTop_in_StackTop(Panel);
        Panel.transform.SetAsLastSibling();
    }
    #endregion

    #region PanelData托管栈中的Panel 模块
    #region Panel栈模块


    public Stack<BaseUIPanel> PanelStack = new Stack<BaseUIPanel>();    //当前所有活动的Panel面板都会在此处托管


    private BaseUIPanel TopPanel;    //当前托管栈中最顶部的面板，一般此面板处于显示状态

    #endregion Panel栈模块
    /// <summary>
    /// 从托管栈中获取UI面板
    /// </summary>
    /// <param name="panel">指定被获取的UI面板</param>
    /// <returns></returns>
    private BaseUIPanel GetPanel_in_Stack(BaseUIPanel panel)
    {
        BaseUIPanel ResultPanel = null;
        if (!PanelStack.Contains(panel))
        {
            Debug.LogError("UIPanel托管栈中不存在该Panel");
        }
        Stack<BaseUIPanel> stackpanel = new Stack<BaseUIPanel>();
        BaseUIPanel pickupPanel;
        int number = PanelStack.Count;
        for (int i = 0; i < number; i++)
        {
            pickupPanel = PanelStack.Pop();
            stackpanel.Push(pickupPanel);
            if (pickupPanel.Panel_Index == panel.Panel_Index)
            {
                ResultPanel = stackpanel.Pop();
                break;
            }
        }

        while (stackpanel.Count > 0)
        {
            PanelStack.Push(stackpanel.Pop());
            if (PanelStack.Count <= 0) break;
        }

        return ResultPanel;
    }
    /// <summary>
    /// 从托管栈中删除UI面板
    /// </summary>
    /// <param name="panel">指定被删除的UI面板</param>
    private void RemovePanel_in_Stack(BaseUIPanel panel)
    {
        if (!panel) return;
        if (!PanelStack.Contains(panel))
        {
            Debug.LogError("UIPanel托管栈中不存在该Panel");
            return;
        }

        var removepanel = GetPanel_in_Stack(panel);
        removepanel.StopCheck();
        removepanel.OnClose();
        removepanel.Panel_State = PanelState.Close;
        CheckPanelState();
    }
    /// <summary>
    /// 放置UI面板到托管栈最顶部
    /// </summary>
    /// <param name="panel">指定被放置的UI面板</param>
    private void TakePanelOnTop_in_StackTop(BaseUIPanel panel)
    {
        if (!panel) return;
        if (!PanelStack.Contains(panel))
        {
            Debug.LogError("UIPanel托管栈中不存在该Panel");
            return;
        }

        TopPanel = PanelStack.First();

        if (!TopPanel) return;

        if (TopPanel.Panel_Index == panel.Panel_Index)
        {
            CheckPanelState();
            TopPanel.OnShow();
            return;
        }
        BaseUIPanel Findpanel = GetPanel_in_Stack(panel);
        if (Findpanel)
            PanelStack.Push(Findpanel);
        CheckPanelState();
        TopPanel.OnShow();
    }

    #endregion PanelData托管栈中的Panel 模块
}

public enum PanelState//UI面板状态
{
    Close,//关闭
    Pause,//暂停
    Show,//显示
}
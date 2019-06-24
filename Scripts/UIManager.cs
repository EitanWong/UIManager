using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{

    public static class UIManager
    {
        public  const string CanvasName= "Canvas";//Canvas的名称（所有创建的UI面板都会存放在这Canvas下面）
        /// <summary>
        /// 显示UI面板
        /// </summary>
        public static void ShowPanel()
        {
            UIData.GetData.ShowPanel();
        }
        /// <summary>
        /// 显示UI面板
        /// </summary>
        /// <param name="panel">指定被显示的UI面板</param>
        public static void ShowPanel(BaseUIPanel panel)
        {
            UIData.GetData.ShowPanel(panel);
        }
        /// <summary>
        /// 显示UI面板
        /// </summary>
        /// <param name="panelindex">UI面板序号</param>
        public static void ShowPanel(int panelindex)
        {
            var Panel = UIData.GetData.GetPanel(panelindex);
            if (!Panel)
            {
                Debug.LogErrorFormat("错误！缓存中没有序号为{0}的Panel",panelindex);
                return;
            }
            UIData.GetData.ShowPanel(Panel);
        }
        /// <summary>
        /// 显示UI面板
        /// </summary>
        /// <param name="panelName">UI面板名称</param>
        public static void ShowPanel(string panelName)
        {
            UIData.GetData.ShowPanel(panelName);
        }
        /// <summary>
        /// 显示UI面板
        /// </summary>
        /// <param name="panelname">UI面板名称</param>
        /// <param name="type">UI面板类型</param>
        public static void ShowPanel(string panelname,PanelType type)
        {
            UIData.GetData.ShowPanel(panelname,type);
        }
        /// <summary>
        /// 关闭显示的UI面板
        /// </summary>
        public static void ClosePanel()
        {
           UIData.GetData.ClosePanel();
        }
        /// <summary>
        /// 关闭UI面板
        /// </summary>
        /// <param name="panel">指定被关闭的UI面板</param>
        public static void ClosePanel(BaseUIPanel panel)
        {
            UIData.GetData.ClosePanel(panel);
        }
        /// <summary>
        /// 关闭UI面板
        /// </summary>
        /// <param name="panelindex">UI面板序号</param>
        public static void ClosePanel(int panelindex)
        {
            UIData.GetData.ClosePanel(panelindex);
        }
        /// <summary>
        /// 关闭UI面板
        /// </summary>
        /// <param name="PanelName">UI面板名称</param>
        public static void ClosePanel(string PanelName)
        {
            UIData.GetData.ClosePanel(PanelName);
        }
        /// <summary>
        /// 获取UI面板
        /// </summary>
        /// <param name="PanelIndex">UI面板序号</param>
        /// <returns></returns>
        public static BaseUIPanel GetPanel(int PanelIndex)
        {
            return UIData.GetData.GetPanel(PanelIndex);
        }
        /// <summary>
        /// 获取UI面板
        /// </summary>
        /// <param name="PanelName">UI面板名称</param>
        /// <returns></returns>
        public static BaseUIPanel GetPanel(string PanelName)
        {
            return UIData.GetData.GetPanel(PanelName);
        }
        /// <summary>
        /// 获取UI面板
        /// </summary>
        /// <param name="PanelName">UI面板名称</param>
        /// <param name="type">UI面板类型</param>
        /// <returns></returns>
        public static BaseUIPanel GetPanel(string PanelName, PanelType type)
        {
            return UIData.GetData.GetPanel(PanelName,type);
        }
        /// <summary>
        /// 销毁UI面板
        /// </summary>
        /// <param name="panel">指定被销毁的UI面板</param>
        public static void DestoryPanel(BaseUIPanel panel)
        {
            UIData.GetData.DestoryPanel(panel);
        }
        /// <summary>
        /// 发送事件消息给UI面板
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="data">数据</param>
        public static void SendEvent(UIEventType eventType, object data)
        {
            UIEvent.GetEvent.SendEvent(UIEvent.GetEvent.WriteEventDto(eventType, data));
        }
        /// <summary>
        /// 发送事件消息给UI面板
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="data">数据</param>
        /// <param name="info">信息</param>
        public static void SendEvent(UIEventType eventType,object data,object info)
        {
            UIEvent.GetEvent.SendEvent(UIEvent.GetEvent.WriteEventDto(eventType, data,info));
        }
        /// <summary>
        /// 发送事件消息给所有UI面板
        /// </summary>
        /// <param name="data">数据</param>
        public static void SendEvent(object data)
        {
            UIEvent.GetEvent.SendEvent(UIEvent.GetEvent.WriteEventDto(UIEventType.Send2All, data));
        }
        /// <summary>
        /// 发送事件消息给指定UI面板
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="data">数据</param>
        /// <param name="targetinfo">指定的UI面板的信息(名称/序号)</param>
        public static void SendEvent(object data, object targetinfo)
        {
            UIEvent.GetEvent.SendEvent(UIEvent.GetEvent.WriteEventDto(UIEventType.SendTarget, data, targetinfo));
        }
    }
    public enum PanelType//UI面板类型
    {
        Singleton,//唯一面板
        Subclass,//复合面板
    }
}


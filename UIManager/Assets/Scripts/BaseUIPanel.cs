using System.Collections;
using UnityEngine;
using UIFrameWork.Data;
using UIFrameWork.Event;
namespace UIFrameWork.BasePanel
{
    public class BaseUIPanel : MonoBehaviour
    {
        internal PanelType Panel_Type;//UI面板类型
        internal PanelState Panel_State;//UI面板状态
        internal Coroutine PanelUpdate;//UI面板状态检查协程

        internal int Panel_Index;//UI面板序号
        internal string Panel_Name;//UI面板名称
                                   /// <summary>
                                   /// 初始化UI面板
                                   /// </summary>
                                   /// <param name="Init_type">UI面板类型</param>
                                   /// <param name="Init_index">UI面板序号</param>
                                   /// <param name="Init_name">UI面板名称</param>
        internal void OnInit(PanelType Init_type, int Init_index, string Init_name)
        {
            var Panel = Instantiate(this.gameObject, GameObject.Find(UIManager.CanvasName).transform).GetComponent<BaseUIPanel>();//实例化UI面板
            {//赋值给UI面板进行初始化
                Panel.Panel_Name = Init_name;
                Panel.Panel_Type = Init_type;
                Panel.Panel_Index = Init_index;
                Panel.transform.name = Panel.Panel_Name;
            }
            {//先UI数据 和UI事件添加实例化的UI面板
                UIData.GetData.UIPanleList.Add(Panel);
                UIData.GetData.PanelStack.Push(Panel);
                UIData.GetData.TakePanelToTop(Panel);
                UIEvent.GetEvent.UIPanel_Event += OnEvent;

            }

        }
        /// <summary>
        /// UI面板显示
        /// </summary>
        internal virtual void OnShow()
        {

        }
        /// <summary>
        /// UI面板显示中
        /// </summary>
        internal virtual void OnShouwing()
        {

        }
        /// <summary>
        /// UI面板暂停
        /// </summary>
        internal virtual void OnPause()
        {

        }
        /// <summary>
        /// UI面板暂停中
        /// </summary>
        internal virtual void OnPausing()
        {

        }
        /// <summary>
        /// UI面板关闭
        /// </summary>
        internal virtual void OnClose()
        {

        }
        /// <summary>
        /// UI面板事件消息
        /// </summary>
        /// <param name="dto"></param>
        internal virtual void OnEvent(EventDto dto)
        {
        }
        /// <summary>
        /// 检查UI面板状态
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckPanelState()
        {
            while (true)
            {
                switch (Panel_State)
                {
                    case PanelState.Show:
                        OnShouwing();
                        break;
                    case PanelState.Pause:
                        OnPausing();
                        break;
                    case PanelState.Close:
                        StopCheck();
                        break;
                }
                yield return null;
            }

        }
        /// <summary>
        /// 开始检查UI面板状态
        /// </summary>
        internal void StartCheck()
        {
            if (gameObject.activeInHierarchy)
                PanelUpdate = StartCoroutine(CheckPanelState());

        }
        /// <summary>
        /// 停止检查UI面板状态
        /// </summary>
        internal void StopCheck()
        {
            StopCoroutine(PanelUpdate);
        }
        /// <summary>
        /// 关闭此UI面板
        /// </summary>
        internal void Close()
        {
            UIManager.ClosePanel(Panel_Index);
        }
    }
}

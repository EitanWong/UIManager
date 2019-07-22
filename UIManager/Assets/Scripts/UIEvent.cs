using UnityEngine;
using UIFrameWork.BasePanel;
using UIFrameWork.Data;
namespace UIFrameWork.Event
{

    public class UIEvent
    {
        private static UIEvent Instence = new UIEvent();

        public static UIEvent GetEvent
        {
            get
            {
                return Instence;
            }
        }

        internal delegate void UIPanelEvent(EventDto Dto);

        /// <summary>
        /// UI面板事件
        /// </summary>
        internal UIPanelEvent UIPanel_Event;

        /// <summary>
        /// 发送事件给UI面板
        /// </summary>
        /// <param name="SenderDto">事件数据对象</param>
        internal void SendEvent(EventDto SenderDto)
        {
            if (SenderDto.Type == UIEventType.SendTarget)
            {
                if (SenderDto.info == null)
                {
                    Debug.LogException(new System.Exception("错误！发送指定UIPanel事件消息需指定发送对象，"));
                    return;
                }
                BaseUIPanel PanelTarget = null;
                var type = SenderDto.info.GetType();
                if (type == typeof(int))
                {
                    PanelTarget = UIData.GetData.GetPanel((int)SenderDto.info);
                }
                if (type == typeof(string))
                {
                    PanelTarget = UIData.GetData.GetPanel((string)SenderDto.info);
                }

                if (PanelTarget == null)
                {
                    Debug.LogException(new System.Exception(string.Format("无法发送事件消息给{0}找不到该Panel", SenderDto.info)));
                    return;
                }
                PanelTarget.OnEvent(SenderDto);
            }
            else
            {
                UIPanel_Event(SenderDto);
            }
        }

        /// <summary>
        /// 创建并编写一个事件数据对象
        /// </summary>
        /// <param name="eventType">UI事件类型</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        internal EventDto WriteEventDto(UIEventType eventType, object data)
        {
            var dto = new EventDto();
            dto.Type = eventType;
            dto.data = data;
            return dto;
        }

        /// <summary>
        /// 创建并编写一个事件数据对象
        /// </summary>
        /// <param name="eventType">UI事件类型</param>
        /// <param name="data">数据</param>
        /// <param name="info">信息</param>
        /// <returns></returns>
        internal EventDto WriteEventDto(UIEventType eventType, object data, object info)
        {
            var dto = new EventDto();
            dto.Type = eventType;
            dto.data = data;
            dto.info = info;
            return dto;
        }
    }
    internal struct EventDto// 事件数据对象
    {
        public UIEventType Type;//UI事件类型
        public object info;//信息
        public object data;//数据
    }
    public enum UIEventType
    {
        Send2All,//发送给所有UI面板
        SendTarget,//发送指定UI面板
    }// UI事件类型
}



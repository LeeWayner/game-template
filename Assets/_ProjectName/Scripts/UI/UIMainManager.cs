using System.Collections.Generic;
using MyTool.Core.Runtime.Singleton;
using DoozyUI;
using UnityEngine;

    public class UIMainManager : Singleton<UIMainManager>
    {
        [SerializeField] private UIElement background;
        public Queue<UIElement> QueuePopup { get; set; }
        public override void Init()
        {
        }
        void Start()
        {
            QueuePopup = new Queue<UIElement>(3);
            UIManager.Instance.InitBackgroundQueue(background, transform);
        }
    }
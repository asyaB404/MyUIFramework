using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 面板接口
    /// </summary>
    public interface IBasePanel
    {
        /// <summary>
        ///     是否存在于面板栈内
        /// </summary>
        bool IsInStack { get; }

        /// <summary>
        ///     初始化方法
        /// </summary>
        void Init();

        /// <summary>
        ///     ShowMe的同时会将其存入栈内
        /// </summary>
        void ShowMe();

        /// <summary>
        ///     HideMe的同时会尝试将其弹出，注意不能调用栈顶以外面板的hideme
        /// </summary>
        void HideMe();

        /// <summary>
        ///     按下ESC键时触发
        /// </summary>
        void OnPressedEsc();

        /// <summary>
        ///     根据IsInStack来决定执行ShowMe还是HideMe
        /// </summary>
        void ChangeMe();

        void ShowAnim();

        void HideAnim();

        /// <summary>
        /// 当面板被推入UI栈顶部时的回调方法。
        /// </summary>
        /// <param name="oldPanel">之前在顶部的面板（如果存在）。</param>
        void CallBackWhenHeadPush(IBasePanel oldPanel);

        /// <summary>
        /// 当有面板从UI栈顶部弹出时的回调方法，this是新的栈顶
        /// </summary>
        /// <param name="popPanel">从顶部弹出的面板。</param>
        void CallBackWhenHeadPop(IBasePanel popPanel);
    }

    /// <summary>
    /// 面板基类
    /// </summary>
    /// <typeparam name="T1">决定单例模式Instance的类型</typeparam>
    public class BasePanel<T1> : MonoBehaviour, IBasePanel where T1 : class
    {
        [SerializeField] private bool openFindControl = true;
        private Dictionary<string, List<UIBehaviour>> _controlDic;
        private CanvasGroup _canvasGroup;
        public static T1 Instance { get; private set; }

        /// <summary>
        ///     CanvasGroup的实例对象，访问时如果没有则会自动创建
        /// </summary>
        protected CanvasGroup CanvasGroupInstance
        {
            get
            {
                _canvasGroup ??= gameObject.AddComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        /// <summary>
        /// 是否在栈中
        /// </summary>
        public bool IsInStack { get; private set; }

        /// <summary>
        /// 初始化，搜寻控件
        /// </summary>
        public virtual void Init()
        {
            //单例模式
            Instance = this as T1;
            if (!openFindControl) return;
            _controlDic = new Dictionary<string, List<UIBehaviour>>();
            FindChildrenControl<Button>();
            FindChildrenControl<Image>();
            FindChildrenControl<Text>();
            FindChildrenControl<TextMeshProUGUI>();
            FindChildrenControl<TMP_InputField>();
            FindChildrenControl<Toggle>();
            FindChildrenControl<ToggleGroup>();
            FindChildrenControl<Slider>();
            FindChildrenControl<ScrollRect>();
            FindChildrenControl<InputField>();
        }


        /// <summary>
        ///     打开当前面板，该面板进入栈内，同时标记为IsInStack = true
        /// </summary>
        public void ShowMe()
        {
            if (IsInStack) return;
            UIManager.Instance.PushPanel(this);
            gameObject.SetActive(true);
            //设置为最后一个子物体，防止被其他已经打开的面板遮挡
            gameObject.transform.SetAsLastSibling();
            IsInStack = true;
        }

        /// <summary>
        ///     关闭当前面板，IsInStack = false,一般只对栈顶的元素执行,若不是栈顶元素执行,将会弹出该元素之上的所有元素和他自己
        /// </summary>
        public void HideMe()
        {
            if (!IsInStack) return;
            if (
                ReferenceEquals(UIManager.Instance.Peek(), this)
            )
            {
                UIManager.Instance.PopPanel();
                IsInStack = false;
            }
            else
            {
                Debug.LogWarning("注意，你关闭了栈顶以外的面板");
                while (!ReferenceEquals(UIManager.Instance.Peek(), this)) UIManager.Instance.Peek().HideMe();
                UIManager.Instance.PopPanel();
                IsInStack = false;
            }
        }

        public virtual void OnPressedEsc()
        {
            HideMe();
        }

        public virtual void ChangeMe()
        {
            if (IsInStack)
                HideMe();
            else
                ShowMe();
        }

        /// <summary>
        /// 当面板被推入UI栈顶部时的回调方法。
        /// </summary>
        /// <param name="oldPanel">之前在顶部的面板（如果存在）。</param>
        public virtual void CallBackWhenHeadPush(IBasePanel oldPanel)
        {
            oldPanel?.HideAnim();
            ShowAnim();
        }

        /// <summary>
        /// 当有面板从UI栈顶部弹出时的回调方法，this是新的栈顶
        /// </summary>
        /// <param name="popPanel">从顶部弹出的面板。</param>
        public virtual void CallBackWhenHeadPop(IBasePanel popPanel)
        {
            popPanel?.HideAnim();
            if (gameObject.activeSelf == false) ShowAnim();
        }


        /// <summary>
        /// 播放面板显示动画。
        /// </summary>
        public virtual void ShowAnim()
        {
            CanvasGroupInstance.interactable = true;
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, UIConst.UIDuration);
        }

        /// <summary>
        /// 播放面板隐藏动画。
        /// </summary>
        public virtual void HideAnim()
        {
            CanvasGroupInstance.interactable = false;
            transform.DOScale(0, UIConst.UIDuration).OnComplete(() => { gameObject.SetActive(false); });
        }

        /// <summary>
        ///     所有按钮的点击事件，可以考虑在这里添加音效
        /// </summary>
        /// <param name="btnName">这个按钮的GameObject的名称</param>
        protected virtual void OnClick(string btnName)
        {
        }

        /// <summary>
        ///     所有多选框的点击事件，可以考虑在这里添加音效
        /// </summary>
        /// <param name="toggleName">这个多选框的GameObject的名称</param>
        /// <param name="value">多选框回调返回值</param>
        protected virtual void OnValueChanged(string toggleName, bool value)
        {
        }

        /// <summary>
        /// 根据在场景中GameObject的名称来寻找UI控件，如果同名则返回第一个找到的（从上往下）
        /// </summary>
        /// <param name="controlName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetControl<T>(string controlName) where T : UIBehaviour
        {
            if (!_controlDic.ContainsKey(controlName)) return null;
            for (var i = 0; i < _controlDic[controlName].Count; ++i)
                if (_controlDic[controlName][i] is T)
                    return _controlDic[controlName][i] as T;

            return null;
        }

        private void FindChildrenControl<T>() where T : UIBehaviour
        {
            var controls = GetComponentsInChildren<T>(true);
            foreach (var t in controls)
            {
                var objName = t.gameObject.name;
                if (_controlDic.TryGetValue(objName, out var value1))
                    value1.Add(t);
                else
                    _controlDic.Add(objName, new List<UIBehaviour> { t });

                switch (t)
                {
                    case Button button:
                        button?.onClick.AddListener(() => { OnClick(objName); });
                        break;
                    case Toggle toggle:
                        toggle?.onValueChanged.AddListener(value => { OnValueChanged(objName, value); });
                        break;
                }
            }
        }
    }
}
using TMPro;
using UI;
using UnityEngine;


namespace Test
{
    /// <summary>
    /// 测试用，能够更清楚看到面板栈内的元素
    /// </summary>
    public class DebugStackView : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject panelInfoPrefab;
        private int _prevCount = -1;

        private void Update()
        {
            if (_prevCount != UIManager.Instance.Panels.Count)
            {
                content.DestroyAllChildren();
                foreach (var itemPanel in UIManager.Instance.Panels)
                {
                    GameObject o = Instantiate(panelInfoPrefab, content, false);
                    o.GetComponentInChildren<TextMeshProUGUI>().text = (itemPanel as MonoBehaviour)?.name;
                }

                _prevCount = UIManager.Instance.Panels.Count;
            }
        }
    }
}
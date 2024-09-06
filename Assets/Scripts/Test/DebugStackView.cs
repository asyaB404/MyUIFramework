using TMPro;
using UI;
using UnityEngine;


namespace Test
{
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
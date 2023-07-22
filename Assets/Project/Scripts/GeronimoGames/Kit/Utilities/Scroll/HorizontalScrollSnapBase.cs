using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Utilities
{
    public class HorizontalScrollSnapBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IHorizontalScrollSnap
    {
        internal Rect PanelDimensions;
        internal RectTransform ScreensContainer;
        internal bool IsVertical;
        internal int Screens = 1;

        internal float ScrollStartPosition;
        internal float ChildSize;
        private float childPos;
        private float maskSize;
        internal Vector2 ChildAnchorPoint;
        internal ScrollRect ScrollRect;
        internal Vector3 LerpTarget;
        internal bool Lerp;
        internal bool PointerDown = false;
        internal bool Settled = true;
        internal Vector3 StartPosition = new Vector3();

        [Tooltip("The currently active page")]
        internal int _currentPage;

        internal int PreviousPage;
        internal int HalfNoVisibleItems;
        internal bool MoveStarted;
        private int _bottomItem;
        private int _topItem;

        [Serializable]
        public class SelectionChangeStartEvent : UnityEvent
        {
        }

        [Serializable]
        public class SelectionPageChangedEvent : UnityEvent<int>
        {
        }

        [Serializable]
        public class SelectionChangeEndEvent : UnityEvent<int>
        {
        }

        [Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
        [SerializeField]
        public int StartingScreen = 0;

        [Tooltip("The distance between two pages based on page height, by default pages are next to each other")]
        [SerializeField]
        [Range(0, 8)]
        public float PageStep = 1;

        [Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
        public GameObject Pagination;

        [Tooltip("Button to go to the previous page. (optional)")]
        public GameObject PrevButton;

        [Tooltip("Button to go to the next page. (optional)")]
        public GameObject NextButton;

        [Tooltip("Transition speed between pages. (optional)")]
        public float TransitionSpeed = 7.5f;

        [Tooltip("Fast Swipe makes swiping page next / previous (optional)")]
        public bool UseFastSwipe = false;

        [Tooltip("Offset for how far a swipe has to travel to initiate a page change (optional)")]
        public int FastSwipeThreshold = 100;

        [Tooltip("Speed at which the ScrollRect will keep scrolling before slowing down and stopping (optional)")]
        public int SwipeVelocityThreshold = 100;

        [Tooltip("The visible bounds area, controls which items are visible/enabled. *Note Should use a RectMask. (optional)")]
        public RectTransform MaskArea;

        [Tooltip("Pixel size to buffer arround Mask Area. (optional)")]
        public float MaskBuffer = 1;

        public int CurrentPage
        {
            get { return _currentPage; }

            internal set
            {
                if ((value != _currentPage && value >= 0 && value < ScreensContainer.childCount) || (value == 0 && ScreensContainer.childCount == 0))
                {
                    PreviousPage = _currentPage;
                    _currentPage = value;
                    if (MaskArea) UpdateVisible();
                    if (!Lerp) ScreenChange();
                    OnCurrentScreenChange(_currentPage);
                }
            }
        }

        [Tooltip("By default the container will lerp to the start when enabled in the scene, this option overrides this and forces it to simply jump without lerping")]
        public bool JumpOnEnable = false;

        [Tooltip("By default the container will return to the original starting page when enabled, this option overrides this behaviour and stays on the current selection")]
        public bool RestartOnEnable = false;

        [Tooltip("(Experimental)\nBy default, child array objects will use the parent transform\nHowever you can disable this for some interesting effects")]
        public bool UseParentTransform = true;

        [Tooltip("Scroll Snap children. (optional)\nEither place objects in the scene as children OR\nPrefabs in this array, NOT BOTH")]
        public GameObject[] ChildObjects;

        [SerializeField]
        [Tooltip("Event fires when a user starts to change the selection")]
        private SelectionChangeStartEvent _onSelectionChangeStartEvent = new SelectionChangeStartEvent();

        public SelectionChangeStartEvent OnSelectionChangeStartEvent
        {
            get { return _onSelectionChangeStartEvent; }
            set { _onSelectionChangeStartEvent = value; }
        }

        [SerializeField]
        [Tooltip("Event fires as the page changes, while dragging or jumping")]
        private SelectionPageChangedEvent _onSelectionPageChangedEvent = new SelectionPageChangedEvent();

        public SelectionPageChangedEvent OnSelectionPageChangedEvent
        {
            get { return _onSelectionPageChangedEvent; }
            set { _onSelectionPageChangedEvent = value; }
        }

        [SerializeField]
        [Tooltip("Event fires when the page settles after a user has dragged")]
        private SelectionChangeEndEvent _onSelectionChangeEndEvent = new SelectionChangeEndEvent();

        public SelectionChangeEndEvent OnSelectionChangeEndEvent
        {
            get { return _onSelectionChangeEndEvent; }
            set { _onSelectionChangeEndEvent = value; }
        }

        private void Awake()
        {
            if (ScrollRect == null)
            {
                ScrollRect = gameObject.GetComponent<ScrollRect>();
            }

            PanelDimensions = gameObject.GetComponent<RectTransform>().rect;

            if (StartingScreen < 0)
            {
                StartingScreen = 0;
            }

            ScreensContainer = ScrollRect.content;

            InitialiseChildObjects();

            if (NextButton)
            {
                NextButton.GetComponent<Button>().onClick.AddListener(NextScreen);
            }

            if (PrevButton)
            {
                PrevButton.GetComponent<Button>().onClick.AddListener(PreviousScreen);
            }
        }

        internal void InitialiseChildObjects()
        {
            if (ChildObjects != null && ChildObjects.Length > 0)
            {
                if (ScreensContainer.transform.childCount > 0)
                {
                    Debug.LogError(
                        "ScrollRect Content has children, this is not supported when using managed Child Objects\n Either remove the ScrollRect Content children or clear the ChildObjects array");
                    return;
                }

                InitialiseChildObjectsFromArray();
            }
            else
            {
                InitialiseChildObjectsFromScene();
            }
        }

        internal void InitialiseChildObjectsFromScene()
        {
            var childCount = ScreensContainer.childCount;
            ChildObjects = new GameObject[childCount];
            for (var i = 0; i < childCount; i++)
            {
                ChildObjects[i] = ScreensContainer.transform.GetChild(i).gameObject;
                if (MaskArea && ChildObjects[i].activeSelf)
                {
                    ChildObjects[i].SetActive(false);
                }
            }
        }

        internal void InitialiseChildObjectsFromArray()
        {
            var childCount = ChildObjects.Length;
            for (var i = 0; i < childCount; i++)
            {
                var child = GameObject.Instantiate(ChildObjects[i]);
                if (UseParentTransform)
                {
                    var childRect = child.GetComponent<RectTransform>();
                    childRect.rotation = ScreensContainer.rotation;
                    childRect.localScale = ScreensContainer.localScale;
                    childRect.position = ScreensContainer.position;
                }

                child.transform.SetParent(ScreensContainer.transform);
                ChildObjects[i] = child;
                if (MaskArea && ChildObjects[i].activeSelf)
                {
                    ChildObjects[i].SetActive(false);
                }
            }
        }

        internal void UpdateVisible()
        {
            if (!MaskArea || ChildObjects == null || ChildObjects.Length < 1 || ScreensContainer.childCount < 1)
            {
                return;
            }

            maskSize = IsVertical ? MaskArea.rect.height : MaskArea.rect.width;
            HalfNoVisibleItems = (int)Math.Round(maskSize / (ChildSize * MaskBuffer), MidpointRounding.AwayFromZero) / 2;
            _bottomItem = _topItem = 0;

            for (var i = HalfNoVisibleItems + 1; i > 0; i--)
            {
                _bottomItem = _currentPage - i < 0 ? 0 : i;
                if (_bottomItem > 0) break;
            }

            for (var i = HalfNoVisibleItems + 1; i > 0; i--)
            {
                _topItem = ScreensContainer.childCount - _currentPage - i < 0 ? 0 : i;
                if (_topItem > 0) break;
            }

            for (var i = CurrentPage - _bottomItem; i < CurrentPage + _topItem; i++)
            {
                try
                {
                    ChildObjects[i].SetActive(true);
                }
                catch
                {
                    Debug.Log("Failed to setactive child [" + i + "]");
                }
            }

            if (_currentPage > HalfNoVisibleItems) ChildObjects[CurrentPage - _bottomItem].SetActive(false);
            if (ScreensContainer.childCount - _currentPage > _topItem) ChildObjects[CurrentPage + _topItem].SetActive(false);
        }

        public void NextScreen()
        {
            if (_currentPage < Screens - 1)
            {
                if (!Lerp) StartScreenChange();

                Lerp = true;
                CurrentPage = _currentPage + 1;
                GetPositionforPage(_currentPage, ref LerpTarget);
                ScreenChange();
            }
        }

        public void PreviousScreen()
        {
            if (_currentPage > 0)
            {
                if (!Lerp) StartScreenChange();

                Lerp = true;
                CurrentPage = _currentPage - 1;
                GetPositionforPage(_currentPage, ref LerpTarget);
                ScreenChange();
            }
        }

        public void GoToScreen(int screenIndex)
        {
            if (screenIndex <= Screens - 1 && screenIndex >= 0)
            {
                if (!Lerp) StartScreenChange();

                Lerp = true;
                CurrentPage = screenIndex;
                GetPositionforPage(_currentPage, ref LerpTarget);
                ScreenChange();
            }
        }

        internal int GetPageforPosition(Vector3 pos)
        {
            return IsVertical ? (int)Math.Round((ScrollStartPosition - pos.y) / ChildSize) : (int)Math.Round((ScrollStartPosition - pos.x) / ChildSize);
        }

        internal bool IsRectSettledOnaPage(Vector3 pos)
        {
            return IsVertical
                ? -((pos.y - ScrollStartPosition) / ChildSize) == -(int)Math.Round((pos.y - ScrollStartPosition) / ChildSize)
                : -((pos.x - ScrollStartPosition) / ChildSize) == -(int)Math.Round((pos.x - ScrollStartPosition) / ChildSize);
        }

        internal void GetPositionforPage(int page, ref Vector3 target)
        {
            childPos = -ChildSize * page;
            if (IsVertical)
            {
                target.y = childPos + ScrollStartPosition;
            }
            else
            {
                target.x = childPos + ScrollStartPosition;
            }
        }

        internal void ScrollToClosestElement()
        {
            Lerp = true;
            CurrentPage = GetPageforPosition(ScreensContainer.localPosition);
            GetPositionforPage(_currentPage, ref LerpTarget);
            OnCurrentScreenChange(_currentPage);
        }

        internal void OnCurrentScreenChange(int currentScreen)
        {
            ChangeBulletsInfo(currentScreen);
            ToggleNavigationButtons(currentScreen);
        }

        private void ChangeBulletsInfo(int targetScreen)
        {
            if (Pagination)
            {
                for (var i = 0; i < Pagination.transform.childCount; i++)
                {
                    Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (targetScreen == i) ? true : false;
                }
            }
        }

        private void ToggleNavigationButtons(int targetScreen)
        {
            if (PrevButton)
            {
                PrevButton.GetComponent<Button>().interactable = targetScreen > 0;
            }

            if (NextButton)
            {
                NextButton.GetComponent<Button>().interactable = targetScreen < ScreensContainer.transform.childCount - 1;
            }
        }

        private void OnValidate()
        {
            if (ScrollRect == null)
            {
                ScrollRect = GetComponent<ScrollRect>();
            }

            if (!ScrollRect.horizontal && !ScrollRect.vertical)
            {
                Debug.LogError("ScrollRect has to have a direction, please select either Horizontal OR Vertical with the appropriate control.");
            }

            if (ScrollRect.horizontal && ScrollRect.vertical)
            {
                Debug.LogError("ScrollRect has to be unidirectional, only use either Horizontal or Vertical on the ScrollRect, NOT both.");
            }

            var children = gameObject.GetComponent<ScrollRect>().content.childCount;
            if (children != 0 || ChildObjects != null)
            {
                var childCount = ChildObjects == null || ChildObjects.Length == 0 ? children : ChildObjects.Length;
                if (StartingScreen > childCount - 1)
                {
                    StartingScreen = childCount - 1;
                }

                if (StartingScreen < 0)
                {
                    StartingScreen = 0;
                }
            }

            if (MaskBuffer <= 0)
            {
                MaskBuffer = 1;
            }

            if (PageStep < 0)
            {
                PageStep = 0;
            }

            if (PageStep > 8)
            {
                PageStep = 9;
            }
        }

        public void StartScreenChange()
        {
            if (!MoveStarted)
            {
                MoveStarted = true;
                OnSelectionChangeStartEvent.Invoke();
            }
        }

        internal void ScreenChange()
        {
            OnSelectionPageChangedEvent.Invoke(_currentPage);
        }

        internal void EndScreenChange()
        {
            OnSelectionChangeEndEvent.Invoke(_currentPage);
            Settled = true;
            MoveStarted = false;
        }

        public Transform CurrentPageObject()
        {
            return ScreensContainer.GetChild(CurrentPage);
        }

        public void CurrentPageObject(out Transform returnObject)
        {
            returnObject = ScreensContainer.GetChild(CurrentPage);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PointerDown = true;
            Settled = false;
            StartScreenChange();
            StartPosition = ScreensContainer.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Lerp = false;
        }

        int IHorizontalScrollSnap.CurrentPage()
        {
            return CurrentPage = GetPageforPosition(ScreensContainer.localPosition);
        }

        public void SetLerp(bool value)
        {
            Lerp = value;
        }

        public void ChangePage(int page)
        {
            GoToScreen(page);
        }
    }
}
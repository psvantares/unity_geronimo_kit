using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Utilities
{
    [RequireComponent(typeof(ScrollRect))]
    public class HorizontalScrollSnap : HorizontalScrollSnapBase, IEndDragHandler
    {
        private void Start()
        {
            IsVertical = false;
            ChildAnchorPoint = new Vector2(0, 0.5f);

            _currentPage = StartingScreen;

            PanelDimensions = gameObject.GetComponent<RectTransform>().rect;

            UpdateLayout();
        }

        private void Update()
        {
            if (!Lerp && ScrollRect.velocity == Vector2.zero)
            {
                if (!Settled && !PointerDown)
                {
                    if (!IsRectSettledOnaPage(ScreensContainer.localPosition))
                    {
                        ScrollToClosestElement();
                    }
                }

                return;
            }
            else if (Lerp)
            {
                ScreensContainer.localPosition = Vector3.Lerp(ScreensContainer.localPosition, LerpTarget, TransitionSpeed * Time.deltaTime);
                if (Vector3.Distance(ScreensContainer.localPosition, LerpTarget) < 0.1f)
                {
                    ScreensContainer.localPosition = LerpTarget;
                    Lerp = false;
                    EndScreenChange();
                }
            }

            CurrentPage = GetPageforPosition(ScreensContainer.localPosition);

            if (!PointerDown)
            {
                if (ScrollRect.velocity.x > 0.01 || ScrollRect.velocity.x < 0.01)
                {
                    if (IsRectMovingSlowerThanThreshold(0))
                    {
                        ScrollToClosestElement();
                    }
                }
            }
        }

        private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
        {
            return (ScrollRect.velocity.x > startingSpeed && ScrollRect.velocity.x < SwipeVelocityThreshold) ||
                   (ScrollRect.velocity.x < startingSpeed && ScrollRect.velocity.x > -SwipeVelocityThreshold);
        }

        private void DistributePages()
        {
            Screens = ScreensContainer.childCount;
            ScrollRect.horizontalNormalizedPosition = 0;

            const int offset = 0;
            float currentXPosition = 0;
            var pageStepValue = ChildSize = (int)PanelDimensions.width * ((PageStep == 0) ? 3 : PageStep);

            for (var i = 0; i < ScreensContainer.transform.childCount; i++)
            {
                var child = ScreensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
                currentXPosition = offset + (int)(i * pageStepValue);
                child.sizeDelta = new Vector2(PanelDimensions.width, PanelDimensions.height);
                child.anchoredPosition = new Vector2(currentXPosition, 0f);
                child.anchorMin = child.anchorMax = child.pivot = ChildAnchorPoint;
            }

            var dimension = currentXPosition + offset * -1;

            ScreensContainer.offsetMax = new Vector2(dimension, 0f);
        }

        [UsedImplicitly]
        public void AddChild(GameObject go)
        {
            AddChild(go, false);
        }

        [UsedImplicitly]
        public void AddChild(GameObject go, bool worldPositionStays)
        {
            ScrollRect.horizontalNormalizedPosition = 0;
            go.transform.SetParent(ScreensContainer, worldPositionStays);
            InitialiseChildObjectsFromScene();
            DistributePages();

            if (MaskArea)
            {
                UpdateVisible();
            }

            SetScrollContainerPosition();
        }

        [UsedImplicitly]
        public void RemoveChild(int index, out GameObject childRemoved)
        {
            RemoveChild(index, false, out childRemoved);
        }

        [UsedImplicitly]
        public void RemoveChild(int index, bool worldPositionStays, out GameObject childRemoved)
        {
            childRemoved = null;
            if (index < 0 || index > ScreensContainer.childCount)
            {
                return;
            }

            ScrollRect.horizontalNormalizedPosition = 0;

            var child = ScreensContainer.transform.GetChild(index);
            child.SetParent(null, worldPositionStays);
            childRemoved = child.gameObject;
            InitialiseChildObjectsFromScene();
            DistributePages();

            if (MaskArea)
            {
                UpdateVisible();
            }

            if (_currentPage > Screens - 1)
            {
                CurrentPage = Screens - 1;
            }

            SetScrollContainerPosition();
        }

        [UsedImplicitly]
        public void RemoveAllChildren(out GameObject[] childrenRemoved)
        {
            RemoveAllChildren(false, out childrenRemoved);
        }

        [UsedImplicitly]
        public void RemoveAllChildren(bool worldPositionStays, out GameObject[] childrenRemoved)
        {
            var screenCount = ScreensContainer.childCount;
            childrenRemoved = new GameObject[screenCount];

            for (int i = screenCount - 1; i >= 0; i--)
            {
                childrenRemoved[i] = ScreensContainer.GetChild(i).gameObject;
                childrenRemoved[i].transform.SetParent(null, worldPositionStays);
            }

            ScrollRect.horizontalNormalizedPosition = 0;
            CurrentPage = 0;
            InitialiseChildObjectsFromScene();
            DistributePages();

            if (MaskArea)
            {
                UpdateVisible();
            }
        }

        private void SetScrollContainerPosition()
        {
            ScrollStartPosition = ScreensContainer.localPosition.x;
            ScrollRect.horizontalNormalizedPosition = (float)(_currentPage) / (Screens - 1);
            OnCurrentScreenChange(_currentPage);
        }

        [UsedImplicitly]
        public void UpdateLayout()
        {
            Lerp = false;
            DistributePages();

            if (MaskArea)
            {
                UpdateVisible();
            }

            SetScrollContainerPosition();
            OnCurrentScreenChange(_currentPage);
        }

        private void OnRectTransformDimensionsChange()
        {
            if (ChildAnchorPoint != Vector2.zero)
            {
                UpdateLayout();
            }
        }

        private void OnEnable()
        {
            InitialiseChildObjectsFromScene();
            DistributePages();

            if (MaskArea)
            {
                UpdateVisible();
            }

            if (JumpOnEnable || !RestartOnEnable)
            {
                SetScrollContainerPosition();
            }

            if (RestartOnEnable)
            {
                GoToScreen(StartingScreen);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerDown = false;

            if (ScrollRect.horizontal)
            {
                var distance = Vector3.Distance(StartPosition, ScreensContainer.localPosition);
                if (UseFastSwipe && distance < PanelDimensions.width && distance >= FastSwipeThreshold)
                {
                    ScrollRect.velocity = Vector3.zero;
                    if (StartPosition.x - ScreensContainer.localPosition.x > 0)
                    {
                        NextScreen();
                    }
                    else
                    {
                        PreviousScreen();
                    }
                }
            }
        }
    }
}
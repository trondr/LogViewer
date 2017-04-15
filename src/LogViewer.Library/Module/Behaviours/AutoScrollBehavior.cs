//Source: http://stackoverflow.com/questions/25800470/wpf-listbox-automatic-scrolling-start-and-stop-behavior

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace LogViewer.Library.Module.Behaviours
{
    // List box automatically scrolls to show new items as they are added asynchronously.
    // A user interaction with the listbox stops automatic scrolling - AKA obnoxious behavior.
    // Once finished interacting, to continue automatic scrolling, drag the scroll bar to 
    // the bottom and let go, or use the mouse wheel or keyboard to do the same. 
    // This indicates that the user wants automatic scrolling to resume.

    public class AutoScrollBehavior : Behavior<ListView>
    {
        private ScrollViewer _scrollViewer;
        private bool _autoScroll = true;
        private bool _justWheeled;
        private bool _userInteracting;
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }

        private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }
            AssociatedObject.SelectionChanged -= AssociatedObjectOnSelectionChanged;
            AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorItemsChanged;
            AssociatedObject.GotMouseCapture -= AssociatedObject_GotMouseCapture;
            AssociatedObject.LostMouseCapture -= AssociatedObject_LostMouseCapture;
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;

            _scrollViewer = null;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _scrollViewer = GetScrollViewer(AssociatedObject);
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;

                AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
                AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
                AssociatedObject.GotMouseCapture += AssociatedObject_GotMouseCapture;
                AssociatedObject.LostMouseCapture += AssociatedObject_LostMouseCapture;
                AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
            }
        }

        private static ScrollViewer GetScrollViewer(DependencyObject root)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(root);
            //for (var i = 0; i < childCount; ++i)
            for (var i = 0; i < childCount; )
            {
                var child = VisualTreeHelper.GetChild(root, i);
                var scrollViewer = child as ScrollViewer;
                if (scrollViewer != null)
                    return scrollViewer;

                return GetScrollViewer(child);
            }
            return null;
        }

        private void AssociatedObject_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // User is actively interacting with listbox. Do not allow automatic scrolling to interfere with user experience.
            _userInteracting = true;
            _autoScroll = false;
        }

        private void AssociatedObject_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // User is done interacting with control.
            _userInteracting = false;
        }

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // diff is exactly zero if the last item in the list is visible. This can occur because of scroll-bar drag, mouse-wheel, or keyboard event.
            var diff = (_scrollViewer.VerticalOffset - (_scrollViewer.ExtentHeight - _scrollViewer.ViewportHeight));

            // User just wheeled; this event is called immediately afterwards.
            if (_justWheeled && Math.Abs(diff) > 0.0001)
            {
                _justWheeled = false;
                _autoScroll = false;
                return;
            }

            if (Math.Abs(diff) < 0.0001)
            {
                // then assume user has finished with interaction and has indicated through this action that scrolling should continue automatically.
                _autoScroll = true;
            }
        }

        private void ItemContainerGeneratorItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Reset)
            {
                // An item was added to the listbox, or listbox was cleared.
                if (_autoScroll && !_userInteracting)
                {
                    // If automatic scrolling is turned on, scroll to the bottom to bring new item into view.
                    // Do not do this if the user is actively interacting with the listbox.
                    _scrollViewer.ScrollToBottom();
                }
            }
        }

        private void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            // User selected (clicked) an item, or used the keyboard to select a different item. 
            // Turn off automatic scrolling.
            _autoScroll = false;
        }

        private void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            // User wheeled the mouse. 
            // Cannot detect whether scroll viewer right at the bottom, because the scroll event has not occurred at this point.
            // Same for bubbling event.
            // Just indicated that the user mouse-wheeled, and that the scroll viewer should decide whether or not to stop autoscrolling.
            _justWheeled = true;
        }
    }
}
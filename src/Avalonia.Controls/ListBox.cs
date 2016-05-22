// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Controls
{
    /// <summary>
    /// An <see cref="ItemsControl"/> in which individual items can be selected.
    /// </summary>
    public class ListBox : SelectingItemsControl
    {
        /// <summary>
        /// The default value for the <see cref="ItemsControl.ItemsPanel"/> property.
        /// </summary>
        private static readonly FuncTemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new VirtualizingStackPanel());

        /// <summary>
        /// Defines the <see cref="SelectedItems"/> property.
        /// </summary>
        public static readonly new AvaloniaProperty<IList> SelectedItemsProperty =
            SelectingItemsControl.SelectedItemsProperty;

        /// <summary>
        /// Defines the <see cref="SelectionMode"/> property.
        /// </summary>
        public static readonly new AvaloniaProperty<SelectionMode> SelectionModeProperty = 
            SelectingItemsControl.SelectionModeProperty;

        /// <summary>
        /// Initializes static members of the <see cref="ItemsControl"/> class.
        /// </summary>
        static ListBox()
        {
            ItemsPanelProperty.OverrideDefaultValue<ListBox>(DefaultPanel);
        }

        /// <inheritdoc/>
        public new IList SelectedItems => base.SelectedItems;

        /// <inheritdoc/>
        public new SelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        /// <inheritdoc/>
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<ListBoxItem>(
                this, 
                ListBoxItem.ContentProperty,
                ListBoxItem.ContentTemplateProperty);
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            if (e.NavigationMethod == NavigationMethod.Directional)
            {
                e.Handled = UpdateSelectionFromEventSource(
                    e.Source,
                    true,
                    (e.InputModifiers & InputModifiers.Shift) != 0);
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.MouseButton == MouseButton.Left || e.MouseButton == MouseButton.Right)
            {
                e.Handled = UpdateSelectionFromEventSource(
                    e.Source,
                    true,
                    (e.InputModifiers & InputModifiers.Shift) != 0,
                    (e.InputModifiers & InputModifiers.Control) != 0);
            }
        }
    }
}

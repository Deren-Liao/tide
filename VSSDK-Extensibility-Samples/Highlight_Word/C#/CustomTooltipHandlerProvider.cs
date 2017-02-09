﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Adornments;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Formatting;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Linq;

namespace HighlightWord
{
    [Export(typeof(IMouseProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("xaml")]
    [Name("MyCustomTooltip")]
    internal sealed class CustomTooltipHandlerProvider : IMouseProcessorProvider
    {
        [Import]
        public IToolTipProviderFactory ToolTipProviderFactory { get; set; }

        [Import]
        public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        internal IClassifierAggregatorService AggregatorService { get; set; }

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            return new CustomTooltipMouseProcessor(view, this.ToolTipProviderFactory, 
                this.NavigatorService.GetTextStructureNavigator(view.TextBuffer), 
                this.AggregatorService.GetClassifier(view.TextBuffer));
        }
    }

    internal class CustomTooltipMouseProcessor : MouseProcessorBase
    {
        private readonly IWpfTextView _view;

        private readonly IToolTipProvider _toolTipProvider;
        private readonly ITextStructureNavigator _navigatorService;
        private readonly IClassifier _classifier;

        private readonly ICollection<FontFamily> _systemFonts;

        private bool IsToolTipShown { get; set; }

        internal CustomTooltipMouseProcessor(IWpfTextView view, IToolTipProviderFactory
            toolTipProviderFactory, ITextStructureNavigator navigatorService, IClassifier classifier)
        {
            this._view = view;
            this._toolTipProvider = toolTipProviderFactory.GetToolTipProvider(this._view);
            this._navigatorService = navigatorService;
            this._classifier = classifier;

            this._systemFonts = Fonts.SystemFontFamilies;

            this.IsToolTipShown = false;
        }

        public override void PreprocessMouseMove(MouseEventArgs e)
        {
            var colorConverter = new ColorConverter();

            foreach (ITextViewLine viewLine in this._view.TextViewLines)
            {

                // Get the span for each line
                var lineSpan = new SnapshotSpan(viewLine.Start, viewLine.End);


                // Get the classification for each span
                var spans = this._classifier.GetClassificationSpans(lineSpan);
                foreach (var span in spans)
                {

                    // If we are on a XAML attribute
                    if (span.ClassificationType.Classification == "XAML Attribute Value")
                    {

                        // Get the attribute value
                        var xamlAttributeValue = span.Span.GetText().Replace("\"", "");


                        // Check if the attribute's value is a valid color
                        if (colorConverter.IsValid(xamlAttributeValue))
                        {
                            #region Tooltip management for FontFamily

                            var convertedColor = colorConverter.ConvertFromInvariantString(xamlAttributeValue);
                            if (convertedColor != null)
                            {
                                var xamlColor = (Color)convertedColor;

                                SnapshotSpan? spanAtMousePosition = 
                                    SpanHelpers.GetSpanAtMousePosition(this._view, this._navigatorService);
                                if (spanAtMousePosition.HasValue)
                                {
                                    var textAtMousePosition = spanAtMousePosition.Value.GetText();

                                    if (!string.IsNullOrWhiteSpace(textAtMousePosition))
                                    {
                                        if (xamlAttributeValue.Contains(textAtMousePosition))
                                        {
                                            e.Handled = true;

                                            if (!this.IsToolTipShown)
                                            {
                                                this.IsToolTipShown = true;

                                                var copyHexColorHyperlink = new Hyperlink(new Run("Copy"));
                                                copyHexColorHyperlink.Click += (sender, args) =>
                                                {
                                                    Clipboard.SetText(xamlColor.ToString());

                                                    this._toolTipProvider.ClearToolTip();
                                                };

                                                this._toolTipProvider.ClearToolTip();
                                                this._toolTipProvider.ShowToolTip(spanAtMousePosition.Value.Snapshot.CreateTrackingSpan(spanAtMousePosition.Value.Span, SpanTrackingMode.EdgeExclusive),
                                                    new Border
                                                    {
                                                        Background = new SolidColorBrush(Colors.LightGray),
                                                        Padding = new Thickness(10),
                                                        Child = new StackPanel
                                                        {
                                                            Orientation = Orientation.Horizontal,
                                                            Children =
                                                            {
                                                        new Rectangle
                                                        {
                                                            Height = 30,
                                                            Width = 30,
                                                            Fill = new SolidColorBrush(xamlColor)
                                                        },
                                                        new TextBlock
                                                        {
                                                            Margin = new Thickness(10, 0, 0, 0),
                                                            Inlines =
                                                            {
                                                                
//new Run(xamlAttributeValue),
                                                                
//new LineBreak(),
                                                                new Run(xamlColor.ToString()),
                                                                new Run(" ("),
                                                                copyHexColorHyperlink,
                                                                new Run(")"),
                                                                new LineBreak(),
                                                                new Run(string.Format("{0}", xamlColor.ToString())),
                                                            }
                                                        }
                                                            }
                                                        }
                                                    }, PopupStyles.PositionClosest);
                                            }

                                            return;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        // Check if the attribute's value is a valid color
                        else if (this._systemFonts.Any(ff => ff.Source == xamlAttributeValue))
                        {
                            #region Tooltip management for FontFamily

                            var xamlFontFamily = this._systemFonts.First(ff => ff.Source == xamlAttributeValue);

                            var spanAtMousePosition = SpanHelpers.GetSpanAtMousePosition(this._view, this._navigatorService);
                            if (spanAtMousePosition.HasValue)
                            {
                                var textAtMousePosition = spanAtMousePosition.Value.GetText();
                                if (!string.IsNullOrWhiteSpace(textAtMousePosition))
                                {

                                    // FontFamily can have whitespace in their name.
                                    if (xamlAttributeValue.Contains(textAtMousePosition))
                                    {
                                        e.Handled = true;

                                        if (!this.IsToolTipShown)
                                        {
                                            this.IsToolTipShown = true;

                                            this._toolTipProvider.ClearToolTip();
                                            this._toolTipProvider.ShowToolTip(
                                                spanAtMousePosition.Value.Snapshot.CreateTrackingSpan(spanAtMousePosition.Value.Span, SpanTrackingMode.EdgeExclusive),
                                                new Border
                                                {
                                                    Background = new SolidColorBrush(Colors.LightGray),
                                                    Padding = new Thickness(10),
                                                    Child = new StackPanel
                                                    {
                                                        Orientation = Orientation.Vertical,
                                                        Children =
                                                        {
                                                    new TextBlock
                                                    {
                                                        Inlines =
                                                        {
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 10), FontSize = 10, FontFamily = xamlFontFamily },
                                                            new LineBreak(),
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 12), FontSize = 12, FontFamily = xamlFontFamily },
                                                            new LineBreak(),
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 15), FontSize = 15, FontFamily = xamlFontFamily },
                                                            new LineBreak(),
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 18), FontSize = 18, FontFamily = xamlFontFamily },
                                                            new LineBreak(),
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 20), FontSize = 20, FontFamily = xamlFontFamily },
                                                            new LineBreak(),
                                                            new Run{ Text = string.Format("{0} ({1})", xamlFontFamily.FamilyNames[this._view.VisualElement.Language], 25), FontSize = 25, FontFamily = xamlFontFamily },
                                                        }
                                                    }
                                                        }
                                                    }
                                                }, PopupStyles.PositionClosest);
                                        }

                                        return;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }

            this._toolTipProvider.ClearToolTip();
            IsToolTipShown = false;
        }
    }
}

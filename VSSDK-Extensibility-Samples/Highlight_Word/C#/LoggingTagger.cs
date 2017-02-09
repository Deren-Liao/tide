﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace HighlightWord
{
    internal class LoggingTagger33 : ITagger<LoggingTag>
    {
        private readonly ITextView _view;
        private ITextSearchService _textSearchService;
        private ITextStructureNavigator _textStructureNavigator;
        private readonly ITextBuffer _sourceBuffer;
        private ITextViewLine _currentViewLine;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public LoggingTagger33(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService,
                                   ITextStructureNavigator textStructureNavigator)
        {
            _sourceBuffer = sourceBuffer;
            _view = view;
            _textSearchService = textSearchService;
            _textStructureNavigator = textStructureNavigator;
            // Subscribe to both change events in the view - any time the view is updated
            // or the caret is moved, we refresh our list of highlighted words.
            _view.Caret.PositionChanged += CaretPositionChanged;
            _view.LayoutChanged += ViewLayoutChanged;
        }

        /// <summary>
        /// Force an update if the view layout changes
        /// </summary>
        private void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // If a new snapshot wasn't generated, then skip this layout
            if (e.NewViewState.EditSnapshot != e.OldViewState.EditSnapshot)
            {
                UpdateAtCaretPosition(_view.Caret.ContainingTextViewLine);
            }
        }

        /// <summary>
        /// Force an update if the caret position changes
        /// </summary>
        private void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateAtCaretPosition(_view.Caret.ContainingTextViewLine);
        }

        /// <summary>
        /// Check the caret position. If the caret is on a new word, update the CurrentWord value
        /// </summary>
        private void UpdateAtCaretPosition(ITextViewLine currentViewLine)
        {
            _currentViewLine = currentViewLine;
            var tempEvent = TagsChanged;
            if (tempEvent != null)
                tempEvent(this, new SnapshotSpanEventArgs(
                    new SnapshotSpan(_sourceBuffer.CurrentSnapshot, 0, _sourceBuffer.CurrentSnapshot.Length)));
        }


        public IEnumerable<ITagSpan<LoggingTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {


            if (spans.Count == 0)
                yield break;

            if (_currentViewLine == null)
            {
                yield break;
            }

            if (!_currentViewLine.IsValid)
            {
                yield break;
            }

            if (spans.Count > 0)
            {
                // look for 'StackdriverLogging' occurrences
                foreach (SnapshotSpan span in _textSearchService.FindAll(new FindData("StackdriverLogging",
                    spans[0].Snapshot, 
                    FindOptions.WholeWord | FindOptions.MatchCase | FindOptions.SingleLine, 
                    _textStructureNavigator)))
                //spans[0].Snapshot, FindOptions.WholeWord | FindOptions.MatchCase, _textStructureNavigator)))
                {
                    if (_currentViewLine.ContainsBufferPosition(span.Start))
                    {
                        yield return new TagSpan<LoggingTag>(span, new LoggingTag());
                    }
                }
            }
        }
    }


    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(TextMarkerTag))]
    [ContentType("text")] // only for code portion. Could be changed to csharp to colorize only C# code for example
    internal class GotoTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView.TextBuffer != buffer)
                return null;

            return new LoggingTagger33(textView, buffer, TextSearchService, 
                TextStructureNavigatorSelector.GetTextStructureNavigator(buffer)) as ITagger<T>;
        }
    }

    internal class LoggingTag : TextMarkerTag
    {
        public LoggingTag() : base("Logging") { }
    }

    [Export(typeof(EditorFormatDefinition))]
    [Name("Logging")]
    [UserVisible(true)]
    internal class GotoFormatDefinition : MarkerFormatDefinition
    {
        public GotoFormatDefinition()
        {
            BackgroundColor = Colors.Red;
            ForegroundColor = Colors.White;
            DisplayName = "Logging Word";
            ZOrder = 5;
        }
    }
}
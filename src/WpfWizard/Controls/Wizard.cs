﻿namespace WpfWizard.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using WpfWizard.Classes;
    using WpfWizard.Interfaces;

    /// <summary>
    /// The wizard control.
    /// </summary>
    public partial class Wizard : Selector, INotifyPropertyChanged
    {
        // ReSharper disable StyleCop.SA1600
        public static readonly DependencyProperty SharedContextProperty = DependencyProperty.Register(nameof(SharedContext), typeof(Dictionary<string, object>), typeof(Wizard));
        public static readonly DependencyProperty FinishCommandProperty = DependencyProperty.Register(nameof(FinishCommand), typeof(ICommand), typeof(Wizard));
        public static readonly DependencyProperty UseCircularNavigationProperty = DependencyProperty.Register(nameof(UseCircularNavigation), typeof(bool), typeof(Wizard));
        public static readonly DependencyProperty NavigationBlockMinHeightProperty = DependencyProperty.Register(nameof(NavigationBlockMinHeight), typeof(double), typeof(Wizard));

        public static readonly DependencyProperty IsTransitionAnimationEnabledProperty = DependencyProperty.Register(nameof(IsTransitionAnimationEnabled), typeof(bool), typeof(Wizard));
        public static readonly DependencyProperty ForwardTransitionAnimationProperty = DependencyProperty.Register(nameof(ForwardTransitionAnimation), typeof(Storyboard), typeof(Wizard));
        public static readonly DependencyProperty BackwardTransitionAnimationProperty = DependencyProperty.Register(nameof(BackwardTransitionAnimation), typeof(Storyboard), typeof(Wizard));

        public static readonly DependencyProperty HelpCommandProperty = DependencyProperty.Register(nameof(HelpCommand), typeof(ICommand), typeof(Wizard));
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(Wizard));
        // ReSharper restore StyleCop.SA1600

        /// <summary>
        /// The default "Forward" transition animation.
        /// </summary>
        private static readonly Storyboard DefaultForwardTransitionAnimation;

        /// <summary>
        /// The default "Backward" transition animation.
        /// </summary>
        private static readonly Storyboard DefaultBackwardTransitionAnimation;

        /// <summary>
        /// Backing field for the <see cref="IsTransiting"/>.
        /// </summary>
        private bool isTransiting;

        public List<WizardStepSummary> SummaryControls { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="Wizard"/> class.
        /// </summary>
        static Wizard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Wizard), new FrameworkPropertyMetadata(typeof(Wizard)));

            DefaultForwardTransitionAnimation = new Storyboard();
            AnimationTimeline animationForward =
                new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                        From = new Thickness(500, 0, -500, 0),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9
                    };

            Storyboard.SetTargetProperty(animationForward, new PropertyPath("Margin"));
            DefaultForwardTransitionAnimation.Children.Add(animationForward);
            DefaultForwardTransitionAnimation.Freeze();

            DefaultBackwardTransitionAnimation = new Storyboard();
            AnimationTimeline animationBackward =
                new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                        From = new Thickness(-500, 0, 500, 0),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9
                    };

            Storyboard.SetTargetProperty(animationBackward, new PropertyPath("Margin"));
            DefaultBackwardTransitionAnimation.Children.Add(animationBackward);
            DefaultBackwardTransitionAnimation.Freeze();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        public Wizard()
        { 
            this.SharedContext = new Dictionary<string, object>();

            this.TransitionController = new TransitionController(
                this.ShowPreviousStep,
                () => this.ShowNextStep(false),
                () => this.ShowNextStep(true),
                x => this.FinishCommand?.Execute(x),
                () => this.SharedContext);

            this.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// Gets or sets the shared context.
        /// </summary>
        public Dictionary<string, object> SharedContext
        {
            get { return (Dictionary<string, object>)this.GetValue(SharedContextProperty); }
            set { this.SetValue(SharedContextProperty, value); }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the close/finish command.
        /// </summary>
        public ICommand FinishCommand
        {
            get { return (ICommand)this.GetValue(FinishCommandProperty); }
            set { this.SetValue(FinishCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether transition animation is enabled.
        /// </summary>
        public bool IsTransitionAnimationEnabled
        {
            get { return (bool)this.GetValue(IsTransitionAnimationEnabledProperty); }
            set { this.SetValue(IsTransitionAnimationEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether circular navigation should be used.
        /// i.e. Last Step -> Next Step will be First Step.
        ///      First Step -> Previous step will be Last Step.
        /// </summary>
        public bool UseCircularNavigation
        {
            get { return (bool)this.GetValue(UseCircularNavigationProperty); }
            set { this.SetValue(UseCircularNavigationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the navigation block (one with buttons and summary) minimum height.
        /// </summary>
        public double NavigationBlockMinHeight
        {
            get { return (double)this.GetValue(NavigationBlockMinHeightProperty); }
            set { this.SetValue(NavigationBlockMinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the "Forward" transition animation.
        /// </summary>
        public Storyboard ForwardTransitionAnimation
        {
            get { return (Storyboard)this.GetValue(ForwardTransitionAnimationProperty); }
            set { this.SetValue(ForwardTransitionAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the "Backward" transition animation.
        /// </summary>
        public Storyboard BackwardTransitionAnimation
        {
            get { return (Storyboard)this.GetValue(BackwardTransitionAnimationProperty); }
            set { this.SetValue(BackwardTransitionAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the help command.
        /// </summary>
        public ICommand HelpCommand
        {
            get { return (ICommand)this.GetValue(HelpCommandProperty); }
            set { this.SetValue(HelpCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        public ICommand CancelCommand
        {
            get { return (ICommand)this.GetValue(CancelCommandProperty); }
            set { this.SetValue(CancelCommandProperty, value); }
        }

        /// <summary>
        /// Gets the transition controller.
        /// </summary>
        public TransitionController TransitionController { get; }

        /// <summary>
        /// Gets the current step.
        /// </summary>
        public WizardStep CurrentStep => (WizardStep)this.SelectedItem;

        /// <summary>
        /// Gets the index of the current step.
        /// </summary>
        public int CurrentStepIndex => this.SelectedIndex;

        /// <summary>
        /// Gets the index of the first step.
        /// </summary>
        public int FirstStepIndex { get; } = 0;

        /// <summary>
        /// Gets the index of the last step.
        /// </summary>
        public int LastStepIndex => this.Items.Count - 1;

        /// <summary>
        /// Gets a value indicating whether current step is the first in the queue.
        /// </summary>
        public bool IsFirstStep => this.CurrentStepIndex == this.FirstStepIndex;

        /// <summary>
        /// Gets a value indicating whether current step is the last in the queue.
        /// </summary>
        public bool IsLastStep => this.CurrentStepIndex == this.LastStepIndex;

        /// <summary>
        /// Gets or sets a value indicating whether transition is in process.
        /// </summary>
        public bool IsTransiting
        {
            get
            {
                return this.isTransiting;
            }

            set
            {
                this.isTransiting = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The <see cref="TransitTo"/> wrapped into Try/Finally.
        /// </summary>
        /// <param name="transitToIndex">
        /// The transit to index.
        /// </param>
        /// <param name="skippingStep">
        /// The skipping step.
        /// </param>
        /// <remarks>
        /// awful [async void], but this sends exceptions to [DispatcherUnhandledException], not just silently kills thread.
        /// Async commands will be a nice improvement, so [async Task] could be used instead.
        /// </remarks>
        public async void TryTransitTo(int transitToIndex, bool skippingStep = false)
        {
            try
            {
                this.IsTransiting = true;

                await this.TransitTo(transitToIndex, skippingStep);
            }
            finally
            {
                this.IsTransiting = false;
            }
        }

        /// <inheritdoc />
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            foreach (WizardStep step in e.RemovedItems.OfType<WizardStep>())
            {
                step.Summary.IsSelected = false;
                HookOrUnhook ( step, false ) ;
            }

            if (e.AddedItems.Count == 0)
            {
                base.OnSelectionChanged(e);
                return;
            }

            var selectedStep = (WizardStep)e.AddedItems[0];
            selectedStep.Summary.IsSelected = true;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            HookOrUnhook ( selectedStep, true ) ;

            this.RaisePropertyChanged(nameof(this.CurrentStep));
            this.RaisePropertyChanged(nameof(this.IsFirstStep));
            this.RaisePropertyChanged(nameof(this.IsLastStep));

            this.RaisePropertyChanged(nameof(this.BackVisibility    ));
            this.RaisePropertyChanged(nameof(this.SkipVisibility    ));
            this.RaisePropertyChanged(nameof(this.ForwardVisibility ));
            this.RaisePropertyChanged(nameof(this.HelpVisibility    ));
            this.RaisePropertyChanged(nameof(this.CancelVisibility  ));
            this.RaisePropertyChanged(nameof(this.BackIsEnabled     ));
            this.RaisePropertyChanged(nameof(this.SkipIsEnabled     ));
            this.RaisePropertyChanged(nameof(this.ForwardIsEnabled  ));
            this.RaisePropertyChanged(nameof(this.HelpIsEnabled     ));
            this.RaisePropertyChanged(nameof(this.CancelIsEnabled   ));
            this.RaisePropertyChanged(nameof(this.BackTooltip       ));
            this.RaisePropertyChanged(nameof(this.SkipTooltip       ));
            this.RaisePropertyChanged(nameof(this.ForwardTooltip    ));
            this.RaisePropertyChanged(nameof(this.HelpTooltip       ));
            this.RaisePropertyChanged(nameof(this.CancelTooltip     ));
            this.RaisePropertyChanged(nameof(this.BackTitle         ));
            this.RaisePropertyChanged(nameof(this.SkipTitle         ));
            this.RaisePropertyChanged(nameof(this.ForwardTitle      ));
            this.RaisePropertyChanged(nameof(this.HelpTitle         ));
            this.RaisePropertyChanged(nameof(this.CancelTitle       ));

            base.OnSelectionChanged(e);
        }

        private void HookOrUnhook ( WizardStep step, bool hook )
        {
            HookOrUnhookAttachedProperty(step, Wizard.BackButtonVisibilityProperty,    BackVisibilityChanged   , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.SkipButtonVisibilityProperty,    SkipVisibilityChanged   , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.ForwardButtonVisibilityProperty, ForwardVisibilityChanged, hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.HelpButtonVisibilityProperty,    HelpVisibilityChanged   , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.CancelButtonVisibilityProperty,  CancelVisibilityChanged , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.BackButtonIsEnabledProperty,     BackIsEnabledChanged    , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.SkipButtonIsEnabledProperty,     SkipIsEnabledChanged    , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.ForwardButtonIsEnabledProperty,  ForwardIsEnabledChanged , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.HelpButtonIsEnabledProperty,     HelpIsEnabledChanged    , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.CancelButtonIsEnabledProperty,   CancelIsEnabledChanged  , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.BackButtonTooltipProperty,       BackTooltipChanged      , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.SkipButtonTooltipProperty,       SkipTooltipChanged      , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.ForwardButtonTooltipProperty,    ForwardTooltipChanged   , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.HelpButtonTooltipProperty,       HelpTooltipChanged      , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.CancelButtonTooltipProperty,     CancelTooltipChanged    , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.BackButtonTitleProperty,         BackTitleChanged        , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.SkipButtonTitleProperty,         SkipTitleChanged        , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.ForwardButtonTitleProperty,      ForwardTitleChanged     , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.HelpButtonTitleProperty,         HelpTitleChanged        , hook ) ;
            HookOrUnhookAttachedProperty(step, Wizard.CancelButtonTitleProperty,       CancelTitleChanged      , hook ) ;
        }

        private void HookOrUnhookAttachedProperty ( WizardStep step, DependencyProperty prop, EventHandler handler, bool hook )
        {
            var topDescriptor = DependencyPropertyDescriptor.FromProperty(prop, typeof(WizardStep));
            if ( hook )
            {
                topDescriptor.AddValueChanged(step, handler);
            }
            else
            {
                topDescriptor.RemoveValueChanged(step, handler);
            }
        }

        private void BackVisibilityChanged    ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(BackVisibility   )) ;
        private void SkipVisibilityChanged    ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(SkipVisibility   ));
        private void ForwardVisibilityChanged ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(ForwardVisibility));
        private void HelpVisibilityChanged    ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(HelpVisibility   ));
        private void CancelVisibilityChanged  ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(CancelVisibility ));
        private void BackIsEnabledChanged     ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(BackIsEnabled    ));
        private void SkipIsEnabledChanged     ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(SkipIsEnabled    ));
        private void ForwardIsEnabledChanged  ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(ForwardIsEnabled ));
        private void HelpIsEnabledChanged     ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(HelpIsEnabled    ));
        private void CancelIsEnabledChanged   ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(CancelIsEnabled  ));
        private void BackTooltipChanged       ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(BackTooltip      ));
        private void SkipTooltipChanged       ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(SkipTooltip      ));
        private void ForwardTooltipChanged    ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(ForwardTooltip   ));
        private void HelpTooltipChanged       ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(HelpTooltip      ));
        private void CancelTooltipChanged     ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(CancelTooltip    ));
        private void BackTitleChanged         ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(BackTitle        ));
        private void SkipTitleChanged         ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(SkipTitle        ));
        private void ForwardTitleChanged      ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(ForwardTitle     ));
        private void HelpTitleChanged         ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(HelpTitle        ));
        private void CancelTitleChanged       ( object sender, EventArgs e ) => RaisePropertyChanged(nameof(CancelTitle      ));


        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property.
        /// </param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when control gets loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.OnLoaded;

            SummaryControls = new List<WizardStepSummary>();
            foreach (WizardStep step in Items)
            {
                SummaryControls.Add(step.Summary);
            }
            RaisePropertyChanged("SummaryControls");

            if ( !DesignerProperties.GetIsInDesignMode(this) )
            {
                // Run mode
                this.TryTransitTo(this.FirstStepIndex);
            }
            else
            {
                // Design mode
                if ( ( Items.Count > 0 ) && ( this.SelectedIndex < 0 ) )
                {
                    this.SelectedIndex = 0 ;
                }
            }
        }

        /// <summary>
        /// Shows/goes to next step.
        /// </summary>
        /// <param name="skippingStep">
        /// The skipping step.
        /// </param>
        private void ShowNextStep(bool skippingStep)
        {
            int navigateTo = this.CurrentStepIndex + 1;

            if (this.UseCircularNavigation && this.IsLastStep)
            {
                navigateTo = this.FirstStepIndex;
            }

            this.TryTransitTo(navigateTo, skippingStep);
        }

        /// <summary>
        /// Shows/goes to previous steps
        /// </summary>
        private void ShowPreviousStep()
        {
            int navigateTo = this.CurrentStepIndex - 1;

            if (this.UseCircularNavigation && this.IsFirstStep)
            {
                navigateTo = this.LastStepIndex;
            }

            this.TryTransitTo(navigateTo);
        }

        /// <summary>
        /// Transits to the next/given step.
        /// </summary>
        /// <param name="transitToIndex">
        /// The transitToIndex.
        /// </param>
        /// <param name="skippingStep">
        /// The skipping step.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task TransitTo(int transitToIndex, bool skippingStep)
        {
            var transitionConext = new TransitionContext
                                       {
                                           SharedContext = this.SharedContext,
                                           TransitedFromStep = this.CurrentStepIndex,
                                           TransitToStep = transitToIndex,
                                           IsSkipAction = skippingStep,

                                           StepIndices =
                                               this.Items.Cast<WizardStep>().Select((x, i) => new { Name = x.Name, Index = i}).ToDictionary(
                                                   x => x.Index,
                                                   x => x.Name),
            };

            bool navigatingForward = !this.IsFirstStep && !this.IsLastStep && this.CurrentStepIndex < transitToIndex;
            navigatingForward |= this.IsFirstStep && transitToIndex != this.LastStepIndex;
            navigatingForward |= this.IsLastStep && transitToIndex == this.FirstStepIndex;

            /* Transit From (OnLoaded starts with "-1") */
            if (this.CurrentStepIndex >= this.FirstStepIndex)
            {
                var navigateFromView = (FrameworkElement)this.CurrentStep.Content;
                var navigateFrom = navigateFromView?.DataContext as ITransitionAware;

                if (navigateFrom != null)
                {
                    await navigateFrom.OnTransitedFrom(transitionConext);

                    if (transitionConext.AbortTransition)
                    {
                        return;
                    }
                }

                this.CurrentStep.Summary.IsProcessed = !skippingStep && navigatingForward;
            }

            /* Non-circular and index was not changed (by user) -> Special cases for first and last steps */
            if (!this.UseCircularNavigation && transitionConext.TransitToStep == transitToIndex)
            {
                /* First step and navigating back. */
                if (this.IsFirstStep && transitToIndex < this.FirstStepIndex)
                {
                    return;
                }

                /* Lasts step and navigating forward. */
                if (this.IsLastStep && transitToIndex > this.LastStepIndex)
                {
                    this.FinishCommand?.Execute(this.SharedContext);
                    return;
                }
            }

            transitToIndex = transitionConext.TransitToStep;

            if (transitToIndex > this.LastStepIndex)
            {
                string message =
                    $"Failed navigating to the step with index {transitToIndex} (Index out of range, max index is {this.LastStepIndex})";

                throw new IndexOutOfRangeException(message);
            }

            Debug.WriteLine($"Navigating to index {transitToIndex}");

            WizardStep stepToSelect = (WizardStep)this.Items[transitToIndex];

            if (stepToSelect.Content == null && stepToSelect.ViewType != null)
            {
                if (WizardSettings.Instance.ViewResolver == null)
                {
                    throw new NullReferenceException("WizardSettings.Instance.ViewResolver is not set.");
                }

                stepToSelect.Content = WizardSettings.Instance.ViewResolver(stepToSelect.ViewType);
            }

            /* Transit To */
            var navigateToView = (FrameworkElement)stepToSelect.Content;
            stepToSelect.UnderlyingDataContext = navigateToView?.DataContext;
            this.SelectedItem = stepToSelect;

            if (this.IsTransitionAnimationEnabled)
            {
                Storyboard storyboard = navigatingForward
                                            ? (this.ForwardTransitionAnimation ?? DefaultForwardTransitionAnimation)
                                            : (this.BackwardTransitionAnimation ?? DefaultBackwardTransitionAnimation);

                navigateToView?.BeginStoryboard(storyboard);
            }

            var navigateTo = navigateToView?.DataContext as ITransitionAware;

            if (navigateTo != null)
            {
                /* Do only once. */
                if (navigateTo.TransitionController == null)
                {
                    navigateTo.TransitionController = this.TransitionController;
                }

                await navigateTo.OnTransitedTo(transitionConext);
            }
        }
    }
}

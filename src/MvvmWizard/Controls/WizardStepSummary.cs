namespace MvvmWizard.Controls
{
    using MvvmWizard.Classes;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class WizardStepSummary : ButtonBase
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(WizardStepSummary));
        public static readonly DependencyProperty IsProcessedProperty = DependencyProperty.Register(nameof(IsProcessed), typeof(bool), typeof(WizardStepSummary));

        public WizardStep Step { get; }

        static WizardStepSummary()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardStepSummary), new FrameworkPropertyMetadata(typeof(WizardStepSummary)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardStepSummary"/> class.
        /// </summary>
        public WizardStepSummary ( WizardStep associatedStep )
        {
            Step = associatedStep;
            this.Command = new SimpleCommand(this.TransitToCurrent);
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether item/step is processed.
        /// </summary>
        public bool IsProcessed
        {
            get { return (bool)this.GetValue(IsProcessedProperty); }
            set { this.SetValue(IsProcessedProperty, value); }
        }

        /// <summary>
        /// Transit to current step.
        /// </summary>
        protected virtual void TransitToCurrent()
        {
            Wizard wizard = Step.ParentTabControl;

            if (!wizard.AllowNavigationOnSummaryItemClick)
            {
                return;
            }

            int transitTo = wizard.SummaryControls.IndexOf(this);

            if (wizard.CurrentStepIndex != transitTo)
            {
                wizard.TryTransitTo(transitTo, true);
            }
        }

    }
}

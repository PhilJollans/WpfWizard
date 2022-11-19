// ReSharper disable StyleCop.SA1600
namespace MvvmWizard.Controls
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The wizard step.
    /// </summary>
    public partial class WizardStep
    {
        public static readonly DependencyProperty BackButtonTooltipProperty = DependencyProperty.Register(nameof(BackButtonTooltip), typeof(object), typeof(WizardStep));
        public static readonly DependencyProperty SkipButtonTooltipProperty = DependencyProperty.Register(nameof(SkipButtonTooltip), typeof(object), typeof(WizardStep));
        public static readonly DependencyProperty ForwardButtonTooltipProperty = DependencyProperty.Register(nameof(ForwardButtonTooltip), typeof(object), typeof(WizardStep));
        public static readonly DependencyProperty HelpButtonTooltipProperty = DependencyProperty.Register(nameof(HelpButtonTooltip), typeof(object), typeof(WizardStep));

        public static readonly DependencyProperty BackButtonVisibilityProperty = DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility), typeof(WizardStep), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty SkipButtonVisibilityProperty = DependencyProperty.Register(nameof(SkipButtonVisibility), typeof(Visibility), typeof(WizardStep), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ForwardButtonVisibilityProperty = DependencyProperty.Register(nameof(ForwardButtonVisibility), typeof(Visibility), typeof(WizardStep), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty HelpButtonVisibilityProperty = DependencyProperty.Register(nameof(HelpButtonVisibility), typeof(Visibility), typeof(WizardStep), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty BackButtonIsEnabledProperty = DependencyProperty.Register(nameof(BackButtonIsEnabled), typeof(bool), typeof(WizardStep), new PropertyMetadata(true));
        public static readonly DependencyProperty SkipButtonIsEnabledProperty = DependencyProperty.Register(nameof(SkipButtonIsEnabled), typeof(bool), typeof(WizardStep), new PropertyMetadata(true));
        public static readonly DependencyProperty ForwardButtonIsEnabledProperty = DependencyProperty.Register(nameof(ForwardButtonIsEnabled), typeof(bool), typeof(WizardStep), new PropertyMetadata(true));
        public static readonly DependencyProperty HelpButtonIsEnabledProperty = DependencyProperty.Register(nameof(HelpButtonIsEnabled), typeof(bool), typeof(WizardStep), new PropertyMetadata(true));

        public static readonly DependencyProperty BackButtonTitleProperty = DependencyProperty.Register(nameof(BackButtonTitle), typeof(string), typeof(WizardStep));
        public static readonly DependencyProperty SkipButtonTitleProperty = DependencyProperty.Register(nameof(SkipButtonTitle), typeof(string), typeof(WizardStep));
        public static readonly DependencyProperty ForwardButtonTitleProperty = DependencyProperty.Register(nameof(ForwardButtonTitle), typeof(string), typeof(WizardStep));
        public static readonly DependencyProperty HelpButtonTitleProperty = DependencyProperty.Register(nameof(HelpButtonTitle), typeof(string), typeof(WizardStep));

        /* Tooltip */
        public object BackButtonTooltip
        {
            get { return this.GetValue(BackButtonTooltipProperty); }
            set { this.SetValue(BackButtonTooltipProperty, value); }
        }

        public object SkipButtonTooltip
        {
            get { return this.GetValue(SkipButtonTooltipProperty); }
            set { this.SetValue(SkipButtonTooltipProperty, value); }
        }

        public object ForwardButtonTooltip
        {
            get { return this.GetValue(ForwardButtonTooltipProperty); }
            set { this.SetValue(ForwardButtonTooltipProperty, value); }
        }

        public object HelpButtonTooltip
        {
            get { return this.GetValue(HelpButtonTooltipProperty); }
            set { this.SetValue(HelpButtonTooltipProperty, value); }
        }

        /* Visibility */
        public Visibility BackButtonVisibility
        {
            get { return (Visibility)this.GetValue(BackButtonVisibilityProperty); }
            set { this.SetValue(BackButtonVisibilityProperty, value); }
        }

        public Visibility SkipButtonVisibility
        {
            get { return (Visibility)this.GetValue(SkipButtonVisibilityProperty); }
            set { this.SetValue(SkipButtonVisibilityProperty, value); }
        }

        public Visibility ForwardButtonVisibility
        {
            get { return (Visibility)this.GetValue(ForwardButtonVisibilityProperty); }
            set { this.SetValue(ForwardButtonVisibilityProperty, value); }
        }

        public Visibility HelpButtonVisibility
        {
            get { return (Visibility)this.GetValue(HelpButtonVisibilityProperty); }
            set { this.SetValue(HelpButtonVisibilityProperty, value); }
        }

        /* IsEnabled */
        public bool BackButtonIsEnabled
        {
            get { return (bool)this.GetValue(BackButtonIsEnabledProperty); }
            set { this.SetValue(BackButtonIsEnabledProperty, value); }
        }

        public bool SkipButtonIsEnabled
        {
            get { return (bool)this.GetValue(SkipButtonIsEnabledProperty); }
            set { this.SetValue(SkipButtonIsEnabledProperty, value); }
        }

        public bool ForwardButtonIsEnabled
        {
            get { return (bool)this.GetValue(ForwardButtonIsEnabledProperty); }
            set { this.SetValue(ForwardButtonIsEnabledProperty, value); }
        }

        public bool HelpButtonIsEnabled
        {
            get { return (bool)this.GetValue(HelpButtonIsEnabledProperty); }
            set { this.SetValue(HelpButtonIsEnabledProperty, value); }
        }

        /* Title */
        public string BackButtonTitle
        {
            get { return (string)this.GetValue(BackButtonTitleProperty); }
            set { this.SetValue(BackButtonTitleProperty, value); }
        }

        public string SkipButtonTitle
        {
            get { return (string)this.GetValue(SkipButtonTitleProperty); }
            set { this.SetValue(SkipButtonTitleProperty, value); }
        }

        public string ForwardButtonTitle
        {
            get { return (string)this.GetValue(ForwardButtonTitleProperty); }
            set { this.SetValue(ForwardButtonTitleProperty, value); }
        }

        public string HelpButtonTitle
        {
            get { return (string)this.GetValue(HelpButtonTitleProperty); }
            set { this.SetValue(HelpButtonTitleProperty, value); }
        }
    }
}

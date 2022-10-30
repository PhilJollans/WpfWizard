// ReSharper disable StyleCop.SA1600
namespace MvvmWizard.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using MvvmWizard.Classes;

    /// <summary>
    /// The wizard step.
    /// </summary>
    public partial class WizardStep : ContentControl
    {
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register(nameof(ViewType), typeof(Type), typeof(WizardStep));
        public static readonly DependencyProperty UnderlyingDataContextProperty = DependencyProperty.Register(nameof(UnderlyingDataContext), typeof(object), typeof(WizardStep));

        public WizardStepSummary Summary { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="WizardStep"/> class.
        /// </summary>
        static WizardStep()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WizardStep),
                new FrameworkPropertyMetadata(typeof(WizardStep)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardStep"/> class.
        /// </summary>
        public WizardStep()
        {
            this.Summary = new WizardStepSummary(this);
        }

        public Type ViewType
        {
            get { return (Type)this.GetValue(ViewTypeProperty); }
            set { this.SetValue(ViewTypeProperty, value); }
        }

        public object UnderlyingDataContext
        {
            get { return this.GetValue(UnderlyingDataContextProperty); }
            set { this.SetValue(UnderlyingDataContextProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="Wizard"/> (parent).
        /// </summary>
        public Wizard ParentTabControl
        {
            get
            {
                ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);

                /* If "Wizard" has dynamic steps (ItemsSource="{Binding ...}) 
                 * then "ItemsControl.ItemsControlFromItemContainer(this)" returns "ItemsControl" object,
                * which cannot be casted to "Wizard", but its templated parent is exactly what we want. */
                var wizard = itemsControl as Wizard ?? itemsControl?.TemplatedParent as Wizard;

                return wizard;
            }
        }

    }
}

using System;
using Cassowary;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
	public class Constraint : BindableObject, IConstraint
    {
        public enum Attribute
        {
			None,
            Top,
            Left,
            Right,
            Bottom,         
			CenterX,
            CenterY,
            Width,
            Height
		}
		
	    public enum Relation
	    {
		    Equal,
		    GreaterThan,
		    LessThan
	    }

		public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(View), typeof(Constraint), propertyChanged:(bindable, oldValue, newValue) => {
			var constraint = ((Constraint)bindable);
			constraint.OnItemChanged(constraint);
		});      

		public static BindableProperty TargetProperty = BindableProperty.Create(nameof(Target), typeof(View), typeof(Constraint), propertyChanged:(bindable, oldValue, newValue) => {
            var constraint = ((Constraint)bindable);
            constraint.OnItemChanged(constraint);
        });

		public static BindableProperty SourceAttributeProperty = BindableProperty.Create(nameof(SourceAttribute), typeof(Attribute), typeof(Constraint), Attribute.None, propertyChanged:(bindable, oldValue, newValue) => {
            var constraint = ((Constraint)bindable);
            constraint.OnItemChanged(constraint);
        }); 

		public static BindableProperty TargetAttributeProperty = BindableProperty.Create(nameof(TargetAttribute), typeof(Attribute), typeof(Constraint), Attribute.None, propertyChanged:(bindable, oldValue, newValue) => {
            var constraint = ((Constraint)bindable);
            constraint.OnItemChanged(constraint);
        });    

		public static BindableProperty MultiplierProperty = BindableProperty.Create(nameof(Multiplier), typeof(double), typeof(Constraint), 1.0, propertyChanged:(bindable, oldValue, newValue) => {
            var constraint = ((Constraint)bindable);
            constraint.OnItemChanged(constraint);
        }); 

		public static BindableProperty ConstantProperty = BindableProperty.Create(nameof(Constant), typeof(double), typeof(Constraint), 0.0, propertyChanged:(bindable, oldValue, newValue) => {
            var constraint = ((Constraint)bindable);
            constraint.OnItemChanged(constraint);
        });  
	    
		public static BindableProperty RelatedByProperty = BindableProperty.Create(nameof(RelatedBy), typeof(Relation), typeof(Constraint), Relation.Equal, propertyChanged:(bindable, oldValue, newValue) => {
		    var constraint = ((Constraint)bindable);
		    constraint.OnItemChanged(constraint);
	    }); 

        public View Source
        {
            get => (View) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public View Target
        {
            get => (View)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public Attribute SourceAttribute
        {
            get => (Attribute)GetValue(SourceAttributeProperty);
            set => SetValue(SourceAttributeProperty, value);
        }

        public Attribute TargetAttribute
        {
            get => (Attribute)GetValue(TargetAttributeProperty);
            set => SetValue(TargetAttributeProperty, value);
        }

		public double Multiplier 
		{ 
			get => (double)GetValue(MultiplierProperty);
			set => SetValue(MultiplierProperty, value);
		}

		public double Constant
		{
			get => (double)GetValue(ConstantProperty);
            set => SetValue(ConstantProperty, value);
		}

	    public Relation RelatedBy
	    {
		    get => (Relation)GetValue(RelatedByProperty);
		    set => SetValue(RelatedByProperty, value);
	    }

		public event EventHandler ItemChanged;

        internal ClVariable Variable { get; set; }
	    

	    public Constraint()
		{
		}

		private void OnItemChanged(Constraint constraint)
        {
			ItemChanged?.Invoke(this, EventArgs.Empty);
        }
	}
}
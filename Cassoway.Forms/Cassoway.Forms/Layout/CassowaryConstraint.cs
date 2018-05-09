using System;
using Cassowary;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
    public class CassowaryConstraint : BindableObject, IConstraint
    {
        public enum Attribute
        {
            Top,
            Left,
            Right,
            Bottom,
			None
		}

        public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(View), typeof(CassowaryConstraint));
        public static BindableProperty TargetProperty = BindableProperty.Create(nameof(Target), typeof(View), typeof(CassowaryConstraint));
		public static BindableProperty SourceAttributeProperty = BindableProperty.Create(nameof(SourceAttribute), typeof(Attribute), typeof(CassowaryConstraint), Attribute.None);
		public static BindableProperty TargetAttributeProperty = BindableProperty.Create(nameof(TargetAttribute), typeof(Attribute), typeof(CassowaryConstraint), Attribute.None);
		public static BindableProperty MultiplierProperty = BindableProperty.Create(nameof(Multiplier), typeof(double), typeof(CassowaryConstraint), 1.0);

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


        public event EventHandler ItemChanged;

        internal ClVariable Variable { get; set; }
        
		public CassowaryConstraint()
		{
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassowary;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
	public class CassowaryConstraint : BindableObject
	{
		public enum Attribute
		{
			Top,
			Left,
			Right,
			Bottom
		}

		public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(View), typeof(CassowaryConstraint));
		public static BindableProperty TargetProperty = BindableProperty.Create(nameof(Target), typeof(View), typeof(CassowaryConstraint));
		public static BindableProperty SourceAttributeProperty = BindableProperty.Create(nameof(SourceAttribute), typeof(Attribute), typeof(CassowaryConstraint));
		public static BindableProperty TargetAttributeProperty = BindableProperty.Create(nameof(SourceAttribute), typeof(Attribute), typeof(CassowaryConstraint));

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
	}

	public class CassowaryLayout : Layout<View>
    {
		public static BindableProperty ConstraintsProperty = BindableProperty.Create(nameof(Constraints), typeof(IList<CassowaryConstraint>), typeof(CassowaryLayout));

        public CassowaryLayout()
        {
            Solver = new ClSimplexSolver();
			Constraints = new List<CassowaryConstraint>();
        }

		public IList<CassowaryConstraint> Constraints
		{
			get => (IList<CassowaryConstraint>) GetValue(ConstraintsProperty);
			set => SetValue(ConstraintsProperty, value);
		}

	    private bool _hasConstraints =false;
        public ClSimplexSolver Solver { get; set; }
	    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
	    {
			return new SizeRequest(new Size(widthConstraint, heightConstraint));
	    }

	    protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var (labelRect, boxRect, box2Rect, label, boxView, box2View) = PerformLayout(width, height);

	        LayoutChildIntoBoundingRegion(label, labelRect);
	        LayoutChildIntoBoundingRegion(boxView, boxRect);
	        LayoutChildIntoBoundingRegion(box2View, box2Rect);
        }

	    private (Rectangle labelRect, Rectangle boxRect, Rectangle box2Rect, View label, View boxView, View box2View) PerformLayout(double width, double height)
	    {
		    if (!_hasConstraints)
			    CreateConstraints();

		    var label = Children[0];
		    var boxView = Children[1];
		    var box2View = Children[2];

		    var labelSizeRequest = label.Measure(LabelRight.Value, double.PositiveInfinity);

		    Solver.BeginEdit(WindowLeft, WindowRight, WindowTop, WindowBottom)
			    .SuggestValue(WindowLeft, 0)
			    .SuggestValue(WindowTop, 0)
			    .SuggestValue(WindowRight, width)
			    .SuggestValue(WindowBottom, height)
			    .EndEdit()
			    .Solve();

		    labelSizeRequest = label.Measure(LabelRight.Value, double.PositiveInfinity);

		    Solver.BeginEdit(LabelHeight)
			    .SuggestValue(LabelHeight, labelSizeRequest.Request.Height)
			    .EndEdit()
			    .Solve();

		    var heightValue = LabelHeight.Value;
		    System.Diagnostics.Debug.WriteLine($"Height {heightValue}");
		    var rect = Rectangle.FromLTRB(LabelLeft.Value, LabelTop.Value, LabelRight.Value, heightValue);

		    var boxRect = Rectangle.FromLTRB(BoxLeft.Value, BoxTop.Value, BoxRight.Value, BoxBottom.Value);

		    var box2Rect = Rectangle.FromLTRB(Box2Left.Value, Box2Top.Value, Box2Right.Value, Box2Bottom.Value);
		    return (rect, boxRect, box2Rect, label, boxView, box2View);
	    }

	    private void CreateConstraints()
        {
            _hasConstraints = true;
            
            WindowLeft = new ClVariable("WindowLeft");
            WindowRight = new ClVariable("WindowRight");
            WindowTop = new ClVariable("WindowTop");
            WindowBottom = new ClVariable("WindowBottom");

            LabelLeft = new ClVariable(nameof(LabelLeft));
            LabelTop = new ClVariable(nameof(LabelTop));
            LabelRight = new ClVariable(nameof(LabelRight));
            LabelBottom = new ClVariable(nameof(LabelBottom));
			LabelHeight = new ClVariable(nameof(LabelHeight));

			BoxLeft = new ClVariable(nameof(BoxLeft));
			BoxTop = new ClVariable(nameof(BoxTop));
			BoxRight = new ClVariable(nameof(BoxRight));
			BoxBottom = new ClVariable(nameof(BoxBottom));

			Box2Left = new ClVariable(nameof(Box2Left));
            Box2Top = new ClVariable(nameof(Box2Top));
            Box2Right = new ClVariable(nameof(Box2Right));
            Box2Bottom = new ClVariable(nameof(Box2Bottom));

			Solver.AddStay(WindowLeft);
			Solver.AddStay(WindowTop);
			Solver.AddStay(WindowRight);
			Solver.AddStay(WindowBottom);
			Solver.AddStay(LabelHeight);

            Solver.AddConstraint(LabelLeft, LabelRight, WindowLeft, WindowRight,
			                     (labelLeft, labelRight, windowLeft, windowRight) => labelLeft == windowLeft && labelRight == (windowRight * 0.5));
            Solver.AddConstraint(LabelTop, WindowTop,
			                     (labelTop, windowTop) => labelTop == windowTop);

			Solver.AddConstraint(BoxLeft, BoxRight, LabelRight, WindowRight, (boxLeft, boxRight, labelRight, windowRight) => boxLeft == labelRight && boxRight == windowRight);
			Solver.AddConstraint(BoxTop, BoxBottom, WindowTop, WindowBottom, (boxTop, boxBottom, windowTop, windowBottom) => boxTop == windowTop && boxBottom == windowBottom - 10);

			Solver.AddConstraint(Box2Left, Box2Right, WindowLeft, BoxLeft, (box2Left, box2Right, windowLeft, boxLeft) => box2Left == windowLeft && box2Right== boxLeft);
			Solver.AddConstraint(Box2Top, LabelTop, LabelHeight, (box2Top, labelTop, labelHeight) => box2Top == labelTop + labelHeight);
			Solver.AddConstraint(Box2Bottom, WindowBottom, (box2Bottom, windowBottom) => box2Bottom == windowBottom - 10);
        }

        public ClVariable LabelBottom { get; set; }

        public ClVariable LabelRight { get; set; }

        public ClVariable LabelLeft { get; set; }

        public ClVariable LabelTop { get; set; }

		public ClVariable LabelHeight { get; set; }      

		public ClVariable BoxBottom { get; set; }

        public ClVariable BoxRight { get; set; }

        public ClVariable BoxLeft { get; set; }

        public ClVariable BoxTop { get; set; }

		public ClVariable Box2Bottom { get; set; }

        public ClVariable Box2Right { get; set; }

        public ClVariable Box2Left { get; set; }

        public ClVariable Box2Top { get; set; }

        public ClVariable WindowTop { get; set; }

        public ClVariable WindowBottom { get; set; }

        public ClVariable WindowRight { get; set; }

        public ClVariable WindowLeft { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassowary;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
	public class CassowaryLayout : Layout<View>
    {
		public static BindableProperty ConstraintsProperty = BindableProperty.Create(nameof(Constraints), typeof(ConstraintCollection), typeof(CassowaryLayout), 
			validateValue: (bindable, value) => value != null, 
			propertyChanged: (bindable, oldvalue, newvalue) =>
		{
			if (oldvalue != null)
				((ConstraintCollection)oldvalue).ItemChanged -= ((CassowaryLayout)bindable).OnDefinitionChanged;
			
			if (newvalue != null)
				((ColumnDefinitionCollection)newvalue).ItemSizeChanged += ((CassowaryLayout)bindable).OnDefinitionChanged;
		}, defaultValueCreator: bindable =>
		{
			var constraintCollection = new ConstraintCollection();
			constraintCollection.ItemChanged += ((CassowaryLayout)bindable).OnDefinitionChanged;
			return constraintCollection;
		});

	    public CassowaryLayout()
        {
            Solver = new ClSimplexSolver();
        }

		public ConstraintCollection Constraints
		{
			get => (ConstraintCollection) GetValue(ConstraintsProperty);
			set => SetValue(ConstraintsProperty, value);
		}

	    private void OnDefinitionChanged(object sender, EventArgs e)
	    {
		    UpdateConstraints();
		    InvalidateLayout();
	    }

	    private void UpdateConstraints()
	    {
		    
	    }
	    
	    private bool _hasConstraints = false;
        public ClSimplexSolver Solver { get; set; }
	    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
	    {
			return new SizeRequest(new Size(widthConstraint, heightConstraint));
	    }

	    protected override void LayoutChildren(double x, double y, double width, double height)
        {
            PerformLayout(width, height);

	        foreach (var child in Children)
	        {
		        var variables = _variables.Keys.Where(v => v.Contains(child.Id.ToString()));
				var leftId = variables.FirstOrDefault(v => v.Contains(GetAttributeName(CassowaryConstraint.Attribute.Left)));
				var rightId = variables.FirstOrDefault(v => v.Contains(GetAttributeName(CassowaryConstraint.Attribute.Right)));
				var topId = variables.FirstOrDefault(v => v.Contains(GetAttributeName(CassowaryConstraint.Attribute.Top)));
				var bottomId = variables.FirstOrDefault(v => v.Contains(GetAttributeName(CassowaryConstraint.Attribute.Bottom)));

				var left = _variables[leftId];
				var right = _variables[rightId];
				var top = _variables[topId];
				var bottom = _variables[bottomId];

				var rect = Rectangle.FromLTRB(left.Value, top.Value, right.Value, bottom.Value);
				LayoutChildIntoBoundingRegion(child, rect);
	        }
//	        LayoutChildIntoBoundingRegion(label, labelRect);
//	        LayoutChildIntoBoundingRegion(boxView, boxRect);
//	        LayoutChildIntoBoundingRegion(box2View, box2Rect);
        }

	    private void PerformLayout(double width, double height)
	    {
		    if (!_hasConstraints)
			    CreateConstraints();

		    Solver.BeginEdit(LayoutRight, LayoutBottom)
			    .SuggestValue(LayoutRight, width)
			    .SuggestValue(LayoutBottom, height)
			    .EndEdit()
			    .Solve();
		    
		    /*var label = Children[0];
		    var boxView = Children[1];
		    var box2View = Children[2];

		    var labelSizeRequest = label.Measure(LabelRight.Value, double.PositiveInfinity);

		    

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
		    return (rect, boxRect, box2Rect, label, boxView, box2View);*/
	    }

	    private readonly Dictionary<string, ClVariable> _variables = new Dictionary<string, ClVariable>();
	    
	    private void CreateConstraints()
        {
            _hasConstraints = true;

			LayoutLeft = GetVariable($"{Id}.Left");
			LayoutRight = GetVariable($"{Id}.Right");
			LayoutTop = GetVariable($"{Id}.Top");
			LayoutBottom = GetVariable($"{Id}.Bottom");

	        Solver.AddStay(LayoutLeft);
	        Solver.AddStay(LayoutTop);
	        Solver.AddStay(LayoutRight);
	        Solver.AddStay(LayoutBottom);

	        foreach (var constraint in Constraints)
	        {
		        var sourceVariableName =
			        $"{constraint.Source.Id.ToString()}.{GetAttributeName(constraint.SourceAttribute)}";
		        var targetVariableName =
			        $"{constraint.Target.Id.ToString()}.{GetAttributeName(constraint.TargetAttribute)}";

		        var sourceVariable = GetVariable(sourceVariableName);
		        var targetVariable = GetVariable(targetVariableName);

				Solver.AddConstraint(sourceVariable, targetVariable, (source, target) => target * constraint.Multiplier == source);
	        }
        }

	    private ClVariable GetVariable(string variableName)
	    {
		    if (_variables.TryGetValue(variableName, out var variable))
			    return variable;

		    variable = new ClVariable(variableName);
		    _variables[variableName] = variable;

		    return variable;
	    }

	    private static string GetAttributeName(CassowaryConstraint.Attribute attribute)
	    {
		    return Enum.GetName(typeof(CassowaryConstraint.Attribute), attribute);
	    }

        public ClVariable LayoutTop { get; set; }

        public ClVariable LayoutBottom { get; set; }

        public ClVariable LayoutRight { get; set; }

        public ClVariable LayoutLeft { get; set; }
    }
}

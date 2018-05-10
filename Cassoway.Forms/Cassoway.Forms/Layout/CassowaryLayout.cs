using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cassowary;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
	public class CassowaryLayout : Layout<View>
    {
	    const double Epsilon = 0.00001;
	    
		public static BindableProperty ConstraintsProperty = BindableProperty.Create(nameof(Constraints), typeof(ConstraintCollection), typeof(CassowaryLayout), 
			validateValue: (bindable, value) => value != null, 
			propertyChanged: (bindable, oldvalue, newvalue) =>
		{
			if (oldvalue != null)
				((ConstraintCollection)oldvalue).ItemChanged -= ((CassowaryLayout)bindable).OnDefinitionChanged;
			
			if (newvalue != null)
				((ConstraintCollection)newvalue).ItemChanged += ((CassowaryLayout)bindable).OnDefinitionChanged;
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
	    
	    private bool _hasConstraints;
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
				// support width, height & center
				var left = GetVariableValue(child.Id, Constraint.Attribute.Left, 0.0);
		        var right = GetVariableValue(child.Id, Constraint.Attribute.Right, 0.0);
		        var top = GetVariableValue(child.Id, Constraint.Attribute.Top, 0.0);
		        var bottom = GetVariableValue(child.Id, Constraint.Attribute.Bottom, 0.0);            
		        var centerX = GetVariableValue(child.Id, Constraint.Attribute.CenterX, 0.0);
		        var centerY = GetVariableValue(child.Id, Constraint.Attribute.CenterY, 0.0);
		        var vWidth = GetVariableValue(child.Id, Constraint.Attribute.Width, 0.0);
		        var vHeight = GetVariableValue(child.Id, Constraint.Attribute.Height, 0.0);
		        
				var rect = Rectangle.FromLTRB(left, top, right, bottom);
				if (Math.Abs(vWidth) > Epsilon )
					rect.Width = vWidth;
				
				if (Math.Abs(vHeight) > Epsilon)
				    rect.Height = vHeight;

		        if (Math.Abs(centerY) > Epsilon && Math.Abs(rect.Center.Y - centerY) > Epsilon)
				    rect = rect.Offset(0, centerY * 0.5);

				if (Math.Abs(centerX) > Epsilon && Math.Abs(rect.Center.X - centerX) > Epsilon)
					rect = rect.Offset(centerX * 0.5, 0);

				LayoutChildIntoBoundingRegion(child, rect);
	        }
        }

	    private void PerformLayout(double width, double height)
	    {
		    if (!_hasConstraints)
			    CreateConstraints();

		    var layoutRight = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Right)}");
		    var layoutBottom = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Bottom)}");
		    var layoutWidth = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Width)}");
		    var layoutHeight = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Height)}");
		    var layoutCenterX = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.CenterX)}");
		    var layoutCenterY = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.CenterY)}");
		    
		    Solver.BeginEdit(layoutRight, layoutBottom, layoutWidth, layoutHeight, layoutCenterX, layoutCenterY)
			    .SuggestValue(layoutRight, width)
			    .SuggestValue(layoutBottom, height)
			    .SuggestValue(layoutWidth, width)
			    .SuggestValue(layoutHeight, height)
			    .SuggestValue(layoutCenterX, width * 0.5)
			    .SuggestValue(layoutCenterY, height * 0.5)
			    .EndEdit()
			    .Solve();
	    }

	    private readonly Dictionary<string, ClVariable> _variables = new Dictionary<string, ClVariable>();

	    private void CreateConstraints()
        {
            _hasConstraints = true;

			var layoutLeft = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Left)}");
			var layoutRight = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Right)}");
			var layoutTop = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Top)}");
			var layoutBottom = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Bottom)}");
			var layoutWidth = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Width)}");
	        var layoutHeight = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.Height)}");
	        var layoutCenterX = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.CenterX)}");
	        var layoutCenterY = GetVariable($"{Id}.{GetAttributeName(Constraint.Attribute.CenterY)}");
	        
	        Solver.AddStay(layoutLeft);
	        Solver.AddStay(layoutTop);
	        Solver.AddStay(layoutRight);
	        Solver.AddStay(layoutBottom);
	        Solver.AddStay(layoutWidth);
	        Solver.AddStay(layoutHeight);
	        Solver.AddStay(layoutCenterX);
	        Solver.AddStay(layoutCenterY);

	        foreach (var child in Children)
	        {
		        var widthVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Width)}");
		        var leftVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Left)}");
		        var rightVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Right)}");
		        
		        var heightVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Height)}");
		        var topVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Top)}");
		        var bottomVariable = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.Bottom)}");

				var centerX = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.CenterX)}");
				var centerY = GetVariable($"{child.Id.ToString()}.{GetAttributeName(Constraint.Attribute.CenterY)}");
                
				//Solver.AddConstraint(centerY, topVariable, heightVariable, (cy, t, h) => cy == t + (h * 0.5));
				Solver.AddConstraint(heightVariable, topVariable, bottomVariable, centerY, (h, t, b, cy) => h == b - t && cy == t + h * 0.5);
		        Solver.AddConstraint(widthVariable, leftVariable, rightVariable, centerX, (h, t, b, cy) => h == b - t && cy == t + h * 0.5);

		  //      Solver.AddConstraint(widthVariable, leftVariable, rightVariable, (w, l, r) => w == r - l);
		  //      Solver.AddConstraint(heightVariable, topVariable, bottomVariable, (w, l, r) => w == r - l);
				//Solver.AddConstraint(heightVariable, topVariable, bottomVariable, (h, t, b) => b == t + h);
				//Solver.AddConstraint(heightVariable, centerY, bottomVariable, (h, cy, b) => b == cy + (h * 0.5));
				//Solver.AddConstraint(heightVariable, centerY, topVariable, (h, cy, t) => t == cy - (h * 0.5));
	        }
	        
	        foreach (var constraint in Constraints)
	        {
		        var sourceVariableName =
			        $"{constraint.Source.Id.ToString()}.{GetAttributeName(constraint.SourceAttribute)}";

				var sourceVariable = GetVariable(sourceVariableName);

				if (constraint.Target != null){
					var targetVariableName =
                    $"{constraint.Target.Id.ToString()}.{GetAttributeName(constraint.TargetAttribute)}";

					var targetVariable = GetVariable(targetVariableName);

					Solver.AddConstraint(sourceVariable, targetVariable, GetExpression(constraint.RelatedBy, constraint.Multiplier, constraint.Constant));
				} else 
				{
					Solver.AddConstraint(sourceVariable, GetSingleExpression(constraint.RelatedBy, constraint.Multiplier, constraint.Constant));
				}
	        }
        }

		private Expression<Func<double, bool>> GetSingleExpression(Constraint.Relation constraintRelatedBy, double multiplier, double constant)
        {
            switch (constraintRelatedBy)
            {
                case Constraint.Relation.Equal:
					return (source) => source == multiplier + constant;
                case Constraint.Relation.GreaterThan:
					return (source) => source >= multiplier + constant;
                case Constraint.Relation.LessThan:
					return (source) => source <= multiplier + constant;
                default:
                    throw new ArgumentOutOfRangeException(nameof(constraintRelatedBy), constraintRelatedBy, null);
            }
        }

	    private Expression<Func<double, double, bool>> GetExpression(Constraint.Relation constraintRelatedBy, double constraintMultiplier, double constraintConstant)
	    {
		    switch (constraintRelatedBy)
		    {
			    case Constraint.Relation.Equal:
				    return EqualsExpression(constraintMultiplier, constraintConstant);
			    case Constraint.Relation.GreaterThan:
				    return GreaterThanExpression(constraintMultiplier, constraintConstant);
			    case Constraint.Relation.LessThan:
				    return LessThanExpression(constraintMultiplier, constraintConstant);
			    default:
				    throw new ArgumentOutOfRangeException(nameof(constraintRelatedBy), constraintRelatedBy, null);
		    }
	    }

	    private Expression<Func<double, double, bool>> GreaterThanExpression(double multiplier, double constant)
	    {
		    return (source, target) => source >= target * multiplier + constant;;
	    }
	    
	    private Expression<Func<double, double, bool>> LessThanExpression(double multiplier, double constant)
	    {
		    return (source, target) => source <= target * multiplier + constant;;
	    }
	    
	    private Expression<Func<double, double, bool>> EqualsExpression(double multiplier, double constant)
	    {
		    return (source, target) => source == target * multiplier + constant;;
	    }

	    private double GetVariableValue(string variableId, double defaultValue)
	    {
		    if (_variables.TryGetValue(variableId, out var variable))
			    return variable.Value;
		    return defaultValue;
	    }
	    
	    private ClVariable GetVariable(string variableName)
	    {
		    if (_variables.TryGetValue(variableName, out var variable))
			    return variable;

		    variable = new ClVariable(variableName);
		    _variables[variableName] = variable;

		    return variable;
	    }
	    
	    private double GetVariableValue(Guid childId, Constraint.Attribute attribute, double defaultValue)
	    {
		    var name = GetName(childId, attribute);
		    return GetVariableValue(name, defaultValue);
	    }

	    private string GetName(Guid childId, Constraint.Attribute attribute)
	    {
		    return $"{childId.ToString()}.{GetAttributeName(attribute)}";
	    }

	    private static string GetAttributeName(Constraint.Attribute attribute)
	    {
		    return Enum.GetName(typeof(Constraint.Attribute), attribute);
	    }
    }
}

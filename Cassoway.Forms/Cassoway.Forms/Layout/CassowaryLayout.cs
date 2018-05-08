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
        public CassowaryLayout()
        {
            Solver = new ClSimplexSolver();
        }

        private bool _hasConstraints  =false;
        public ClSimplexSolver Solver { get; set; }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!_hasConstraints)
                CreateConstraints();


            Solver.BeginEdit(WindowLeft, WindowRight, WindowTop, WindowBottom)
                .SuggestValue(WindowLeft, x)
                .SuggestValue(WindowTop, y)
                .SuggestValue(WindowRight, width)
                .SuggestValue(WindowBottom, height)
                .EndEdit();
        }

        private void CreateConstraints()
        {
            _hasConstraints = true;
            
            WindowLeft = new ClVariable("WindowLeft");
            WindowRight = new ClVariable("WindowRight");
            WindowTop = new ClVariable("WindowTop");
            WindowBottom = new ClVariable("WindowBottom");
            
            foreach (var child in Children)
            {
                    if (child is Label label)
                    {
                        var labelLeft = new ClVariable("labelLeft");
                        var labelRight = new ClVariable("labelRight");

                        Solver.AddConstraint(labelLeft, labelRight, labelTop, labelBottom, (ll, lr, wr, wl) => { });
                    }
            }
        }

        public ClVariable WindowTop { get; set; }

        public ClVariable WindowBottom { get; set; }

        public ClVariable WindowRight { get; set; }

        public ClVariable WindowLeft { get; set; }
    }
}

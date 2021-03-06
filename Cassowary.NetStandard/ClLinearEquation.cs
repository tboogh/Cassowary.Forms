/*
  Cassowary.net: an incremental constraint solver for .NET
  (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
  
  Copyright (C) 2005-2006  Jo Vermeulen (jo.vermeulen@uhasselt.be)
  
  This program is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public License
  as published by the Free Software Foundation; either version 2.1
  of  the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

namespace Cassowary
{
    public class ClLinearEquation : ClLinearConstraint
    {
        public ClLinearEquation(ClLinearExpression cle, ClStrength strength, double weight)
            : base(cle, strength, weight)
        {
        }

        public ClLinearEquation(ClLinearExpression cle, ClStrength strength)
            : base(cle, strength)
        {
        }

        public ClLinearEquation(ClLinearExpression cle)
            : base(cle)
        {
        }

        public ClLinearEquation(ClAbstractVariable clv, ClLinearExpression cle, ClStrength strength, double weight = 1.0)
            : base(cle, strength, weight)
        {
            ExpressionField.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(ClAbstractVariable clv, ClLinearExpression cle)
            : this(clv, cle, ClStrength.Required)
        {
        }

        public ClLinearEquation(ClAbstractVariable clv, double val, ClStrength strength, double weight = 1.0)
            : base(new ClLinearExpression(val), strength, weight)
        {
            ExpressionField.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(ClAbstractVariable clv,
            double val)
            : this(clv, val, ClStrength.Required)
        {
        }

        public ClLinearEquation(ClLinearExpression cle, ClAbstractVariable clv, ClStrength strength, double weight = 1.0)
            : base(cle.Clone(), strength, weight)
        {
            ExpressionField.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(ClLinearExpression cle, ClAbstractVariable clv)
            : this(cle, clv, ClStrength.Required)
        {
        }

        public ClLinearEquation(ClLinearExpression cle1, ClLinearExpression cle2, ClStrength strength, double weight = 1.0)
            : base(cle1.Clone(), strength, weight)
        {
            ExpressionField.AddExpression(cle2, -1.0);
        }

        public ClLinearEquation(ClLinearExpression cle1, ClLinearExpression cle2)
            : this(cle1, cle2, ClStrength.Required)
        {
        }

        public override string ToString()
        {
            return base.ToString() + " = 0)";
        }
    }
}

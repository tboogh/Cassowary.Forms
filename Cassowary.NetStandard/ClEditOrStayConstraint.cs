/*
  Cassowary.net: an incremental constraint solver for .NET
  (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
    
  Copyright (C) 2005  Jo Vermeulen (jo.vermeulen@uhasselt.be)
    
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
    public abstract class ClEditOrStayConstraint : ClConstraint
    {
        protected ClEditOrStayConstraint(ClVariable var,
            ClStrength strength,
            double weight = 1.0)
            : base(strength, weight)
        {
            _variable = var;
            _expression = new ClLinearExpression(_variable, -1.0, _variable.Value);
        }

        protected ClEditOrStayConstraint(ClVariable var)
            : this(var, ClStrength.Required, 1.0)
        {
            _variable = var;
        }

        public override string ToString()
        {
            // add missing bracket -> see ClConstraint#ToString(...)
            return base.ToString() + ")";
        }

        public ClVariable Variable
        {
            get { return _variable; }
        }

        public override ClLinearExpression Expression
        {
            get { return _expression; }
        }

        private readonly ClVariable _variable;
        private readonly ClLinearExpression _expression;
    }
}
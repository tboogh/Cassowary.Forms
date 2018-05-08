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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cassowary
{
    public class ClSymbolicWeight
    {
        public ClSymbolicWeight(double w1, double w2, double w3)
        {
            _values = Array.AsReadOnly(new[] { w1, w2, w3 });
        }

        public ClSymbolicWeight(params double[] weights)
        {
            _values = Array.AsReadOnly((double[])weights.Clone());
        }

        protected ClSymbolicWeight(ReadOnlyCollection<double> weights)
        {
            _values = weights;
        }

        protected ClSymbolicWeight(IEnumerable<double> weights)
            : this(weights.ToArray())
        {
        }

        protected virtual ClSymbolicWeight Clone()
        {
            return new ClSymbolicWeight(_values);
        }

        public static ClSymbolicWeight operator *(ClSymbolicWeight clsw, double n)
        {
            return clsw.Times(n);
        }

        public static ClSymbolicWeight operator *(double n, ClSymbolicWeight clsw)
        {
            return clsw.Times(n);
        }

        public ClSymbolicWeight Times(double n)
        {
            return new ClSymbolicWeight(_values.Select(a => a * n).ToArray());
        }

        public static ClSymbolicWeight operator /(ClSymbolicWeight clsw, double n)
        {
            return clsw.DivideBy(n);
        }

        private ClSymbolicWeight DivideBy(double n)
        {
            // Assert(n != 0);

            return new ClSymbolicWeight(_values.Select(a => a / n).ToArray());
        }

        public static ClSymbolicWeight operator +(ClSymbolicWeight clsw1, ClSymbolicWeight clsw2)
        {
            return clsw1.Add(clsw2);
        }

        private ClSymbolicWeight Add(ClSymbolicWeight clsw1)
        {
            // Assert(clws.CLevels == CLevels);

            return new ClSymbolicWeight(_values.Select((a, i) => a + clsw1._values[i]).ToArray());
        }

        public static ClSymbolicWeight operator -(ClSymbolicWeight clsw1, ClSymbolicWeight clsw2)
        {
            return clsw1.Subtract(clsw2);
        }

        private ClSymbolicWeight Subtract(ClSymbolicWeight clsw1)
        {
            // Assert(clsw1.CLevels == CLevels);

            return new ClSymbolicWeight(_values.Select((a, i) => a - clsw1._values[i]).ToArray());
        }

        // TODO: comparison operators (<, <=, >, >=, ==)
        public bool LessThan(ClSymbolicWeight clsw1)
        {
            // Assert(clsw1.CLevels == CLevels);

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] < clsw1._values[i])
                    return true;
                if (_values[i] > clsw1._values[i])
                    return false;
            }

            return false; // they are equal
        }

        public bool LessThanOrEqual(ClSymbolicWeight clsw1)
        {
            // Assert(clsw1.CLevels == CLevels);

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] < clsw1._values[i])
                    return true;
                if (_values[i] > clsw1._values[i])
                    return false;
            }

            return true; // they are equal
        }

        public bool Equal(ClSymbolicWeight clsw1)
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            return !_values.Where((t, i) => t != clsw1._values[i]).Any();
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public bool GreaterThan(ClSymbolicWeight clsw1)
        {
            return !LessThan(clsw1);
        }

        public double AsDouble()
        {
            double sum = 0;
            double factor = 1;
            const double MULTIPLIER = 1000;

            for (var i = _values.Count - 1; i >= 0; i--)
            {
                sum += _values[i] * factor;
                factor *= MULTIPLIER;
            }

            return sum;
        }

        public override string ToString()
        {
            var builder = new StringBuilder('[')
                .Append(string.Join(",", _values.Select(a => a.ToString(CultureInfo.InvariantCulture)).ToArray()))
                .Append("]");

            return builder.ToString();
        }

        public int CLevels
        {
            get { return _values.Count; }
        }

        private readonly ReadOnlyCollection<double> _values;
    }
}
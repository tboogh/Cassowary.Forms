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

namespace Cassowary
{
    public class ClDouble : IEquatable<ClDouble>
    {
        public ClDouble(double val)
        {
            _value = val;
        }

        public ClDouble()
            : this(0.0)
        {
        }

        public virtual ClDouble Clone()
        {
            return new ClDouble(_value);
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override sealed String ToString()
        {
            return Convert.ToString(_value);
        }

        public override sealed bool Equals(Object o)
        {
            if (o == null)
                return false;

            var d = o as ClDouble;
            return d != null && Equals(d);
        }

        public bool Equals(ClDouble o)
        {
            return o.Value.Equals(_value);
        }

        public override sealed int GetHashCode()
        {
            throw new NotSupportedException();
        }
    }
}
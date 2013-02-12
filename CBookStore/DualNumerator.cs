using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBookStore
{
    class DualNumerator
    {
        private object[][] data;
        int current = 0;
        int max = 0;

        public DualNumerator(object[][] data, int maxSize) {
            this.data = data;
            this.max = maxSize - 1;
        }

        public bool HasNext() {
            return current + 1 <= max;
        }

        public bool HasPrevious() {
            return current - 1 >= 0;
        }

        public object[] GetFirst() {
            if (max > 0)
            {
                return data[0];
            }
            return new object[] { };
        }

        public object[] GetLast() {
            return data[max];
        }

        public object[] Next() {
            if (HasNext()) {
                current++;
                return data[current];
            }

            return null;
        }

        public object[] Previous() {
            if (HasPrevious()) {
                current--;
                return data[current];
            }

            return null;
        }

        public object[] GetCurrent() {
            try
            {
                return data[current];
            }
            catch (IndexOutOfRangeException) {
                return new object[] { };
            }
        }
    }
}

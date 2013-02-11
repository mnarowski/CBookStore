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
            this.max = maxSize;
        }

        public bool HasNext() {
            return current + 1 < max;
        }

        public bool hasPrevious() {
            return current - 1 >= 0;
        }

        public object[] getFirst() {
            return data[0];
        }

        public object[] getLast() {
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
            if (hasPrevious()) {
                current--;
                return data[current];
            }

            return null;
        }
    }
}

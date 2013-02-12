﻿using System;
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
            return data[0];
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
            return data[current];
        }
    }
}

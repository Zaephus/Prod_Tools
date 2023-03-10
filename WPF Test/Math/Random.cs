
using System;
using System.Collections;
using System.Collections.Generic;

namespace Prod_Tools {

    public static class Random {

        private static System.Random rand = new System.Random();

        public static float Range(float _minInclusive, float _maxInclusive) {
            float temp = rand.NextSingle();
            temp = temp.Map(0, 1, _minInclusive, _maxInclusive);
            return temp;
        }

    }
    
}
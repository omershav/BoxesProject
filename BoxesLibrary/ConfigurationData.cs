using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public class ConfigurationData
    {
        public int MaxQuantity { get; }
        public int MinQuantity { get; }
        public double DeltaValue { get; }
        public int MaxSplits { get; }
        public int PercentsExceed { get; }
        public int DigitsSensitivity { get; }
        public int CoefficientValue { get; }
        public int FrequencyCheck { get; }
        public int LifeTime { get; }

        public ConfigurationData(int maxQuantity, int minQuantity, int digitsSensitivity, int maxSplits, int percentsExceed, int frequencyCheck, int lifeTime)
        {
            MaxQuantity = maxQuantity; //Maximum quantity of boxes
            MinQuantity = minQuantity; //Minimum quantity of boxes
            DigitsSensitivity = digitsSensitivity; //Maximum fractional digits
            MaxSplits = maxSplits; //Maximum number of splits
            PercentsExceed = percentsExceed; //Max percentage number of box size for gift for calculating the upper bound size
            FrequencyCheck = frequencyCheck; //Number of days between every check
            LifeTime = lifeTime; //After how many days the box is considered old and needed to be deleted
            CoefficientValue = 1 + (percentsExceed / 100); //Calculating the coefficient upper bound value
            DeltaValue = 1 / Math.Pow(10, DigitsSensitivity); //Calculating the delta value for searching a box to gift function
        }
    }
}

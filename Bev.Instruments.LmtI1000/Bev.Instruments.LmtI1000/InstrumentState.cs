using System.Globalization;

namespace Bev.Instruments.LmtI1000
{
    public class InstrumentState
    {
        public InstrumentState()
        {
            ResetState();
        }

        public LmtMode Mode { get; private set; }
        public double CurrentA { get; private set; }
        public double CurrentB { get; private set; }
        public bool OverflowA { get; private set; }
        public bool OverflowB { get; private set; }
        public MeasurementRange RangeA { get; private set; }
        public MeasurementRange RangeB { get; private set; }
        public double Ratio => CurrentA / CurrentB;

        public void ParseString(string line)
        {
            ResetState();
            if (line.Length == 31)
            {
                CurrentA = GetValue(line, 1);
                CurrentB = GetValue(line, 17);
                int statusByteA = GetDigit(line, 14);
                OverflowA = CheckOverflow(statusByteA);
                OverflowB = CheckOverflow(GetDigit(line, 30));
                if (statusByteA == 7) Mode = LmtMode.TwoChannels;
                if (statusByteA == 6) Mode = LmtMode.BlueRed;
                if (statusByteA == 3) Mode = LmtMode.TwoChannels;
                if (statusByteA == 2) Mode = LmtMode.BlueRed;
                RangeA = GetRange(GetDigit(line, 11));
                RangeB = GetRange(GetDigit(line, 27));
                return;
            }
            if (line.Length == 14)
            {
                CurrentA = GetValue(line, 0);
                OverflowA = CheckOverflow(GetDigit(line, 13));
                Mode = LmtMode.SingleChannel;
                RangeA = GetRange(GetDigit(line, 10));
                return;
            }
        }

        public override string ToString()
        {
            string toStr = $"[InstrumentState Mode:{Mode}]";
            string toStrA = $"[Channel A - Current:{CurrentA} Range:{RangeA} Overflow:{OverflowA}]";
            string toStrB = $"[Channel B - Current:{CurrentB} Range:{RangeB} Overflow:{OverflowB}]";
            return $"{toStr}\n{toStrA}\n{toStrB}";
        }

        private MeasurementRange GetRange(int exp)
        {
            switch (exp)
            {
                case 2:
                    return MeasurementRange.Range0;
                case 3:
                    return MeasurementRange.Range1;
                case 4:
                    return MeasurementRange.Range2;
                case 5:
                    return MeasurementRange.Range3;
                case 6:
                    return MeasurementRange.Range4;
                case 7:
                    return MeasurementRange.Range5;
                case 8:
                    return MeasurementRange.Range6;
                case 9:
                    return MeasurementRange.Range7;
                default:
                    return MeasurementRange.Unknown;
            }
        }

        private bool CheckOverflow(int statusByte)
        {
            if (statusByte == 7) return false;
            if (statusByte == 6) return false;
            return true;
        }

        private int GetDigit(string token, int startIndex)
        {
            string subToken = token.Substring(startIndex, 1);
            return int.TryParse(subToken, out int value) ? value : -1; // good old C++ error
        }

        private double GetValue(string token, int startIndex)
        {
            string subToken = token.Substring(startIndex, 11);
            return double.TryParse(subToken, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : double.NaN;
        }

        private void ResetState()
        {
            Mode = LmtMode.Unknown;
            OverflowA = false;
            OverflowB = false;
            CurrentA = double.NaN;
            CurrentB = double.NaN;
            RangeA = MeasurementRange.Unknown;
            RangeB = MeasurementRange.Unknown;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfusionPumpV1
{
    public partial class Form1 : Form
    {

        // Define crisp inputs
        double heartRate = 0;
        double bloodPressure = 0;

        private void calculateFuzzyLogic()
        {
            // 1. FUZZIFICATION (Input Memberships)
            // HeartRate Changes
            double hrLow = TrapezoidalMembership(heartRate, 40, 40, 60, 80);
            double hrNormal = TrapezoidalMembership(heartRate, 60, 80, 100, 120);
            double hrHigh = TrapezoidalMembership(heartRate, 100, 120, 180, 180);

            // BloodPressure Changes
            double bpLow = TrapezoidalMembership(bloodPressure, 60, 60, 80, 100);
            double bpNormal = TrapezoidalMembership(bloodPressure, 90, 105, 125, 140);
            double bpHigh = TrapezoidalMembership(bloodPressure, 130, 145, 180, 180);

            // 2. Rule Evaluation

            // Rule Group: Decrease Pump Rate
            double rule1 = Math.Min(hrLow, bpLow);
            double rule2 = Math.Min(hrLow, bpNormal);
            double rule3 = Math.Min(hrNormal, bpLow);

            // Rule Group: Maintain Pump Rate
            double rule4 = Math.Min(hrLow, bpHigh);
            double rule5 = Math.Min(hrNormal, bpNormal);
            double rule6 = Math.Min(hrHigh, bpLow);

            // Rule Group: Increase Pump Rate
            double rule7 = Math.Min(hrNormal, bpHigh);
            double rule8 = Math.Min(hrHigh, bpNormal);
            double rule9 = Math.Min(hrHigh, bpHigh);

            // Aggregate strengths for each output category
            double strengthDecrease = Math.Max(rule1, Math.Max(rule2, rule4));
            double strengthMaintain = Math.Max(rule3, Math.Max(rule5, rule7));
            double strengthIncrease = Math.Max(rule6, Math.Max(rule8, rule9));

            label4.Text = $"Decrease Pump Rate: {strengthDecrease:F0}";
            label5.Text = $"Maintain Pump Rate: {strengthMaintain:F0}";
            label6.Text = $"Increase Pump Rate: {strengthIncrease:F0}";

            // 3. IMPLICATION, AGGREGATION & DEFUZZIFICATION (Center of Gravity)
            // Output range: 0 to 15 mg
            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 0.1; // Discrete integration steps

            for (double y = 0.0; y <= 15.0; y += step)
            {
                // Define output membership functions for Pump Rate
                double outDecrease = TrapezoidalMembership(y, 0.0, 0.0, 3.0, 6.0);
                double outMaintain = TrapezoidalMembership(y, 4.0, 6.0, 9.0, 11.0);
                double outIncrease = TrapezoidalMembership(y, 9.0, 12.0, 15.0, 15.0);

                // Implication: Clip each output fuzzy set by its rule firing strength using Min
                double clippedDecrease = Math.Min(strengthDecrease, outDecrease);
                double clippedMaintain = Math.Min(strengthMaintain, outMaintain);
                double clippedIncrease = Math.Min(strengthIncrease, outIncrease);

                // Aggregation: Combine all clipped output sets using Max
                double aggregatedY = Math.Max(clippedDecrease, Math.Max(clippedMaintain, clippedIncrease));

                // Accumulate for Center of Gravity calculation
                sumNumerator += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            double crispOutput = 0.0;
            if (sumDenominator > 0.0)
            {
                crispOutput = sumNumerator / sumDenominator;
            }

            label7.Text = $"Calculated Crisp Pump\nDosage Output:\n{crispOutput:F2} mg";
        }

        static double TrapezoidalMembership(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d) return 0.0;
            if (x >= b && x <= c) return 1.0;
            if (x > a && x < b) return (x - a) / (b - a);
            return (d - x) / (d - c);
        }

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            calculateFuzzyLogic();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            // 1. Account for the scrollbar thumb width offset
            int maxScrollableRange = hScrollBar1.Maximum - hScrollBar1.LargeChange + 1;

            // 2. Get percentage as a decimal (0.0 to 1.0)
            double percentRatio = (double)hScrollBar1.Value / maxScrollableRange;

            // 3. Clamp between 0.0 and 1.0 to prevent overrun
            percentRatio = Math.Min(1.0, Math.Max(0.0, percentRatio));

            // 4. Map to 40 - 180 range: Start at 40, add up to 140
            double valueFrom40To180 = 40.0 + (percentRatio * 140.0);

            // Display or use the value
            heartRate = valueFrom40To180;
            label2.Text = $"HeartRate: {heartRate:F0}";

            //Triggers Update || DO NOT REMOVE PLEASE
            calculateFuzzyLogic();
        }
        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            // 1. Account for the scrollbar thumb width offset
            int maxScrollableRange = hScrollBar2.Maximum - hScrollBar2.LargeChange + 1;

            // 2. Get percentage as a decimal (0.0 to 1.0)
            double percentRatio = (double)hScrollBar2.Value / maxScrollableRange;

            // 3. Clamp between 0.0 and 1.0 to prevent overrun
            percentRatio = Math.Min(1.0, Math.Max(0.0, percentRatio));

            // 4. Map to 40 - 180 range: Start at 40, add up to 140
            double valueFrom60To180 = 60.0 + (percentRatio * 120.0);

            // Display or use the value
            bloodPressure = valueFrom60To180;
            label3.Text = $"Blood Pressure: {bloodPressure:F0}";

            //Triggers Update || DO NOT REMOVE PLEASE
            calculateFuzzyLogic();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

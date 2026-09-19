using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestTest
{
    public partial class Form1 : Form
    {
        // UI Components
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Button startPumpBtn;

        // GroupBoxes
        private GroupBox gbOverview;
        private GroupBox gbInputs;
        private GroupBox gbFuzzification;
        private GroupBox gbRules;
        private GroupBox gbDefuzzification;
        private GroupBox gbVisualizations;

        // Input Controls
        private TrackBar trackHR;
        private TrackBar trackBP;
        private Label lblHRVal;
        private Label lblBPVal;

        // Fuzzification Controls
        private ProgressBar pbHRLow;
        private ProgressBar pbHRNormal;
        private ProgressBar pbBPLow;
        private ProgressBar pbBPNormal;

        // Rule Base Grid
        private DataGridView dgvRules;

        // Output Display
        private Label lblResultDisplay;

        // Visualization Tabs
        private TabControl tabControlVis;

        // Theme Colors
        private Color themeWhite = Color.White;
        private Color themeText = Color.Black;
        private Color themeBlueAccent = Color.FromArgb(0, 85, 170);
        private Color themeScreenBlue = Color.FromArgb(74, 144, 226);

        public Form1()
        {
            InitializeComponent();
            SetupFormProperties();
            BuildMenuBar();
            BuildStatusStrip();
            BuildLayoutPanels();
            PopulateOverviewSection();
            PopulateInputsSection();
            PopulateFuzzificationSection();
            PopulateRulesSection();
            PopulateDefuzzificationSection();
            PopulateVisualizationsSection();

            // Run initial calculation
            CalculateFuzzyLogic();
        }

        private void SetupFormProperties()
        {
            this.Text = "Mamdani Fuzzy Logic Infusion Pump - System Demo";
            this.Size = new Size(1280, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = themeWhite;
            this.ForeColor = themeText;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void BuildMenuBar()
        {
            menuStrip = new MenuStrip
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = themeText
            };

            ToolStripMenuItem menuFile = new ToolStripMenuItem("File");
            ToolStripMenuItem menuSim = new ToolStripMenuItem("Simulation");
            ToolStripMenuItem menuHelp = new ToolStripMenuItem("Help");

            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuSim, menuHelp });
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void BuildStatusStrip()
        {
            statusStrip = new StatusStrip
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = themeText
            };

            statusLabel = new ToolStripStatusLabel
            {
                Text = "Status: Pump Active - Monitoring Vitals",
                Alignment = ToolStripItemAlignment.Left
            };

            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void BuildLayoutPanels()
        {
            int topMargin = 30;
            int height = 265;

            gbOverview = CreateGroupBox("System Overview & Protocol", new Point(10, topMargin), new Size(220, height));
            gbInputs = CreateGroupBox("Patient Vitals (Inputs)", new Point(240, topMargin), new Size(160, height));
            gbFuzzification = CreateGroupBox("Fuzzification (Memberships)", new Point(410, topMargin), new Size(170, height));
            gbRules = CreateGroupBox("Mamdani Rule Base (3x3 Matrix)", new Point(590, topMargin), new Size(310, 135));
            gbDefuzzification = CreateGroupBox("Defuzzification & Dosage", new Point(590, topMargin + 140), new Size(310, 125));
            gbVisualizations = CreateGroupBox("Visualizations", new Point(910, topMargin), new Size(345, height));

            this.Controls.AddRange(new Control[] {
                gbOverview, gbInputs, gbFuzzification, gbRules, gbDefuzzification, gbVisualizations
            });
        }

        private GroupBox CreateGroupBox(string title, Point location, Size size)
        {
            return new GroupBox
            {
                Text = title,
                Location = location,
                Size = size,
                ForeColor = themeBlueAccent,
                Font = new Font("Segoe UI", 8.25F, FontStyle.Bold)
            };
        }

        private void PopulateOverviewSection()
        {
            Label lblDesc = new Label
            {
                Text = "Problem Statement:\nAutomated control system regulating a medical drug pump based on real-time HR and BP to prevent patient trauma.\n\n" +
                       "Fuzzy Sustainability:\n- Eliminates 'Bang-Bang' control transitions.\n" +
                       "- Handles vague biological overlap safely.\n" +
                       "- Resolves conflicting rules (e.g., normal HR but low BP) using a Mamdani inference engine by mathematically weighing active conditions.",
                Location = new Point(10, 20),
                Size = new Size(200, 230),
                ForeColor = themeText,
                Font = new Font("Segoe UI", 8.0F, FontStyle.Regular)
            };
            gbOverview.Controls.Add(lblDesc);
        }

        //Heartbeat & Bloodpressure Input Slider
        private void PopulateInputsSection()
        {
            Label lblHR = new Label { Text = "Heart Rate (bpm)", Location = new Point(10, 18), AutoSize = true, ForeColor = themeText, Font = new Font("Segoe UI", 8F) };
            trackHR = new TrackBar { Location = new Point(5, 35), Size = new Size(110, 45), Minimum = 40, Maximum = 180, Value = 85, TickFrequency = 20 };
            lblHRVal = new Label { Text = "85 bpm", Location = new Point(115, 40), AutoSize = true, ForeColor = themeText };
            Label lblHRMinMax = new Label { Text = "40                         180", Location = new Point(10, 75), Size = new Size(140, 15), Font = new Font("Segoe UI", 7F), ForeColor = Color.Gray };

            Label lblBP = new Label { Text = "Blood Pressure (mmHg)", Location = new Point(10, 100), AutoSize = true, ForeColor = themeText, Font = new Font("Segoe UI", 8F) };
            trackBP = new TrackBar { Location = new Point(5, 120), Size = new Size(110, 45), Minimum = 60, Maximum = 180, Value = 115, TickFrequency = 20 };
            lblBPVal = new Label { Text = "115 mmHg", Location = new Point(115, 125), AutoSize = true, ForeColor = themeText };
            Label lblBPMinMax = new Label { Text = "60                         180", Location = new Point(10, 160), Size = new Size(140, 15), Font = new Font("Segoe UI", 7F), ForeColor = Color.Gray };

            trackHR.ValueChanged += (s, e) => { lblHRVal.Text = $"{trackHR.Value} bpm"; CalculateFuzzyLogic(); };
            trackBP.ValueChanged += (s, e) => { lblBPVal.Text = $"{trackBP.Value} mmHg"; CalculateFuzzyLogic(); };

            gbInputs.Controls.AddRange(new Control[] {
                lblHR, trackHR, lblHRVal, lblHRMinMax,
                lblBP, trackBP, lblBPVal, lblBPMinMax
            });
        }

        private void PopulateFuzzificationSection()
        {
            Label lblH1 = new Label { Text = "HR [Low: 0.65, Norm: 0.35]", Location = new Point(10, 18), AutoSize = true, Font = new Font("Segoe UI", 7.5F), ForeColor = themeText };
            pbHRLow = new ProgressBar { Location = new Point(10, 35), Size = new Size(145, 15), Value = 65 };

            Label lblH2 = new Label { Text = "HR [Norm: 0.35, High: 0.0]", Location = new Point(10, 70), AutoSize = true, Font = new Font("Segoe UI", 7.5F), ForeColor = themeText };
            pbHRNormal = new ProgressBar { Location = new Point(10, 88), Size = new Size(145, 15), Value = 35 };

            Label lblB1 = new Label { Text = "BP [Low: 0.0, Norm: 0.90]", Location = new Point(10, 125), AutoSize = true, Font = new Font("Segoe UI", 7.5F), ForeColor = themeText };
            pbBPNormal = new ProgressBar { Location = new Point(10, 143), Size = new Size(145, 15), Value = 90 };

            gbFuzzification.Controls.AddRange(new Control[] { lblH1, pbHRLow, lblH2, pbHRNormal, lblB1, pbBPNormal });
        }

        private void PopulateRulesSection()
        {
            dgvRules = new DataGridView
            {
                Location = new Point(5, 20),
                Size = new Size(300, 105),
                BackgroundColor = Color.WhiteSmoke,
                ForeColor = themeText,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvRules.Columns.Add("RuleNum", "Rule #");
            dgvRules.Columns.Add("RuleDesc", "Condition & Action");
            dgvRules.Columns.Add("Firing", "Strength");

            dgvRules.Rows.Add("Rule 1", "IF HR Low AND BP Low THEN Decrease", "0.00");
            dgvRules.Rows.Add("Rule 2", "IF HR Low AND BP Norm THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 3", "IF HR Low AND BP High THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 4", "IF HR Norm AND BP Low THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 5", "IF HR Norm AND BP Norm THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 6", "IF HR Norm AND BP High THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 7", "IF HR High AND BP Low THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 8", "IF HR High AND BP Norm THEN Maintain", "0.00");
            dgvRules.Rows.Add("Rule 9", "IF HR High AND BP High THEN Increase", "0.00");

            gbRules.Controls.Add(dgvRules);
        }

        private void PopulateDefuzzificationSection()
        {
            Label lblDec = new Label { Text = "Decrease: 0-6 mg", Location = new Point(10, 20), AutoSize = true, ForeColor = themeText, Font = new Font("Segoe UI", 8F) };
            Label lblMain = new Label { Text = "Maintain: 4-11 mg", Location = new Point(10, 45), AutoSize = true, ForeColor = themeText, Font = new Font("Segoe UI", 8F) };
            Label lblInc = new Label { Text = "Increase: 9-15 mg", Location = new Point(10, 70), AutoSize = true, ForeColor = themeText, Font = new Font("Segoe UI", 8F) };

            Label lblCrisp = new Label { Text = "Centroid Output Dosage:", Location = new Point(135, 20), AutoSize = true, ForeColor = themeText };

            Panel resultPanel = new Panel
            {
                Location = new Point(135, 40),
                Size = new Size(165, 50),
                BackColor = themeScreenBlue
            };

            lblResultDisplay = new Label
            {
                Text = "7.2 mg/min",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            resultPanel.Controls.Add(lblResultDisplay);

            startPumpBtn = new Button
            {
                Text = "Activate Pump",
                Location = new Point(210, 95),
                Size = new Size(90, 23),
                BackColor = themeBlueAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startPumpBtn.FlatAppearance.BorderSize = 0;

            gbDefuzzification.Controls.AddRange(new Control[] { lblDec, lblMain, lblInc, lblCrisp, resultPanel, startPumpBtn });
        }

        private void PopulateVisualizationsSection()
        {
            tabControlVis = new TabControl
            {
                Location = new Point(10, 20),
                Size = new Size(325, 230)
            };

            TabPage tab1 = new TabPage("Input MFs (Trapezoidal)");
            TabPage tab2 = new TabPage("Output Aggregation");
            TabPage tab3 = new TabPage("3D Control Surface");

            tab1.BackColor = Color.WhiteSmoke;
            tab2.BackColor = Color.WhiteSmoke;
            tab3.BackColor = Color.WhiteSmoke;

            Button btn3D = new Button
            {
                Text = "Render Surface Plot",
                Location = new Point(100, 100),
                Size = new Size(120, 30),
                BackColor = themeBlueAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn3D.FlatAppearance.BorderSize = 0;
            tab3.Controls.Add(btn3D);

            tabControlVis.TabPages.Add(tab1);
            tabControlVis.TabPages.Add(tab2);
            tabControlVis.TabPages.Add(tab3);

            gbVisualizations.Controls.Add(tabControlVis);
        }

        private void CalculateFuzzyLogic()
        {
            if (trackHR == null || trackBP == null || lblResultDisplay == null) return;

            double hr = trackHR.Value;
            double bp = trackBP.Value;

            // 1. FUZZIFICATION
            double hrLow = TrapezoidalMembership(hr, 0, 0, 60, 80);
            double hrNorm = TrapezoidalMembership(hr, 60, 80, 100, 120);
            double hrHigh = TrapezoidalMembership(hr, 100, 120, 180, 200);

            double bpLow = TrapezoidalMembership(bp, 0, 0, 80, 100);
            double bpNorm = TrapezoidalMembership(bp, 90, 105, 125, 140);
            double bpHigh = TrapezoidalMembership(bp, 130, 145, 180, 200);

            // Update UI Fuzzification Progress Bars
            if (pbHRLow != null) pbHRLow.Value = (int)(hrLow * 100);
            if (pbHRNormal != null) pbHRNormal.Value = (int)(hrNorm * 100);
            if (pbBPNormal != null) pbBPNormal.Value = (int)(bpNorm * 100);

            // 2. RULE EVALUATION (Mamdani AND = Math.Min)
            double r1 = Math.Min(hrLow, bpLow);   // Decrease
            double r2 = Math.Min(hrLow, bpNorm);  // Maintain
            double r3 = Math.Min(hrLow, bpHigh);  // Maintain
            double r4 = Math.Min(hrNorm, bpLow);  // Maintain
            double r5 = Math.Min(hrNorm, bpNorm); // Maintain
            double r6 = Math.Min(hrNorm, bpHigh); // Maintain
            double r7 = Math.Min(hrHigh, bpLow);  // Maintain
            double r8 = Math.Min(hrHigh, bpNorm); // Maintain
            double r9 = Math.Min(hrHigh, bpHigh); // Increase

            if (dgvRules != null && dgvRules.Rows.Count == 9)
            {
                dgvRules.Rows[0].Cells[2].Value = r1.ToString("F2");
                dgvRules.Rows[1].Cells[2].Value = r2.ToString("F2");
                dgvRules.Rows[2].Cells[2].Value = r3.ToString("F2");
                dgvRules.Rows[3].Cells[2].Value = r4.ToString("F2");
                dgvRules.Rows[4].Cells[2].Value = r5.ToString("F2");
                dgvRules.Rows[5].Cells[2].Value = r6.ToString("F2");
                dgvRules.Rows[6].Cells[2].Value = r7.ToString("F2");
                dgvRules.Rows[7].Cells[2].Value = r8.ToString("F2");
                dgvRules.Rows[8].Cells[2].Value = r9.ToString("F2");
            }

            // 3. AGGREGATION (Mamdani OR = Math.Max)
            double strengthDecrease = r1;
            double strengthMaintain = Math.Max(r2, Math.Max(r3, Math.Max(r4, Math.Max(r5, Math.Max(r6, Math.Max(r7, r8))))));
            double strengthIncrease = r9;

            // 4. DEFUZZIFICATION (Centroid)
            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 0.1;

            for (double y = 0.0; y <= 15.0; y += step)
            {
                double outDec = TrapezoidalMembership(y, 0, 0, 4, 6);
                double outMain = TrapezoidalMembership(y, 4, 6, 9, 11);
                double outInc = TrapezoidalMembership(y, 9, 11, 15, 15);

                double clipDec = Math.Min(strengthDecrease, outDec);
                double clipMain = Math.Min(strengthMaintain, outMain);
                double clipInc = Math.Min(strengthIncrease, outInc);

                double aggregatedY = Math.Max(clipDec, Math.Max(clipMain, clipInc));

                sumNumerator += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            double crispOutput = 0.0;
            if (sumDenominator > 0.0)
            {
                crispOutput = sumNumerator / sumDenominator;
            }

            // Display Output
            lblResultDisplay.Text = $"{crispOutput:F1} mg/min";
        }

        private double TrapezoidalMembership(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d)
                return 0.0;
            if (x >= b && x <= c)
                return 1.0;
            if (x > a && x < b)
                return (x - a) / (b - a);
            return (d - x) / (d - c);
        }
    }
}
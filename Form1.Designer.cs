namespace InfusionPumpV1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // inside panel1
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageInputs = new System.Windows.Forms.TabPage();
            this.gbOverview = new System.Windows.Forms.GroupBox();
            this.lblOverview = new System.Windows.Forms.Label();
            this.gbScenarios = new System.Windows.Forms.GroupBox();
            this.btnScenarioHypertensive = new System.Windows.Forms.Button();
            this.btnScenarioBradycardia = new System.Windows.Forms.Button();
            this.btnScenarioStable = new System.Windows.Forms.Button();
            this.gbInputs = new System.Windows.Forms.GroupBox();
            this.pbBloodPressure = new InfusionPumpV1.MembershipChartPanel();
            this.pbHeartRate = new InfusionPumpV1.MembershipChartPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.tabPageRules = new System.Windows.Forms.TabPage();
            this.gbRules = new System.Windows.Forms.GroupBox();
            this.dgvRules = new System.Windows.Forms.DataGridView();
            this.colRuleNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRuleDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFiring = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.gbDefuzz = new System.Windows.Forms.GroupBox();
            this.pbPumpRate = new InfusionPumpV1.MembershipChartPanel();
            this.label7 = new System.Windows.Forms.Label();
            // outside panel1
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();

            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageInputs.SuspendLayout();
            this.gbOverview.SuspendLayout();
            this.gbScenarios.SuspendLayout();
            this.gbInputs.SuspendLayout();
            this.tabPageRules.SuspendLayout();
            this.gbRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).BeginInit();
            this.tabPageOutput.SuspendLayout();
            this.gbDefuzz.SuspendLayout();
            this.SuspendLayout();

            // tabControl1
            this.tabControl1.Controls.Add(this.tabPageInputs);
            this.tabControl1.Controls.Add(this.tabPageRules);
            this.tabControl1.Controls.Add(this.tabPageOutput);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.TabIndex = 0;

            // tabPageInputs
            this.tabPageInputs.Controls.Add(this.gbInputs);
            this.tabPageInputs.Controls.Add(this.gbScenarios);
            this.tabPageInputs.Controls.Add(this.gbOverview);
            this.tabPageInputs.Name = "tabPageInputs";
            this.tabPageInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInputs.TabIndex = 0;
            this.tabPageInputs.Text = "1. Inputs & Scenarios";
            this.tabPageInputs.UseVisualStyleBackColor = true;

            // gbOverview
            this.gbOverview.Controls.Add(this.lblOverview);
            this.gbOverview.Location = new System.Drawing.Point(15, 15);
            this.gbOverview.Name = "gbOverview";
            this.gbOverview.Size = new System.Drawing.Size(350, 150);
            this.gbOverview.TabIndex = 0;
            this.gbOverview.TabStop = false;
            this.gbOverview.Text = "System Overview & Problem Statement";

            // lblOverview
            this.lblOverview.Location = new System.Drawing.Point(15, 30);
            this.lblOverview.Name = "lblOverview";
            this.lblOverview.Size = new System.Drawing.Size(315, 105);
            this.lblOverview.TabIndex = 0;
            this.lblOverview.Text = "Fuzzy logic handles the vagueness and non-linear relationship between vital signs (HR, BP) to determine safe, continuous adjustments to the infusion pump rate.\r\n\r\nInputs: Heart Rate, Blood Pressure\r\nOutput: Pump Dosage Rate";

            // gbScenarios
            this.gbScenarios.Controls.Add(this.btnScenarioHypertensive);
            this.gbScenarios.Controls.Add(this.btnScenarioBradycardia);
            this.gbScenarios.Controls.Add(this.btnScenarioStable);
            this.gbScenarios.Location = new System.Drawing.Point(15, 180);
            this.gbScenarios.Name = "gbScenarios";
            this.gbScenarios.Size = new System.Drawing.Size(350, 200);
            this.gbScenarios.TabIndex = 1;
            this.gbScenarios.TabStop = false;
            this.gbScenarios.Text = "Edge Case / Scenario Testing";

            // btnScenarioHypertensive
            this.btnScenarioHypertensive.Location = new System.Drawing.Point(20, 130);
            this.btnScenarioHypertensive.Name = "btnScenarioHypertensive";
            this.btnScenarioHypertensive.Size = new System.Drawing.Size(310, 40);
            this.btnScenarioHypertensive.TabIndex = 2;
            this.btnScenarioHypertensive.Text = "Hypertensive Crisis (High BP)";
            this.btnScenarioHypertensive.UseVisualStyleBackColor = true;
            this.btnScenarioHypertensive.Click += new System.EventHandler(this.btnScenarioHypertensive_Click);

            // btnScenarioBradycardia
            this.btnScenarioBradycardia.Location = new System.Drawing.Point(20, 80);
            this.btnScenarioBradycardia.Name = "btnScenarioBradycardia";
            this.btnScenarioBradycardia.Size = new System.Drawing.Size(310, 40);
            this.btnScenarioBradycardia.TabIndex = 1;
            this.btnScenarioBradycardia.Text = "Extreme Bradycardia (Low HR)";
            this.btnScenarioBradycardia.UseVisualStyleBackColor = true;
            this.btnScenarioBradycardia.Click += new System.EventHandler(this.btnScenarioBradycardia_Click);

            // btnScenarioStable
            this.btnScenarioStable.Location = new System.Drawing.Point(20, 30);
            this.btnScenarioStable.Name = "btnScenarioStable";
            this.btnScenarioStable.Size = new System.Drawing.Size(310, 40);
            this.btnScenarioStable.TabIndex = 0;
            this.btnScenarioStable.Text = "Stable Patient (Normal Vitals)";
            this.btnScenarioStable.UseVisualStyleBackColor = true;
            this.btnScenarioStable.Click += new System.EventHandler(this.btnScenarioStable_Click);

            // gbInputs
            this.gbInputs.Controls.Add(this.pbBloodPressure);
            this.gbInputs.Controls.Add(this.pbHeartRate);
            this.gbInputs.Controls.Add(this.label3);
            this.gbInputs.Controls.Add(this.label2);
            this.gbInputs.Controls.Add(this.hScrollBar2);
            this.gbInputs.Controls.Add(this.hScrollBar1);
            this.gbInputs.Location = new System.Drawing.Point(380, 15);
            this.gbInputs.Name = "gbInputs";
            this.gbInputs.Size = new System.Drawing.Size(510, 545);
            this.gbInputs.TabIndex = 2;
            this.gbInputs.TabStop = false;
            this.gbInputs.Text = "Fuzzification (Input Membership Degrees)";

            // label2
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 30);
            this.label2.Name = "label2";
            this.label2.TabIndex = 6;
            this.label2.Text = "Heart Rate: 40";
            this.label2.Click += new System.EventHandler(this.label2_Click);

            // hScrollBar1
            this.hScrollBar1.Location = new System.Drawing.Point(17, 58);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(475, 25);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);

            // pbHeartRate
            this.pbHeartRate.ChartType = InfusionPumpV1.ChartVariableType.HeartRate;
            this.pbHeartRate.CurrentValue = 40D;
            this.pbHeartRate.InputMax = 180D;
            this.pbHeartRate.InputMin = 40D;
            this.pbHeartRate.Location = new System.Drawing.Point(17, 90);
            this.pbHeartRate.Name = "pbHeartRate";
            this.pbHeartRate.Size = new System.Drawing.Size(475, 165);
            this.pbHeartRate.TabIndex = 8;

            // label3
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 270);
            this.label3.Name = "label3";
            this.label3.TabIndex = 7;
            this.label3.Text = "Blood Pressure: 60";
            this.label3.Click += new System.EventHandler(this.label3_Click);

            // hScrollBar2
            this.hScrollBar2.Location = new System.Drawing.Point(17, 298);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(475, 25);
            this.hScrollBar2.TabIndex = 5;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);

            // pbBloodPressure
            this.pbBloodPressure.ChartType = InfusionPumpV1.ChartVariableType.BloodPressure;
            this.pbBloodPressure.CurrentValue = 60D;
            this.pbBloodPressure.InputMax = 180D;
            this.pbBloodPressure.InputMin = 60D;
            this.pbBloodPressure.Location = new System.Drawing.Point(17, 330);
            this.pbBloodPressure.Name = "pbBloodPressure";
            this.pbBloodPressure.Size = new System.Drawing.Size(475, 195);
            this.pbBloodPressure.TabIndex = 9;

            // tabPageRules
            this.tabPageRules.Controls.Add(this.gbRules);
            this.tabPageRules.Name = "tabPageRules";
            this.tabPageRules.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRules.TabIndex = 1;
            this.tabPageRules.Text = "2. Fuzzy Rules";
            this.tabPageRules.UseVisualStyleBackColor = true;

            // gbRules
            this.gbRules.Controls.Add(this.dgvRules);
            this.gbRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRules.Name = "gbRules";
            this.gbRules.TabIndex = 3;
            this.gbRules.TabStop = false;
            this.gbRules.Text = "Fuzzy Rule Base & Inference";

            // dgvRules
            this.dgvRules.AllowUserToAddRows = false;
            this.dgvRules.AllowUserToDeleteRows = false;
            this.dgvRules.AllowUserToResizeColumns = false;
            this.dgvRules.AllowUserToResizeRows = false;
            this.dgvRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRules.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRuleNum,
            this.colRuleDesc,
            this.colFiring});
            this.dgvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRules.Location = new System.Drawing.Point(3, 18);
            this.dgvRules.Name = "dgvRules";
            this.dgvRules.ReadOnly = true;
            this.dgvRules.RowHeadersVisible = false;
            this.dgvRules.RowHeadersWidth = 51;
            this.dgvRules.RowTemplate.Height = 30;
            this.dgvRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRules.TabIndex = 0;

            // colRuleNum
            this.colRuleNum.FillWeight = 10F;
            this.colRuleNum.HeaderText = "Rule #";
            this.colRuleNum.MinimumWidth = 6;
            this.colRuleNum.Name = "colRuleNum";
            this.colRuleNum.ReadOnly = true;

            // colRuleDesc
            this.colRuleDesc.FillWeight = 75F;
            this.colRuleDesc.HeaderText = "Rule Description";
            this.colRuleDesc.MinimumWidth = 6;
            this.colRuleDesc.Name = "colRuleDesc";
            this.colRuleDesc.ReadOnly = true;

            // colFiring
            this.colFiring.FillWeight = 15F;
            this.colFiring.HeaderText = "Firing Strength";
            this.colFiring.MinimumWidth = 6;
            this.colFiring.Name = "colFiring";
            this.colFiring.ReadOnly = true;

            // tabPageOutput
            this.tabPageOutput.Controls.Add(this.gbDefuzz);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOutput.TabIndex = 2;
            this.tabPageOutput.Text = "3. Output & Defuzzification";
            this.tabPageOutput.UseVisualStyleBackColor = true;

            // gbDefuzz
            this.gbDefuzz.Controls.Add(this.label7);
            this.gbDefuzz.Controls.Add(this.pbPumpRate);
            this.gbDefuzz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDefuzz.Name = "gbDefuzz";
            this.gbDefuzz.TabIndex = 4;
            this.gbDefuzz.TabStop = false;
            this.gbDefuzz.Text = "Defuzzification & Output";

            // pbPumpRate
            this.pbPumpRate.ChartType = InfusionPumpV1.ChartVariableType.PumpRate;
            this.pbPumpRate.CurrentValue = 0D;
            this.pbPumpRate.InputMax = 15D;
            this.pbPumpRate.InputMin = 0D;
            this.pbPumpRate.Location = new System.Drawing.Point(20, 40);
            this.pbPumpRate.Name = "pbPumpRate";
            this.pbPumpRate.Size = new System.Drawing.Size(830, 310);
            this.pbPumpRate.TabIndex = 0;

            // label7
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 370);
            this.label7.Name = "label7";
            this.label7.TabIndex = 1;
            this.label7.Text = "Final Crisp Pump Rate: 0.00 mg/h";

            // panel1
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(20, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(920, 630);
            this.panel1.TabIndex = 1;

            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 12);
            this.label1.Name = "label1";
            this.label1.TabIndex = 2;
            this.label1.Text = "Infusion Pump";
            this.label1.Click += new System.EventHandler(this.label1_Click);

            // button8 (Mute) top-right
            this.button8.Location = new System.Drawing.Point(862, 5);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 32);
            this.button8.TabIndex = 9;
            this.button8.Text = "Mute";
            this.button8.UseVisualStyleBackColor = true;

            // label4/5/6 – status row
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 684);
            this.label4.Name = "label4";
            this.label4.TabIndex = 12;
            this.label4.Text = "Decrease: 0.00";

            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 684);
            this.label5.Name = "label5";
            this.label5.TabIndex = 13;
            this.label5.Text = "Maintain: 0.00";

            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(420, 684);
            this.label6.Name = "label6";
            this.label6.TabIndex = 14;
            this.label6.Text = "Increase: 0.00";

            // Hardware buttons – bottom row
            this.button10.Location = new System.Drawing.Point(35, 710);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 50);
            this.button10.TabIndex = 11;
            this.button10.Text = "On / Off";
            this.button10.UseVisualStyleBackColor = true;

            this.button9.Location = new System.Drawing.Point(116, 710);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 50);
            this.button9.TabIndex = 10;
            this.button9.Text = "Bolus";
            this.button9.UseVisualStyleBackColor = true;

            this.button5.Location = new System.Drawing.Point(238, 710);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 50);
            this.button5.TabIndex = 6;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;

            this.button4.Location = new System.Drawing.Point(319, 710);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 50);
            this.button4.TabIndex = 5;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;

            this.button3.Location = new System.Drawing.Point(433, 710);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 50);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;

            this.button2.Location = new System.Drawing.Point(514, 710);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 50);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;

            this.button1.Location = new System.Drawing.Point(626, 710);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 50);
            this.button1.TabIndex = 1;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            this.button6.Location = new System.Drawing.Point(751, 710);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 50);
            this.button6.TabIndex = 7;
            this.button6.Text = "Stop";
            this.button6.UseVisualStyleBackColor = true;

            this.button7.Location = new System.Drawing.Point(832, 710);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 50);
            this.button7.TabIndex = 8;
            this.button7.Text = "Menu / Exit";
            this.button7.UseVisualStyleBackColor = true;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 775);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Fuzzy Logic Infusion Pump";

            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageInputs.ResumeLayout(false);
            this.gbOverview.ResumeLayout(false);
            this.gbScenarios.ResumeLayout(false);
            this.gbInputs.ResumeLayout(false);
            this.gbInputs.PerformLayout();
            this.tabPageRules.ResumeLayout(false);
            this.gbRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).EndInit();
            this.tabPageOutput.ResumeLayout(false);
            this.gbDefuzz.ResumeLayout(false);
            this.gbDefuzz.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // inside panel1
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageInputs;
        private System.Windows.Forms.GroupBox gbOverview;
        private System.Windows.Forms.Label lblOverview;
        private System.Windows.Forms.GroupBox gbScenarios;
        private System.Windows.Forms.Button btnScenarioStable;
        private System.Windows.Forms.Button btnScenarioHypertensive;
        private System.Windows.Forms.Button btnScenarioBradycardia;
        private System.Windows.Forms.GroupBox gbInputs;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private InfusionPumpV1.MembershipChartPanel pbHeartRate;
        private InfusionPumpV1.MembershipChartPanel pbBloodPressure;
        private System.Windows.Forms.TabPage tabPageRules;
        private System.Windows.Forms.GroupBox gbRules;
        private System.Windows.Forms.DataGridView dgvRules;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRuleNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRuleDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFiring;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.GroupBox gbDefuzz;
        private InfusionPumpV1.MembershipChartPanel pbPumpRate;
        private System.Windows.Forms.Label label7;
        // outside panel1
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}

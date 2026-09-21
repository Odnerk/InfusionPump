using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace InfusionPumpV1
{
    public struct MembershipTriple
    {
        public double Low;
        public double Normal;
        public double High;

        public MembershipTriple(double low, double normal, double high)
        {
            Low    = low;
            Normal = normal;
            High   = high;
        }
    }

    public enum ChartVariableType
    {
        HeartRate,
        BloodPressure,
        PumpRate
    }

    public sealed class MembershipChartPanel : Panel
    {
        private static readonly Color C_BG   = SystemColors.Control;
        private static readonly Color C_GRID = Color.FromArgb(210, 210, 210);
        private static readonly Color C_AXIS = Color.FromArgb(120, 120, 120);
        private static readonly Color C_TXT  = Color.FromArgb(80,  80,  80);

        private static readonly Color C_LO_F = Color.FromArgb(60,  70, 130, 220);
        private static readonly Color C_LO_L = Color.FromArgb(50,  90, 180);
        private static readonly Color C_NM_F = Color.FromArgb(60,  40, 160,  60);
        private static readonly Color C_NM_L = Color.FromArgb(30, 140,  30);
        private static readonly Color C_HI_F = Color.FromArgb(60, 210,  50,  50);
        private static readonly Color C_HI_L = Color.FromArgb(190,  40,  40);

        private const int ML = 40, MR = 12, MT = 20, MB = 24;

        public ChartVariableType ChartType { get; set; } = ChartVariableType.HeartRate;
        
        public double InputMin { get; set; } = 40.0;
        public double InputMax { get; set; } = 180.0;
        public double CurrentValue { get; set; } = 40.0;
        
        // For Inputs: Stores fuzzy membership (mu) of the CurrentValue
        // For Outputs: Stores the rule firing strengths to clip the sets
        public MembershipTriple Membership { get; set; }

        public MembershipChartPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            BackColor = C_BG;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int cw = Width - ML - MR;
            int ch = Height - MT - MB;
            if (cw <= 0 || ch <= 0) return;

            g.Clear(C_BG);
            DrawGrid(g, cw, ch);
            DrawAxes(g, cw, ch);

            g.SetClip(new Rectangle(ML, MT, cw, ch));

            if (ChartType == ChartVariableType.HeartRate)
            {
                DrawTrap(g, cw, ch,  40,  40,  60,  80, C_LO_F, C_LO_L, "Low");
                DrawTrap(g, cw, ch,  60,  75, 105, 120, C_NM_F, C_NM_L, "Normal");
                DrawTrap(g, cw, ch, 100, 130, 180, 180, C_HI_F, C_HI_L, "High");
            }
            else if (ChartType == ChartVariableType.BloodPressure)
            {
                DrawTrap(g, cw, ch,  60,  60,  80, 100, C_LO_F, C_LO_L, "Low");
                DrawTrap(g, cw, ch,  90, 100, 125, 140, C_NM_F, C_NM_L, "Normal");
                DrawTrap(g, cw, ch, 130, 155, 180, 180, C_HI_F, C_HI_L, "High");
            }
            else if (ChartType == ChartVariableType.PumpRate)
            {
                // Draw clipped trapezoids based on firing strengths
                DrawClippedTrap(g, cw, ch, 0, 0, 3, 6, Membership.Low, C_LO_F, C_LO_L, "Dec");
                DrawClippedTrap(g, cw, ch, 4, 6, 9, 11, Membership.Normal, C_NM_F, C_NM_L, "Main");
                DrawClippedTrap(g, cw, ch, 9, 12, 15, 15, Membership.High, C_HI_F, C_HI_L, "Inc");
                
                // Draw the aggregated shape outline
                DrawAggregatedOutline(g, cw, ch);
            }

            g.ResetClip();
            
            if (ChartType == ChartVariableType.PumpRate)
                DrawOutputCentroid(g, cw, ch);
            else
                DrawCursor(g, cw, ch);
        }

        private void DrawGrid(Graphics g, int cw, int ch)
        {
            using (var pen = new Pen(C_GRID, 1f))
            {
                for (int s = 1; s <= 4; s++)
                {
                    int py = MT + ch - (int)(s * 0.25 * ch);
                    g.DrawLine(pen, ML, py, ML + cw, py);
                }
                double range = InputMax - InputMin;
                double step = GridStep();
                for (double v = Math.Ceiling(InputMin / step) * step; v <= InputMax; v += step)
                {
                    int px = ML + (int)((v - InputMin) / range * cw);
                    g.DrawLine(pen, px, MT, px, MT + ch);
                }
            }
        }

        private double GridStep() 
        {
            double r = InputMax - InputMin;
            if (r > 100) return 20;
            if (r > 50) return 10;
            if (r > 10) return 2;
            return 1;
        }

        private void DrawAxes(Graphics g, int cw, int ch)
        {
            using (var pen = new Pen(C_AXIS, 1f))
            {
                g.DrawLine(pen, ML, MT, ML, MT + ch);
                g.DrawLine(pen, ML, MT + ch, ML + cw, MT + ch);
            }

            using (var font = new Font("Segoe UI", 6.5f))
            using (var brush = new SolidBrush(C_TXT))
            {
                var fmt = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                string[] muL = { "0", ".25", ".50", ".75", "1" };
                for (int s = 0; s <= 4; s++)
                {
                    int py = MT + ch - (int)(s * 0.25 * ch);
                    g.DrawString(muL[s], font, brush, new RectangleF(0, py - 8, ML - 3, 16), fmt);
                }

                double range = InputMax - InputMin;
                double step = GridStep();
                var ctrFmt = new StringFormat { Alignment = StringAlignment.Center };
                for (double v = Math.Ceiling(InputMin / step) * step; v <= InputMax; v += step)
                {
                    int px = ML + (int)((v - InputMin) / range * cw);
                    g.DrawString(v.ToString("0.##"), font, brush, new RectangleF(px - 16, MT + ch + 3, 32, 14), ctrFmt);
                }
            }
        }

        private void DrawTrap(Graphics g, int cw, int ch, double a, double b, double c, double d, Color fill, Color line, string label)
        {
            var pts = new[] { MapPoint(a, 0, cw, ch), MapPoint(b, 1, cw, ch), MapPoint(c, 1, cw, ch), MapPoint(d, 0, cw, ch) };
            using (var fb = new SolidBrush(fill)) g.FillPolygon(fb, pts);
            using (var lp = new Pen(line, 1.5f)) g.DrawPolygon(lp, pts);

            double midX = (Math.Max(a,b) + Math.Min(c,d)) / 2.0;
            var midPt = MapPoint(midX, 1, cw, ch);
            using (var lf = new Font("Segoe UI", 7f, FontStyle.Bold))
            using (var lb = new SolidBrush(line))
            {
                var sz = g.MeasureString(label, lf);
                g.DrawString(label, lf, lb, midPt.X - sz.Width / 2f, midPt.Y - sz.Height - 1);
            }
        }

        private void DrawClippedTrap(Graphics g, int cw, int ch, double a, double b, double c, double d, double alpha, Color fill, Color line, string label)
        {
            if (alpha <= 0.001) return;
            
            // Calculate intersection points for the alpha cut
            double leftX = a + (b - a) * alpha;
            double rightX = d - (d - c) * alpha;
            
            // Clip to bounds
            if (leftX < a) leftX = a;
            if (leftX > b) leftX = b;
            if (rightX > d) rightX = d;
            if (rightX < c) rightX = c;

            var pts = new[] {
                MapPoint(a, 0, cw, ch),
                MapPoint(leftX, alpha, cw, ch),
                MapPoint(rightX, alpha, cw, ch),
                MapPoint(d, 0, cw, ch)
            };
            
            using (var fb = new SolidBrush(fill)) g.FillPolygon(fb, pts);
            
            // Draw faint full outline for context
            var fullPts = new[] { MapPoint(a, 0, cw, ch), MapPoint(b, 1, cw, ch), MapPoint(c, 1, cw, ch), MapPoint(d, 0, cw, ch) };
            using (var fp = new Pen(Color.FromArgb(40, line), 1f)) g.DrawPolygon(fp, fullPts);

            // Draw label
            double midX = (leftX + rightX) / 2.0;
            var midPt = MapPoint(midX, alpha, cw, ch);
            using (var lf = new Font("Segoe UI", 7f, FontStyle.Bold))
            using (var lb = new SolidBrush(line))
            {
                var sz = g.MeasureString(label, lf);
                g.DrawString(label, lf, lb, midPt.X - sz.Width / 2f, midPt.Y - sz.Height - 1);
            }
        }
        
        private void DrawAggregatedOutline(Graphics g, int cw, int ch)
        {
            // Simple approach: sample points and draw the max envelope
            using (var p = new Pen(Color.FromArgb(180, 40, 40, 40), 2f))
            {
                PointF? lastPt = null;
                double step = (InputMax - InputMin) / (cw / 2.0); // 1 point per 2 pixels
                for (double x = InputMin; x <= InputMax; x += step)
                {
                    double d1 = Math.Min(Membership.Low, TrapMem(x, 0, 0, 3, 6));
                    double d2 = Math.Min(Membership.Normal, TrapMem(x, 4, 6, 9, 11));
                    double d3 = Math.Min(Membership.High, TrapMem(x, 9, 12, 15, 15));
                    double max = Math.Max(d1, Math.Max(d2, d3));
                    
                    PointF pt = MapPoint(x, max, cw, ch);
                    if (lastPt.HasValue) g.DrawLine(p, lastPt.Value, pt);
                    lastPt = pt;
                }
            }
        }
        
        private double TrapMem(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d) return 0.0;
            if (x >= b && x <= c) return 1.0;
            if (x > a && x < b) return (x - a) / (b - a);
            return (d - x) / (d - c);
        }

        private PointF MapPoint(double x, double mu, int cw, int ch)
        {
            double range = InputMax - InputMin;
            return new PointF(ML + (float)((x - InputMin) / range * cw), MT + ch - (float)(mu * ch));
        }

        private void DrawCursor(Graphics g, int cw, int ch)
        {
            double range = InputMax - InputMin;
            double clamped = Math.Max(InputMin, Math.Min(InputMax, CurrentValue));
            int cx = ML + (int)((clamped - InputMin) / range * cw);

            using (var cp = new Pen(Color.FromArgb(160, 80, 80, 80), 1.5f))
            {
                cp.DashStyle = DashStyle.Dash;
                g.DrawLine(cp, cx, MT, cx, MT + ch);
            }

            using (var db = new SolidBrush(Color.FromArgb(80, 80, 80)))
                g.FillEllipse(db, cx - 3, MT + ch - 3, 6, 6);

            string valStr = ChartType == ChartVariableType.HeartRate 
                ? $"{CurrentValue:F0} bpm" 
                : $"{CurrentValue:F0} mmHg";

            using (var vf = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var vb = new SolidBrush(Color.FromArgb(60, 60, 60)))
            {
                var sz = g.MeasureString(valStr, vf);
                float lx = Math.Max(ML, Math.Min(ML + cw - sz.Width, cx - sz.Width / 2f));
                g.DrawString(valStr, vf, vb, lx, 2f);
            }

            DrawMuHit(g, cx, cw, ch, Membership.Low, C_LO_L, "L");
            DrawMuHit(g, cx, cw, ch, Membership.Normal, C_NM_L, "N");
            DrawMuHit(g, cx, cw, ch, Membership.High, C_HI_L, "H");
        }
        
        private void DrawOutputCentroid(Graphics g, int cw, int ch)
        {
            if (CurrentValue <= 0) return; // No output yet
            
            double range = InputMax - InputMin;
            double clamped = Math.Max(InputMin, Math.Min(InputMax, CurrentValue));
            int cx = ML + (int)((clamped - InputMin) / range * cw);

            // Bold red line for centroid
            using (var cp = new Pen(Color.FromArgb(220, 30, 30), 2f))
            {
                g.DrawLine(cp, cx, MT, cx, MT + ch);
            }
            
            using (var db = new SolidBrush(Color.FromArgb(220, 30, 30)))
            {
                // Top triangle marker
                PointF[] pts = { new PointF(cx - 5, MT), new PointF(cx + 5, MT), new PointF(cx, MT + 8) };
                g.FillPolygon(db, pts);
            }

            string valStr = $"{CurrentValue:F2} mg/h";
            using (var vf = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var vb = new SolidBrush(Color.FromArgb(220, 30, 30)))
            {
                var sz = g.MeasureString(valStr, vf);
                float lx = Math.Max(ML, Math.Min(ML + cw - sz.Width, cx - sz.Width / 2f));
                g.DrawString(valStr, vf, vb, lx, 2f);
            }
        }

        private void DrawMuHit(Graphics g, int cx, int cw, int ch, double mu, Color col, string setLetter)
        {
            if (mu < 0.01) return;
            int py = MT + ch - (int)(mu * ch);

            using (var fb = new SolidBrush(col)) g.FillEllipse(fb, cx - 4, py - 4, 8, 8);
            using (var pb = new Pen(Color.FromArgb(60, 60, 60), 1f)) g.DrawEllipse(pb, cx - 4, py - 4, 8, 8);

            using (var font = new Font("Segoe UI", 6.5f, FontStyle.Bold))
            using (var brush = new SolidBrush(col))
            {
                string txt = $"\u03bc{setLetter}={mu:F2}";
                int labelX = cx + 6;
                if (labelX + 46 > ML + cw) labelX = cx - 46;
                g.DrawString(txt, font, brush, labelX, py - 8);
            }
        }
    }
}

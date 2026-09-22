using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;

namespace InfusionPumpV1
{
    public class Surface3DControl : Control
    {
        private double[,] surfaceData;
        private int gridSizeHR = 57; // 40 to 180 step 2.5
        private int gridSizeBP = 49; // 60 to 180 step 2.5

        private float minHR = 40f;
        private float maxHR = 180f;
        private float minBP = 60f;
        private float maxBP = 180f;
        private float minPR = 0f;
        private float maxPR = 15f;

        private float cameraPitch = 30f; // degrees
        private float cameraYaw = 40f;   // degrees
        private float cameraScale = 1.0f;
        
        private Point lastMousePos;
        private Point dragStartPos;
        private bool isDragging = false;
        private Point hoverMousePos;
        private double hoverHR = -1;
        private double hoverBP = -1;
        private double hoverPR = -1;

        public bool DarkMode { get; set; } = false;

        public double CurrentHR { get; set; } = 110;
        public double CurrentBP { get; set; } = 95;
        public double CurrentPumpRate { get; set; } = 7.5;

        public event Action<double, double> OnSurfaceClicked;

        private ContextMenuStrip cameraMenu;

        public Surface3DControl()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.White;
            this.SetStyle(ControlStyles.Selectable, true);

            GenerateSurfaceData();

            this.MouseDown += Surface3DControl_MouseDown;
            this.MouseMove += Surface3DControl_MouseMove;
            this.MouseUp += Surface3DControl_MouseUp;
            this.MouseWheel += Surface3DControl_MouseWheel;
            this.MouseLeave += Surface3DControl_MouseLeave;
            
            InitializeCameraMenu();
        }

        private void Surface3DControl_MouseLeave(object sender, EventArgs e)
        {
            hoverHR = -1;
            Invalidate();
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            float rotateStep = 5f;
            if (e.KeyCode == Keys.Up) { cameraPitch += rotateStep; e.Handled = true; }
            if (e.KeyCode == Keys.Down) { cameraPitch -= rotateStep; e.Handled = true; }
            if (e.KeyCode == Keys.Left) { cameraYaw += rotateStep; e.Handled = true; }
            if (e.KeyCode == Keys.Right) { cameraYaw -= rotateStep; e.Handled = true; }
            if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) { cameraScale *= 1.1f; e.Handled = true; }
            if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) { cameraScale /= 1.1f; e.Handled = true; }

            cameraPitch = Math.Max(2, Math.Min(90, cameraPitch));
            if (e.Handled) Invalidate();
            
            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        private void InitializeCameraMenu()
        {
            cameraMenu = new ContextMenuStrip();
            cameraMenu.Items.Add("Angled View", null, (s, e) => { cameraPitch = 30f; cameraYaw = 40f; cameraScale = 1.0f; Invalidate(); });
            cameraMenu.Items.Add("Top View", null, (s, e) => { cameraPitch = 90f; cameraYaw = 0f; cameraScale = 1.0f; Invalidate(); });
            cameraMenu.Items.Add("Side View (HR)", null, (s, e) => { cameraPitch = 5f; cameraYaw = 0f; cameraScale = 1.0f; Invalidate(); });
            cameraMenu.Items.Add("Side View (BP)", null, (s, e) => { cameraPitch = 5f; cameraYaw = -90f; cameraScale = 1.0f; Invalidate(); });
            this.ContextMenuStrip = cameraMenu;
        }

        public void GenerateSurfaceData()
        {
            surfaceData = new double[gridSizeHR, gridSizeBP];
            for (int i = 0; i < gridSizeHR; i++)
            {
                double hr = minHR + (maxHR - minHR) * i / (gridSizeHR - 1);
                for (int j = 0; j < gridSizeBP; j++)
                {
                    double bp = minBP + (maxBP - minBP) * j / (gridSizeBP - 1);
                    surfaceData[i, j] = CalculatePumpRateLogic(hr, bp);
                }
            }
            Invalidate();
        }

        private void Surface3DControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                cameraScale *= 1.1f;
            else if (e.Delta < 0)
                cameraScale /= 1.1f;
            Invalidate();
        }

        private void Surface3DControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPos = e.Location;
                lastMousePos = e.Location;
            }
        }

        private void Surface3DControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                float dx = e.X - lastMousePos.X;
                float dy = e.Y - lastMousePos.Y;

                cameraYaw += dx * 0.5f;
                cameraPitch -= dy * 0.5f;

                cameraPitch = Math.Max(2, Math.Min(90, cameraPitch)); // Clamped roughly 0.05 to 1.5 rad

                lastMousePos = e.Location;
                Invalidate();
            }
            else
            {
                hoverMousePos = e.Location;
                double bestDist = double.MaxValue;
                double bestHR = -1;
                double bestBP = -1;
                double bestPR = -1;

                if (surfaceData != null)
                {
                    for (int i = 0; i < gridSizeHR; i += 2) // Step 2 for performance
                    {
                        double hr = minHR + (maxHR - minHR) * i / (gridSizeHR - 1);
                        for (int j = 0; j < gridSizeBP; j += 2)
                        {
                            double bp = minBP + (maxBP - minBP) * j / (gridSizeBP - 1);
                            double pr = surfaceData[i, j];

                            PointF pt2d = Project(hr, bp, pr, out _);
                            double dist = Math.Pow(pt2d.X - e.X, 2) + Math.Pow(pt2d.Y - e.Y, 2);
                            if (dist < bestDist)
                            {
                                bestDist = dist;
                                bestHR = hr;
                                bestBP = bp;
                                bestPR = pr;
                            }
                        }
                    }

                    if (bestDist < 400) // Within 20 pixels radius
                    {
                        if (hoverHR != bestHR || hoverBP != bestBP)
                        {
                            hoverHR = bestHR;
                            hoverBP = bestBP;
                            hoverPR = bestPR;
                            Invalidate();
                        }
                    }
                    else if (hoverHR != -1)
                    {
                        hoverHR = -1;
                        Invalidate();
                    }
                }
            }
        }

        private void Surface3DControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
            }
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.X - dragStartPos.X) < 3 && Math.Abs(e.Y - dragStartPos.Y) < 3)
                {
                    if (hoverHR != -1)
                    {
                        OnSurfaceClicked?.Invoke(Math.Round(hoverHR), Math.Round(hoverBP));
                    }
                }
            }
        }

        private PointF Project(double hr, double bp, double pr, out double depth)
        {
            double x = (hr - 110) / 70.0;
            double y = (bp - 120) / 60.0;
            double z = (pr / 15.0) * 1.1 - 0.55;

            double pitchRad = cameraPitch * Math.PI / 180.0;
            double yawRad = cameraYaw * Math.PI / 180.0;

            double x1 = x * Math.Cos(yawRad) - y * Math.Sin(yawRad);
            double y1 = x * Math.Sin(yawRad) + y * Math.Cos(yawRad);

            double sc = Math.Min(Width * 0.27, Height * 0.33) * cameraScale;
            double ox = Width / 2.0;
            double oy = Height / 2.0;

            double screenX = ox + x1 * sc;
            double screenY = oy - (z * Math.Cos(pitchRad) + y1 * Math.Sin(pitchRad)) * sc;
            
            depth = y1 * Math.Cos(pitchRad) - z * Math.Sin(pitchRad);

            return new PointF((float)screenX, (float)screenY);
        }

        private Color GetHeightColor(double pr, int alpha = 255)
        {
            // 3-stop gradient: Blue (0) -> Green (7.5) -> Orange-Red (15)
            float ratio = (float)(pr / 15.0);
            ratio = Math.Max(0, Math.Min(1, ratio));

            Color low = Color.FromArgb(47, 111, 208);   // #2F6FD0
            Color mid = Color.FromArgb(31, 154, 107);   // #1F9A6B
            Color high = Color.FromArgb(224, 89, 47);   // #E0592F

            if (ratio < 0.5f)
            {
                float t = ratio * 2f;
                return Color.FromArgb(alpha, 
                    (int)(low.R + t * (mid.R - low.R)),
                    (int)(low.G + t * (mid.G - low.G)),
                    (int)(low.B + t * (mid.B - low.B)));
            }
            else
            {
                float t = (ratio - 0.5f) * 2f;
                return Color.FromArgb(alpha, 
                    (int)(mid.R + t * (high.R - mid.R)),
                    (int)(mid.G + t * (high.G - mid.G)),
                    (int)(mid.B + t * (high.B - mid.B)));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            this.BackColor = DarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            if (surfaceData == null) return;

            DrawWallsAndFloor(e.Graphics);

            // Collect all polygons for the surface and floor map
            List<Polygon> polys = new List<Polygon>();
            
            // Floor contour map (stride of 3 for performance/coarseness)
            int stride = 3;
            for (int i = 0; i < gridSizeHR - stride; i += stride)
            {
                for (int j = 0; j < gridSizeBP - stride; j += stride)
                {
                    double hr1 = minHR + (maxHR - minHR) * i / (gridSizeHR - 1);
                    double hr2 = minHR + (maxHR - minHR) * (i + stride) / (gridSizeHR - 1);
                    double bp1 = minBP + (maxBP - minBP) * j / (gridSizeBP - 1);
                    double bp2 = minBP + (maxBP - minBP) * (j + stride) / (gridSizeBP - 1);

                    double avgPr = (surfaceData[i, j] + surfaceData[i + stride, j] + surfaceData[i, j + stride] + surfaceData[i + stride, j + stride]) / 4.0;
                    
                    polys.Add(new Polygon(hr1, bp1, 0, hr2, bp1, 0, hr2, bp2, 0, hr1, bp2, 0, GetHeightColor(avgPr, 100), false));
                }
            }

            // Surface Mesh
            for (int i = 0; i < gridSizeHR - 1; i++)
            {
                for (int j = 0; j < gridSizeBP - 1; j++)
                {
                    double hr1 = minHR + (maxHR - minHR) * i / (gridSizeHR - 1);
                    double hr2 = minHR + (maxHR - minHR) * (i + 1) / (gridSizeHR - 1);
                    double bp1 = minBP + (maxBP - minBP) * j / (gridSizeBP - 1);
                    double bp2 = minBP + (maxBP - minBP) * (j + 1) / (gridSizeBP - 1);

                    double pr11 = surfaceData[i, j];
                    double pr21 = surfaceData[i + 1, j];
                    double pr12 = surfaceData[i, j + 1];
                    double pr22 = surfaceData[i + 1, j + 1];

                    double avgPr = (pr11 + pr21 + pr12 + pr22) / 4.0;

                    polys.Add(new Polygon(hr1, bp1, pr11, hr2, bp1, pr21, hr2, bp2, pr22, hr1, bp2, pr12, GetHeightColor(avgPr, 255), true));
                }
            }

            // Project all
            foreach (var poly in polys)
            {
                poly.Project(Project);
            }

            // Sort by depth (painter's algorithm, largest first)
            polys = polys.OrderByDescending(p => p.AvgDepth).ToList();

            Pen meshPen = new Pen(Color.FromArgb(50, DarkMode ? 0 : 255, DarkMode ? 0 : 255, DarkMode ? 0 : 255));
            foreach (var poly in polys)
            {
                using (SolidBrush b = new SolidBrush(poly.Color))
                {
                    e.Graphics.FillPolygon(b, poly.Points2D);
                }
                if (poly.IsSurface)
                    e.Graphics.DrawPolygon(meshPen, poly.Points2D);
            }

            DrawCurrentPoint(e.Graphics);
            DrawColorKey(e.Graphics);
            
            if (Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
            }

            if (hoverHR != -1)
            {
                string tt = $"HR: {hoverHR:F0}\nBP: {hoverBP:F0}\nPR: {hoverPR:F2}";
                Font ttFont = new Font("Segoe UI", 9);
                SizeF sz = e.Graphics.MeasureString(tt, ttFont);
                RectangleF rect = new RectangleF(hoverMousePos.X + 15, hoverMousePos.Y + 15, sz.Width + 10, sz.Height + 10);
                e.Graphics.FillRectangle(DarkMode ? Brushes.Black : Brushes.White, rect);
                e.Graphics.DrawRectangle(DarkMode ? Pens.Gray : Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
                e.Graphics.DrawString(tt, ttFont, DarkMode ? Brushes.White : Brushes.Black, rect.X + 5, rect.Y + 5);
            }
            
            e.Graphics.DrawString("Drag: Rotate | Wheel: Zoom | Click: Set Vitals | RightClick: Camera", Font, Brushes.Gray, 5, Height - 20);
        }

        private void DrawWallsAndFloor(Graphics g)
        {
            Pen gridPen = new Pen(DarkMode ? Color.FromArgb(60, 60, 60) : Color.LightGray, 1);
            Pen axisPen = new Pen(DarkMode ? Color.Gray : Color.Black, 1);
            Font lblFont = new Font("Segoe UI", 8);
            Brush lblBrush = DarkMode ? Brushes.LightGray : Brushes.DimGray;
            Brush titleBrush = DarkMode ? Brushes.White : Brushes.Black;

            // Define the 4 vertical walls
            var walls = new List<Wall>
            {
                new Wall { Hr1 = 40, Bp1 = 60, Hr2 = 180, Bp2 = 60, Axis = "HR" },
                new Wall { Hr1 = 180, Bp1 = 60, Hr2 = 180, Bp2 = 180, Axis = "BP" },
                new Wall { Hr1 = 180, Bp1 = 180, Hr2 = 40, Bp2 = 180, Axis = "HR" },
                new Wall { Hr1 = 40, Bp1 = 180, Hr2 = 40, Bp2 = 60, Axis = "BP" }
            };

            foreach (var w in walls)
            {
                double cHr = (w.Hr1 + w.Hr2) / 2.0;
                double cBp = (w.Bp1 + w.Bp2) / 2.0;
                Project(cHr, cBp, 7.5, out double d);
                w.Depth = d;
            }

            // Two back walls have largest depth
            var backWalls = walls.OrderByDescending(w => w.Depth).Take(2).ToList();

            foreach (var w in backWalls)
            {
                // Draw wall grid
                if (w.Axis == "HR")
                {
                    for (int h = 40; h <= 180; h += 20)
                    {
                        PointF p1 = Project(h, w.Bp1, 0, out _);
                        PointF p2 = Project(h, w.Bp1, 15, out _);
                        g.DrawLine(gridPen, p1, p2);
                    }
                }
                else
                {
                    for (int b = 60; b <= 180; b += 20)
                    {
                        PointF p1 = Project(w.Hr1, b, 0, out _);
                        PointF p2 = Project(w.Hr1, b, 15, out _);
                        g.DrawLine(gridPen, p1, p2);
                    }
                }

                // Horizontal Z lines
                for (int z = 0; z <= 15; z += 5)
                {
                    PointF p1 = Project(w.Hr1, w.Bp1, z, out _);
                    PointF p2 = Project(w.Hr2, w.Bp2, z, out _);
                    g.DrawLine(gridPen, p1, p2);
                }
            }

            // Floor grid
            for (int h = 40; h <= 180; h += 20)
            {
                PointF p1 = Project(h, 60, 0, out _);
                PointF p2 = Project(h, 180, 0, out _);
                g.DrawLine(gridPen, p1, p2);
            }
            for (int b = 60; b <= 180; b += 20)
            {
                PointF p1 = Project(40, b, 0, out _);
                PointF p2 = Project(180, b, 0, out _);
                g.DrawLine(gridPen, p1, p2);
            }

            // Front axes / labels (the two walls NOT in back walls)
            var frontWalls = walls.Except(backWalls).ToList();
            foreach (var w in frontWalls)
            {
                PointF p1 = Project(w.Hr1, w.Bp1, 0, out _);
                PointF p2 = Project(w.Hr2, w.Bp2, 0, out _);
                g.DrawLine(axisPen, p1, p2);

                if (w.Axis == "HR")
                {
                    for (int h = 40; h <= 180; h += 20)
                    {
                        PointF pt = Project(h, w.Bp1, 0, out _);
                        g.DrawString(h.ToString(), lblFont, lblBrush, pt.X - 10, pt.Y + 5);
                    }
                    PointF mid = Project((w.Hr1 + w.Hr2)/2, w.Bp1, 0, out _);
                    g.DrawString("Heart rate (bpm)", lblFont, titleBrush, mid.X - 30, mid.Y + 20);
                }
                else
                {
                    for (int b = 60; b <= 180; b += 20)
                    {
                        PointF pt = Project(w.Hr1, b, 0, out _);
                        g.DrawString(b.ToString(), lblFont, lblBrush, pt.X - 10, pt.Y + 5);
                    }
                    PointF mid = Project(w.Hr1, (w.Bp1 + w.Bp2)/2, 0, out _);
                    g.DrawString("Blood pressure (mmHg)", lblFont, titleBrush, mid.X + 15, mid.Y + 10);
                }
            }
            
            // Leftmost vertical edge for Z labels
            Wall leftWall = frontWalls.OrderBy(w => Project(w.Hr1, w.Bp1, 0, out _).X).FirstOrDefault();
            if (leftWall != null)
            {
                PointF pBot = Project(leftWall.Hr1, leftWall.Bp1, 0, out _);
                PointF pTop = Project(leftWall.Hr1, leftWall.Bp1, 15, out _);
                g.DrawLine(axisPen, pBot, pTop);

                for (int z = 0; z <= 15; z += 5)
                {
                    PointF pt = Project(leftWall.Hr1, leftWall.Bp1, z, out _);
                    g.DrawString(z.ToString(), lblFont, lblBrush, pt.X - 20, pt.Y - 5);
                }
                PointF midZ = Project(leftWall.Hr1, leftWall.Bp1, 7.5, out _);
                g.DrawString("Pump rate (mg/min)", lblFont, titleBrush, midZ.X - 110, midZ.Y - 5);
            }
        }

        private void DrawCurrentPoint(Graphics g)
        {
            PointF p3d = Project(CurrentHR, CurrentBP, CurrentPumpRate, out _);
            PointF pFloor = Project(CurrentHR, CurrentBP, 0, out _);

            using (Pen dashPen = new Pen(DarkMode ? Color.LightGray : Color.Black, 1) { DashStyle = DashStyle.Dash })
            {
                g.DrawLine(dashPen, p3d, pFloor);
            }

            g.FillEllipse(new SolidBrush(Color.FromArgb(100, DarkMode ? 255 : 0, DarkMode ? 255 : 0, DarkMode ? 255 : 0)), pFloor.X - 6, pFloor.Y - 3, 12, 6);
            
            g.FillEllipse(Brushes.Black, p3d.X - 4, p3d.Y - 4, 8, 8);
            g.DrawEllipse(Pens.White, p3d.X - 4, p3d.Y - 4, 8, 8);

            string txt = $"{CurrentPumpRate:F2} mg/min";
            Font f = new Font("Segoe UI", 9, FontStyle.Bold);
            // Halo
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    g.DrawString(txt, f, DarkMode ? Brushes.Black : Brushes.White, p3d.X + 10 + i, p3d.Y - 10 + j);
            g.DrawString(txt, f, DarkMode ? Brushes.White : Brushes.Black, p3d.X + 10, p3d.Y - 10);
        }

        private void DrawColorKey(Graphics g)
        {
            int x = 20;
            int y = 20;
            int width = 150;
            int height = 15;

            Rectangle rect = new Rectangle(x, y, width, height);
            using (LinearGradientBrush b = new LinearGradientBrush(rect, Color.FromArgb(47, 111, 208), Color.FromArgb(224, 89, 47), 0f))
            {
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0f, 0.5f, 1f };
                cb.Colors = new[] { Color.FromArgb(47, 111, 208), Color.FromArgb(31, 154, 107), Color.FromArgb(224, 89, 47) };
                b.InterpolationColors = cb;
                g.FillRectangle(b, rect);
            }
            g.DrawRectangle(Pens.Gray, rect);

            Brush textBrush = DarkMode ? Brushes.White : Brushes.Black;
            Font f = new Font("Segoe UI", 8);
            g.DrawString("0", f, textBrush, x, y + height);
            g.DrawString("7.5", f, textBrush, x + width/2 - 10, y + height);
            g.DrawString("15", f, textBrush, x + width - 15, y + height);
        }

        class Wall
        {
            public double Hr1, Bp1, Hr2, Bp2;
            public string Axis;
            public double Depth;
        }

        class Polygon
        {
            public double Hr1, Bp1, Pr1, Hr2, Bp2, Pr2, Hr3, Bp3, Pr3, Hr4, Bp4, Pr4;
            public PointF[] Points2D;
            public double AvgDepth;
            public Color Color;
            public bool IsSurface;

            public delegate PointF ProjectDelegate(double hr, double bp, double pr, out double depth);

            public Polygon(double hr1, double bp1, double pr1, double hr2, double bp2, double pr2, double hr3, double bp3, double pr3, double hr4, double bp4, double pr4, Color c, bool isSurface)
            {
                Hr1 = hr1; Bp1 = bp1; Pr1 = pr1;
                Hr2 = hr2; Bp2 = bp2; Pr2 = pr2;
                Hr3 = hr3; Bp3 = bp3; Pr3 = pr3;
                Hr4 = hr4; Bp4 = bp4; Pr4 = pr4;
                Color = c;
                IsSurface = isSurface;
                Points2D = new PointF[4];
            }

            public void Project(ProjectDelegate proj)
            {
                Points2D[0] = proj(Hr1, Bp1, Pr1, out double d1);
                Points2D[1] = proj(Hr2, Bp2, Pr2, out double d2);
                Points2D[2] = proj(Hr3, Bp3, Pr3, out double d3);
                Points2D[3] = proj(Hr4, Bp4, Pr4, out double d4);
                AvgDepth = (d1 + d2 + d3 + d4) / 4.0;
            }
        }

        // --- DUPLICATED FUZZY LOGIC FOR PURE CALCULATION ---
        static double TrapezoidalMembership(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d) return 0.0;
            if (x >= b && x <= c) return 1.0;
            if (x > a && x < b) return (x - a) / (b - a);
            return (d - x) / (d - c);
        }

        public static double CalculatePumpRateLogic(double hr, double bp)
        {
            double hrLow    = TrapezoidalMembership(hr,    40,  40,  60,  80);
            double hrNormal = TrapezoidalMembership(hr,    60,  80, 100, 120);
            double hrHigh   = TrapezoidalMembership(hr,   100, 120, 180, 180);

            double bpLow    = TrapezoidalMembership(bp,  60,  60,  80, 100);
            double bpNormal = TrapezoidalMembership(bp,  90, 105, 125, 140);
            double bpHigh   = TrapezoidalMembership(bp, 130, 145, 180, 180);

            double rule1 = Math.Min(hrLow, bpLow);
            double rule2 = Math.Min(hrLow, bpNormal);
            double rule3 = Math.Min(hrNormal, bpLow);
            double rule4 = Math.Min(hrLow, bpHigh);
            double rule5 = Math.Min(hrNormal, bpNormal);
            double rule6 = Math.Min(hrHigh, bpLow);
            double rule7 = Math.Min(hrNormal, bpHigh);
            double rule8 = Math.Min(hrHigh, bpNormal);
            double rule9 = Math.Min(hrHigh, bpHigh);

            double strengthDecrease = Math.Max(rule1, Math.Max(rule2, rule3));
            double strengthMaintain = Math.Max(rule4, Math.Max(rule5, rule6));
            double strengthIncrease = Math.Max(rule7, Math.Max(rule8, rule9));

            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 0.1; // Matched C# code 151 points

            for (double y = 0.0; y <= 15.0; y += step)
            {
                double outDecrease = TrapezoidalMembership(y, 0.0, 0.0,  3.0,  6.0);
                double outMaintain = TrapezoidalMembership(y, 4.0, 6.0,  9.0, 11.0);
                double outIncrease = TrapezoidalMembership(y, 9.0, 12.0, 15.0, 15.0);

                double clippedDecrease = Math.Min(strengthDecrease, outDecrease);
                double clippedMaintain = Math.Min(strengthMaintain, outMaintain);
                double clippedIncrease = Math.Min(strengthIncrease, outIncrease);

                double aggregatedY = Math.Max(clippedDecrease, Math.Max(clippedMaintain, clippedIncrease));

                sumNumerator   += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            if (sumDenominator > 0.0)
                return sumNumerator / sumDenominator;
            return 0.0;
        }
    }
}

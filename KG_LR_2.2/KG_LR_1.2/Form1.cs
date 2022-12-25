using System;
using System.Drawing;
using System.Windows.Forms;
using myGraphics;

namespace KG_LR_2_1
{
    public partial class MainForm : Form
    {
        struct KeyState
        {
            public bool W, S, A, D, Space, C,           //  movement
                        Up, Down, Left, Right, Q, E,    //  rotation around local origin
                        I, K, J, L, U, O,               //  rotation around global origin
                        R, F;                           //  scaling
        }

        OpenFileDialog OPF = new OpenFileDialog();
        Mesh mesh;
        Graphics g;
        Rectangle winRect;

        Color borderColor = Color.Aqua;
        Color faceColor = Color.DarkCyan;

        float moveSpeed = 0.07f,
              rotSpeed = 4f,        // degrees
              scaleSpeed = 0.03f;
        bool animationState = false;

        KeyState keyState;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Width = 800;
            this.Height = 600;
            winRect.Height = this.Height;
            winRect.Width = this.Width;
            OPF.Filter = "Obj файл|*.obj";
            OPF.Multiselect = false;
            timer1.Enabled = false;
        }
        
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                string inputFile;
                inputFile = OPF.FileName;
                mesh = new Mesh(inputFile);
                timer1.Enabled = true;
            }
            else
                return;
            mesh.InitiliseWBuffer(winRect.Width, winRect.Height);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            winRect.Height = this.Height;
            winRect.Width = this.Width;
            g.Clear(Color.MidnightBlue);
            if (mesh != null)
                mesh.DrawObject(g, winRect, borderColor, faceColor);
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //  movement
            if (e.KeyValue == (char)Keys.W)// 'W'
                keyState.W = true;
            if (e.KeyValue == (char)Keys.S)// 'S'
                keyState.S = true;
            if (e.KeyValue == (char)Keys.A)// 'A'
                keyState.A = true;
            if (e.KeyValue == (char)Keys.D)// 'D'
                keyState.D = true;
            if (e.KeyValue == (char)Keys.Space)
                keyState.Space = true;
            if (e.KeyValue == (char)Keys.C)// 'C'
                keyState.C = true;

            // rotation around global origin
            if (e.KeyValue == (char)Keys.I)
                keyState.I = true;
            if (e.KeyValue == (char)Keys.K)
                keyState.K = true;
            if (e.KeyValue == (char)Keys.L)
                keyState.L = true;
            if (e.KeyValue == (char)Keys.J)
                keyState.J = true;
            if (e.KeyValue == (char)Keys.U)
                keyState.U = true;
            if (e.KeyValue == (char)Keys.O)
                keyState.O = true;

            //  rotation around local origin
            if (e.KeyValue == (char)Keys.Up)
                keyState.Up = true;
            if (e.KeyValue == (char)Keys.Down)
                keyState.Down = true;
            if (e.KeyValue == (char)Keys.Right)
                keyState.Right = true;
            if (e.KeyValue == (char)Keys.Left)
                keyState.Left = true;
            if (e.KeyValue == (char)Keys.E)// 'E'
                keyState.E = true;
            if (e.KeyValue == (char)Keys.Q)// 'Q'
                keyState.Q = true;

            // scaling
            if (e.KeyValue == (char)Keys.R)
                keyState.R = true;
            if (e.KeyValue == (char)Keys.F)
                keyState.F = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //  movement
            if (e.KeyValue == (char)Keys.W)// 'W'
                keyState.W = false;
            if (e.KeyValue == (char)Keys.S)// 'S'
                keyState.S = false;
            if (e.KeyValue == (char)Keys.A)// 'A'
                keyState.A = false;
            if (e.KeyValue == (char)Keys.D)// 'D'
                keyState.D = false;
            if (e.KeyValue == (char)Keys.Space)
                keyState.Space = false;
            if (e.KeyValue == (char)Keys.C)// 'C'
                keyState.C = false;

            //  rotaion around local origin
            if (e.KeyValue == (char)Keys.Up)
                keyState.Up = false;
            if (e.KeyValue == (char)Keys.Down)
                keyState.Down = false;
            if (e.KeyValue == (char)Keys.Right)
                keyState.Right = false;
            if (e.KeyValue == (char)Keys.Left)
                keyState.Left = false;
            if (e.KeyValue == (char)Keys.E)// 'E'
                keyState.E = false;
            if (e.KeyValue == (char)Keys.Q)// 'Q'
                keyState.Q = false;

            //  rotation around global origin
            if (e.KeyValue == (char)Keys.I)
                keyState.I = false;
            if (e.KeyValue == (char)Keys.K)
                keyState.K = false;
            if (e.KeyValue == (char)Keys.L)
                keyState.L = false;
            if (e.KeyValue == (char)Keys.J)
                keyState.J = false;
            if (e.KeyValue == (char)Keys.U)
                keyState.U = false;
            if (e.KeyValue == (char)Keys.O)
                keyState.O = false;

            // scaling
            if (e.KeyValue == (char)Keys.R)
                keyState.R = false;
            if (e.KeyValue == (char)Keys.F)
                keyState.F = false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            float moveX = 0, moveY = 0, moveZ = 0,
                  rotXl = 0, rotYl = 0, rotZl = 0,
                  rotXg = 0, rotYg = 0, rotZg = 0,
                  scaleFac = 1;
            if (animationState)
            {
                rotYl = rotSpeed / 3;
                rotXl = rotSpeed / 3;
                rotZl = rotSpeed / 3;
            }
            else
            {
                if (keyState.W) moveZ = -moveSpeed;
                if (keyState.S) moveZ = moveSpeed;
                if (keyState.A) moveX = -moveSpeed;
                if (keyState.D) moveX = moveSpeed;
                if (keyState.Space) moveY = moveSpeed;
                if (keyState.C) moveY = -moveSpeed;

                if (keyState.Up) rotXl = -rotSpeed;
                if (keyState.Down) rotXl = rotSpeed;
                if (keyState.Left) rotZl = rotSpeed;
                if (keyState.Right) rotZl = -rotSpeed;
                if (keyState.Q) rotYl = -rotSpeed;
                if (keyState.E) rotYl = rotSpeed;

                if (keyState.I) rotXg = -rotSpeed / 2;
                if (keyState.K) rotXg = rotSpeed / 2;
                if (keyState.J) rotZg = rotSpeed / 2;
                if (keyState.L) rotZg = -rotSpeed / 2;
                if (keyState.U) rotYg = -rotSpeed / 2;
                if (keyState.O) rotYg = rotSpeed / 2;

                if (keyState.R) scaleFac += scaleSpeed;
                if (keyState.F) scaleFac -= scaleSpeed;
            }

            mesh.move(moveX, moveY, moveZ);
            mesh.rotate_local(rotXl, Axis.axX);
            mesh.rotate_local(rotYl, Axis.axY);
            mesh.rotate_local(rotZl, Axis.axZ);
            mesh.rotate_global(rotXg, Axis.axX);
            mesh.rotate_global(rotYg, Axis.axY);
            mesh.rotate_global(rotZg, Axis.axZ);
            mesh.scale(scaleFac);

            Refresh();
        }

        private void AnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(mesh!=null)
                animationState = !animationState;
        }

        private void SetOrtho_Click(object sender, EventArgs e)
        {
            mesh.PerspectiveOn(false);
        }

        private void SetPerspective_Click(object sender, EventArgs e)
        {
            mesh.PerspectiveOn(true);
        }

        private void SetDefaultRender_Click(object sender, EventArgs e)
        {
            mesh.RenderMode(0);
        }

        private void SetRobertsRender_Click(object sender, EventArgs e)
        {
            mesh.RenderMode(1);
        }

        private void SetZBuffRender_Click(object sender, EventArgs e)
        {
            mesh.RenderMode(2);
        }

        private void SetColorDefault_Click(object sender, EventArgs e)
        {
            mesh.ColorMode(0);
        }

        private void SetMulticolor_Click(object sender, EventArgs e)
        {
            mesh.ColorMode(1);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            winRect.Height = this.Height;
            winRect.Width = this.Width;
            if(mesh!=null) mesh.InitiliseWBuffer(winRect.Width, winRect.Height);
        }

       
    }
}

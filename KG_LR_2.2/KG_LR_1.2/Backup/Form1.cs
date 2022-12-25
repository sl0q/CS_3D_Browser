using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myGraphics;

namespace KG_LR_1._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;
        }

        static string inputFile = "cube.obj";
        Graphics g;
        RECT winRect;
        Mesh mesh = new Mesh(inputFile);
        Pen mainPen = new Pen(Color.Aqua, 3);
        //Color background = new Color(TVBlack);
        float SPEED = 0.1f;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            winRect.Height = this.Height;
            winRect.Width = this.Width;
            g.Clear(Color.MidnightBlue);
            mesh.drawObject(g, winRect, mainPen);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.W)// 'W'
                mesh.move(mesh, Axis.axY, SPEED);
            if (e.KeyValue == (char)Keys.S)// 'S'
                mesh.move(mesh, Axis.axY, -SPEED);
            if (e.KeyValue == (char)Keys.A)// 'A'
                mesh.move(mesh, Axis.axX, -SPEED);
            if (e.KeyValue == (char)Keys.D)// 'D'
                mesh.move(mesh, Axis.axX, SPEED);
            if (e.KeyValue == (char)Keys.Space)
                mesh.move(mesh, Axis.axZ, SPEED);
            if (e.Modifiers == Keys.Control)
                mesh.move(mesh, Axis.axZ, -SPEED);

            //Вращение вокруг глобальных осей координат
            if (e.KeyValue == (char)Keys.I)// 'I'
                mesh.rotate_global(mesh, Axis.axX, 4);
            if (e.KeyValue == (char)Keys.K)// 'K'
                mesh.rotate_global(mesh, Axis.axX, -4);
            if (e.KeyValue == (char)Keys.L)// 'L'
                mesh.rotate_global(mesh, Axis.axZ, 4);
            if (e.KeyValue == (char)Keys.J)// 'J'
                mesh.rotate_global(mesh, Axis.axZ, -4);
            if (e.KeyValue == (char)Keys.O)// 'O'
                mesh.rotate_global(mesh, Axis.axY, 4);
            if (e.KeyValue == (char)Keys.U)// 'U'
                mesh.rotate_global(mesh, Axis.axY, -4);
            // 
            //Вращение вокруг локальных осей координат
            if (e.KeyValue == (char)Keys.Up)
                mesh.rotate_local(mesh, Axis.axX, 4);
            if (e.KeyValue == (char)Keys.Down)
                mesh.rotate_local(mesh, Axis.axX, -4);
            if (e.KeyValue == (char)Keys.Right)
                mesh.rotate_local(mesh, Axis.axZ, 4);
            if (e.KeyValue == (char)Keys.Left)
                mesh.rotate_local(mesh, Axis.axZ, -4);
            if (e.KeyValue == (char)Keys.E)// 'E'
                mesh.rotate_local(mesh, Axis.axY, 4);
            if (e.KeyValue == (char)Keys.Q)// 'Q'
                mesh.rotate_local(mesh, Axis.axY, -4);

            //Масштабирование
            if (e.KeyValue == (char)Keys.R)// 'R'
                mesh.scale(mesh, 1.05f);
            if (e.KeyValue == (char)Keys.F)// 'F'
                mesh.scale(mesh, 0.95f);
            Refresh();
        }
    }
}

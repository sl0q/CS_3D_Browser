using System.Drawing;
using System.Collections.Generic;

namespace myGraphics
{
    partial class Mesh
    {
        private void ToScreenPxl(Rectangle winRect)
        {
            Point buff = new Point();
            if (options.perspective)
                for (int i = 0; i < vertices.rows; i++)
                {
                    buff.X = (int)(-vertices[i, 0] * winRect.Height / (-d + vertices[i, 2]) + winRect.Width / 2);
                    buff.Y = (int)(vertices[i, 1] * winRect.Height / (-d + vertices[i, 2]) + winRect.Height / 2);
                    convPos[i] = buff;
                }
            else
            {
                for (int i = 0; i < vertices.rows; i++)
                {
                    buff.X = (int)(-vertices[i, 0] * winRect.Height / -d + winRect.Width / 2);
                    buff.Y = (int)(vertices[i, 1] * winRect.Height / -d + winRect.Height / 2);
                    convPos[i] = buff;
                }
            }
        }
        private void DrawFace(Graphics g, List<int> tempFace, Pen pen, SolidBrush brush = null)
        {

            if (tempFace[tempFace.Count - 1] < 0)
            {
                tempFace[tempFace.Count - 1] = 1;
                return;
            }

            int i;
            Point[] face = new Point[tempFace.Count - 1];
            for (i = 0; i < tempFace.Count - 1; i++)
                face[i] = convPos[tempFace[i]];
            if (brush != null)
                g.FillPolygon(brush, face);
            for (i = 0; i < tempFace.Count - 2; i++)
                g.DrawLine(pen, convPos[tempFace[i]], convPos[tempFace[i + 1]]);
            g.DrawLine(pen, convPos[tempFace[i]], convPos[tempFace[0]]);
        }

        public void DrawObject(Graphics g, Rectangle winRect, Color borderColor, Color faceColor)
        {
            //  0 - without invisible lines deleting (wireframe)
            //  1 - Roberts' algorithm
            //  2 - Z-buffer
            switch (options.renderMode)
            {
                case 0:
                    {
                        Pen pen = new Pen(borderColor);
                        ToScreenPxl(winRect);
                        for (int i = 0; i < indices.Count; i++)
                            DrawFace(g, indices[i], pen);
                        break;
                    }
                case 1:
                    {
                        Roberts();
                        ToScreenPxl(winRect);
                        Pen pen = new Pen(borderColor);
                        SolidBrush brush = new SolidBrush(default);
                        if (options.colorMode == 0)
                        {
                            faceColor = defaultColor;
                            brush.Color = faceColor;
                            for (int i = 0; i < indices.Count; i++)
                                DrawFace(g, indices[i], pen, brush);
                        }
                        else if (options.colorMode == 1)
                        {
                            for (int i = 0; i < indices.Count; i++)
                            {
                                brush.Color = colorKit[i % colorKit.Count];
                                DrawFace(g, indices[i], pen, brush);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        SolidBrush borderBrush = new SolidBrush(borderColor);
                        SolidBrush mainBrush = new SolidBrush(default);
                        ClearWBuffer();
                        ToScreenPxl(winRect);
                        if (options.colorMode == 0)
                        {
                            faceColor = defaultColor;
                            mainBrush.Color = faceColor;
                            for (int i = 0; i < indices.Count; i++)
                                DrawFaceZBuff(g, indices[i], borderBrush, mainBrush);
                        }
                        else if(options.colorMode == 1)
                        {
                            for (int i = 0; i < indices.Count; i++)
                            {
                                mainBrush.Color = colorKit[i % colorKit.Count];
                                DrawFaceZBuff(g, indices[i], borderBrush, mainBrush);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void Roberts()
        {
            for (int i = 0; i < indices.Count; i++)
            {
                double Nz = vertices[indices[i][0], 0] * (vertices[indices[i][1], 1] - vertices[indices[i][2], 1]) +
                           vertices[indices[i][1], 0] * (vertices[indices[i][2], 1] - vertices[indices[i][0], 1]) +
                           vertices[indices[i][2], 0] * (vertices[indices[i][0], 1] - vertices[indices[i][1], 1]);
                if (Nz <= 0)
                    indices[i][indices[i].Count - 1] = -1;
                else
                    indices[i][indices[i].Count - 1] = 1;
            }
        }
    }
}
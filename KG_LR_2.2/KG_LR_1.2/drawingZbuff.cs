using System;
using System.Drawing;
using System.Collections.Generic;

namespace myGraphics
{
    partial class Mesh
    {
        private void DrawFaceZBuff(Graphics g, List<int> tempFace, SolidBrush borderBrush, SolidBrush mainBrush)
        {
            if (tempFace[tempFace.Count - 1] < 0)
                return;

            int i,
                ymin = int.MaxValue,
                ymax = int.MinValue;
            Point[] face = new Point[tempFace.Count - 1];
            for (i = 0; i < face.Length; i++)
            {
                face[i] = convPos[tempFace[i]];
                if (face[i].Y < ymin) {
                    if (face[i].Y < 0)
                        ymin = 0;
                    else if (face[i].Y>WbufferHeight)
                        ymin = WbufferHeight - 1;
                    else
                        ymin = face[i].Y;
                }

                if (face[i].Y > ymax)
                {
                    if(face[i].Y < 0)
                        ymax = 0;
                    else if (face[i].Y >= WbufferHeight)
                        ymax = WbufferHeight - 1;
                    else
                        ymax = face[i].Y;
                }
            }
            if (ymin != ymax)//  face is degenerate
            {    
                tXW[] xmin = new tXW[ymax - ymin];
                tXW[] xmax = new tXW[ymax - ymin];
                for (int y = 0; y < ymax - ymin; y++)
                {
                    xmin[y].xm = int.MaxValue;
                    xmax[y].xm = 0;
                }

                //  contour traversal
                for (i = 0; i < face.Length - 1; i++)
                {
                    if (face[i].Y != face[i + 1].Y)
                        LineW(face[i], vertices[tempFace[i], 2], face[i + 1], vertices[tempFace[i + 1], 2], xmin, xmax, ymin, ymax);
                }
                if (face[i].Y != face[0].Y)
                    LineW(face[i], vertices[tempFace[i], 2], face[0], vertices[tempFace[0], 2], xmin, xmax, ymin, ymax);

                for (int y = ymin; y < ymax; y++)
                    HLineW(g, y, ymin, xmin, xmax, borderBrush, mainBrush);
            }
        }

        private void LineW(Point V1, float z1, Point V2, float z2, tXW[] xmin, tXW[] xmax, int ymin, int ymax)
        {
            float w, dw = 0, k = 0, xf;
            int x, y, yend;
            if (V1.Y < V2.Y)
            {
                if (V1.Y < ymin)
                    y = ymin;
                else
                    y = V1.Y;
                xf = V1.X;
                if (V2.Y > ymax)
                    yend = ymax;
                else
                    yend = V2.Y;
                w = 1 / (d - z1);
                dw = (1 / (d - z2) - w) / (V2.Y - V1.Y);
                k = (float)(V2.X - V1.X) / (V2.Y - V1.Y);
            }
            else
            {
                if (V2.Y < ymin)
                    y = ymin;
                else
                    y = V2.Y;
                xf = V2.X;
                if (V1.Y > ymax)
                    yend = ymax;
                else
                    yend = V1.Y;
                w = 1 / (d - z2);
                if (V1.Y != V2.Y)
                {
                    dw = (1 / (d - z1) - w) / (V1.Y - V2.Y);
                    k = (float)(V2.X - V1.X) / (V2.Y - V1.Y);
                }
            }

            for (int i = y; i < yend; i++)
            {
                x = (int)xf;
                if (x < xmin[i - ymin].xm)
                {
                    xmin[i - ymin].W = w;
                    if (x < 0)
                        xmin[i - ymin].xm = 0;
                    else if (x >= WbufferWidth)
                        xmin[i - ymin].xm = WbufferWidth - 1;
                    else
                        xmin[i - ymin].xm = x;
                    
                    
                }
                if (x >= xmax[i - ymin].xm)
                {
                    xmax[i - ymin].W = w;
                    if (x < 0)
                        xmax[i - ymin].xm = 0;
                    else if (x >= WbufferWidth)
                        xmax[i - ymin].xm = WbufferWidth - 1;
                    else
                        xmax[i - ymin].xm = x;
                }
                xf = xf + k;
                w += dw;
            }
        }

        private void HLineW(Graphics g, int y, int ymin, tXW[] xmin, tXW[] xmax, SolidBrush borderBrush, SolidBrush mainBrush)
        {
            float eps = 0.000001f,
                  dw = 0;
            int line = y - ymin;
            int dx = xmax[line].xm - xmin[line].xm;
            if (dx != 0)
                dw = (xmax[line].W - xmin[line].W) / dx;

            float w = xmin[line].W;

            if (xmin[line].W > Wbuffer[xmin[line].xm, y])    //  first pixel of the line painting into border color
            {
                Wbuffer[xmin[line].xm, y] = xmin[line].W + eps;
                g.FillRectangle(borderBrush, xmin[line].xm, y, 1, 1);
            }
            w += dw;
            for (int x = xmin[line].xm + 1; x < xmax[line].xm; x++)
            {
                if (w > Wbuffer[x, y])
                {
                    Wbuffer[x, y] = w + eps;
                    g.FillRectangle(mainBrush, x, y, 1, 1);
                }
                w += dw;
            }
            if (xmax[line].W > Wbuffer[xmax[line].xm, y])    //  last pixel of the line painting into border color
            {
                Wbuffer[xmax[line].xm, y] = xmax[line].W + eps;
                g.FillRectangle(borderBrush, xmax[line].xm, y, 1, 1);
            }
        }
    }
}
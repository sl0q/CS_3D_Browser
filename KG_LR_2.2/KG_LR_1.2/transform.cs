using System;

namespace myGraphics
{
    partial class Mesh
    {
        public void move(float dx, float dy, float dz)
        {
            if (dx == 0 && dy == 0 && dz == 0)
                return;

            transMatrix.Fill_identity();
            transMatrix[3, 0] = dx;
            transMatrix[3, 1] = dy;
            transMatrix[3, 2] = dz;
            this.posMatrix = this.posMatrix * transMatrix;
            this.vertices = this.vertices * transMatrix;
        }

        public void rotate_global(float angle, Axis axis)
        {
            if (angle == 0)
                return;

            angle = (float)(angle * Math.PI / 180); // from deg to rad
            float Cos = (float)Math.Cos(angle),
                Sin = (float)Math.Sin(angle);

            transMatrix.Fill(0);
            switch (axis)
            {
                case Axis.axX:
                    transMatrix[0, 0] = 1;
                    transMatrix[1, 1] = Cos;
                    transMatrix[1, 2] = Sin;
                    transMatrix[2, 1] = -Sin;
                    transMatrix[2, 2] = Cos;
                    transMatrix[3, 3] = 1;
                    break;
                case Axis.axY:
                    transMatrix[0, 0] = Cos;
                    transMatrix[0, 2] = -Sin;
                    transMatrix[1, 1] = 1;
                    transMatrix[2, 0] = Sin;
                    transMatrix[2, 2] = Cos;
                    transMatrix[3, 3] = 1;
                    break;
                case Axis.axZ:
                    transMatrix[0, 0] = Cos;
                    transMatrix[0, 1] = Sin;
                    transMatrix[1, 0] = -Sin;
                    transMatrix[1, 1] = Cos;
                    transMatrix[2, 2] = 1;
                    transMatrix[3, 3] = 1;
                    break;
                default:
                    transMatrix.Fill_identity();
                    break;
            }
            this.posMatrix = this.posMatrix * transMatrix;
            this.vertices = this.vertices * transMatrix;
        }

        public void rotate_local(float angle, Axis axis)
        {
            if (angle == 0)
                return;

            float dx = this.posMatrix[0, 0],
                dy = this.posMatrix[0, 1],
                dz = this.posMatrix[0, 2];

            angle = (float)(angle * Math.PI / 180); // from deg to rad
            float Cos = (float)Math.Cos(angle),
                Sin = (float)Math.Sin(angle);

            transMatrix.Fill(0);
            switch (axis)
            {
                case Axis.axX:
                    transMatrix[0, 0] = 1;
                    transMatrix[1, 1] = Cos;
                    transMatrix[1, 2] = Sin;
                    transMatrix[2, 1] = -Sin;
                    transMatrix[2, 2] = Cos;
                    transMatrix[3, 1] = -dy * Cos + dz * Sin + dy;
                    transMatrix[3, 2] = -dy * Sin - dz * Cos + dz;
                    transMatrix[3, 3] = 1;
                    break;
                case Axis.axY:
                    transMatrix[0, 0] = Cos;
                    transMatrix[0, 2] = -Sin;
                    transMatrix[1, 1] = 1;
                    transMatrix[2, 0] = Sin;
                    transMatrix[2, 2] = Cos;
                    transMatrix[3, 0] = -dx * Cos - dz * Sin + dx;
                    transMatrix[3, 2] = dx * Sin - dz * Cos + dz;
                    transMatrix[3, 3] = 1;
                    break;
                case Axis.axZ:
                    transMatrix[0, 0] = Cos;
                    transMatrix[0, 1] = Sin;
                    transMatrix[1, 0] = -Sin;
                    transMatrix[1, 1] = Cos;
                    transMatrix[2, 2] = 1;
                    transMatrix[3, 0] = -dx * Cos + dy * Sin + dx;
                    transMatrix[3, 1] = -dx * Sin - dy * Cos + dy;
                    transMatrix[3, 3] = 1;
                    break;
                default:
                    transMatrix.Fill_identity();
                    break;
            }
            this.vertices = this.vertices * transMatrix;
        }

        public void scale(float factor)
        {
            if (factor == 1)
                return;

            transMatrix.Fill(0);
            transMatrix[0, 0] = factor;
            transMatrix[1, 1] = factor;
            transMatrix[2, 2] = factor;
            transMatrix[3, 3] = 1;

            this.vertices = this.vertices * transMatrix;
            this.posMatrix = this.posMatrix * transMatrix;
        }
    }
}
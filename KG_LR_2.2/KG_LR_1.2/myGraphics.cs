using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Matrix;

namespace myGraphics
{
    enum Axis : int
    {
        None = -1,
        X = 0,
        Y = 1,
        Z = 2,
        axX = 1,
        axY = 2,
        axZ = 3
    }

    struct Options
    {
        public bool perspective;
        public int renderMode;
        public int colorMode;

        public Options(int mode = 0)
        {
            if (mode == 0)
            {
                perspective = false;
                renderMode = 0;
                colorMode = 0;
            }
            else
            {
                perspective = true;
                renderMode = mode % 3;
                colorMode = mode % 2;
            }
        }
    }
    
    struct tXW
    {
        public int xm;
        public float W;
    }
    partial class Mesh
    {
        private MatrixF vertices = new MatrixF();   // array of coordinates of vertices
        private List<List<int>> indices = new List<List<int>>();    // polygon array (indices)
        private List<Point> convPos;    // positions of points, converted into 2D pixel coordinates
        private MatrixF posMatrix = new MatrixF();  // position of object relative to the global origin
        private float d = 7;    
        private MatrixF transMatrix = new MatrixF();// general matrix of transforming
        Options options;
        private float[,] Wbuffer;
        private int WbufferWidth, WbufferHeight;
        private Color defaultColor = Color.DarkCyan;
        private List<Color> colorKit;
        static Random random;

        public Mesh()
        {
            posMatrix.Resize(1, 4);
            posMatrix[0, 3] = 1;
            transMatrix.Resize(4, 4);
        }

        public Mesh(string fileName)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            posMatrix.Resize(1, 4);
            posMatrix[0, 3] = 1;
            transMatrix.Resize(4, 4);
            options = new Options();
            ReadFile(fileName);
            colorKit = new List<Color>();
            random = new Random();
            for (int i = 0; i < 10; i++)
                colorKit.Add(GenerateColor());
        }

        public void InitiliseWBuffer(int width, int height)
        {
            WbufferWidth = width;
            WbufferHeight = height;
            Wbuffer = new float[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Wbuffer[i, j] = float.NegativeInfinity;
        }

        private void ClearWBuffer()
        {
            for (int i = 0; i < WbufferWidth; i++)
                for (int j = 0; j < WbufferHeight; j++)
                    Wbuffer[i, j] = float.NegativeInfinity;
        }

        private void ReadFile(string filename)
        {
            
            if (!File.Exists(filename))
            {
                System.Windows.Forms.MessageBox.Show("File " + filename + " doesn't exists!", "Error");
                System.Environment.Exit(-1);
            }
            StreamReader myFile = new StreamReader(filename);
            
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            string line;

            while((line = myFile.ReadLine()) != null)
            {
                string[] subLine = line.Split();

                if (subLine[0] == "v")
                {
                    List<float> tempV = new List<float>{ 0, 0, 0, 1 };
                    tempV[0] = float.Parse(subLine[1], NumberStyles.Any, ci);
                    tempV[1] = float.Parse(subLine[2], NumberStyles.Any, ci);
                    tempV[2] = float.Parse(subLine[3], NumberStyles.Any, ci);          
                    vertices.PushBackRow(tempV);
                }
                else if (subLine[0] == "f")
                {
                    int faceType = subLine.Length - 1;
                    List<int> tempFace = new List<int>(faceType + 1);
                    string[] temp_index;

                    for(int i = 1;i<=faceType;i++)
                    {
                        temp_index = subLine[i].Split('/');
                        tempFace.Add(int.Parse(temp_index[0]) - 1);
                    }
                    tempFace.Add(1);
                    indices.Add(tempFace);
                }
            }
            myFile.Close();
            int vertNum = vertices.rows;
            convPos = new List<Point>(vertNum);
            for (int i = 0; i < vertNum; i++)
                convPos.Add(new Point());
        }

        private Color GenerateColor()
        {
            return Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        }
    }
}

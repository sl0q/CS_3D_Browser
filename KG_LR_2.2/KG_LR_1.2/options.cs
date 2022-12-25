using System.Drawing;

namespace myGraphics
{
    partial class Mesh
    {
        public void PerspectiveOn(bool flag)
        {
            options.perspective = flag;
        }

        public void RenderMode(int flag)
        //  0 - without deleting of invisible lines
        //  1 - Roberts' algorithm
        //  2 - Z-buffer
        {
            options.renderMode = flag % 3;
        }

        public void ColorMode(int flag)
        {
            options.colorMode = flag % 2;
        }

        public void SetDefaultColor(Color newColor)
        {
            defaultColor = newColor;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rayCasterTest
{
    public class Camera
    { 
        public int width;
        public int height;
        public int resoultion;
        public double spacing;
        public double fov;
        public double range;
        public double lightRange;
        public double scale;

        public Camera(int SCREEN_WIDTH, int SCREEN_HEIGHT, int SCREEN_RESOLUTION, double CAMERA_FOV)
        {
            width = SCREEN_WIDTH;
            height = SCREEN_HEIGHT;
            resoultion = SCREEN_RESOLUTION;
            spacing = SCREEN_WIDTH / SCREEN_RESOLUTION;
            fov = CAMERA_FOV;
            range = 64;
            lightRange = 32;
            scale = (width + height) / 1200;
        }
    }
}

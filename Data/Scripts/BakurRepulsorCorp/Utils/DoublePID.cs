using VRageMath;

namespace BakurRepulsorCorp {

    public class DoublePID {

        public double Kp = 0.6;
        public double Ki = 0;
        public double Kd = 0.2;

        public double lastOutput;

        public bool clampOutput = true;
        public double minOutput = -1;
        public double maxOutput = 1;
        double _prevDx;
        double _accumDx;

        public void Reset() {
            _prevDx = 0;
            _accumDx = 0;
        }

        public double UpdateValue(double input, double desired, double deltaTime) {

            double error = desired - input;
            return UpdateValue(error, deltaTime);
        }

    
        public double UpdateValue(double error, double deltaTime) {

            double dx = error;
            double ddx = (dx - _prevDx) / deltaTime;
            _accumDx = 0.99f * _accumDx + dx;
            _prevDx = dx;
            double f = Kp * dx + Ki * _accumDx + Kd * ddx;          
            if (clampOutput) {
                f = MathHelper.Clamp(f, minOutput, maxOutput);
            }
            lastOutput = f;
            return f;
        }
    }
}
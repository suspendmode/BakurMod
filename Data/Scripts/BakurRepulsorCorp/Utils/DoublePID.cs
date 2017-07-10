using VRageMath;

namespace BakurRepulsorCorp {

    public class DoublePID {

        public double Kp = 0.1;
        public double Ki = 0;
        public double Kd = 0.01;

        public double lastError;
        public double integral;
        public double derivative;

        public double lastOutput;

        public bool clampOutput = false;
        public double minOutput = 0;
        public double maxOutput = 0;

        public void Reset() {
            lastError = 0;
        }

        public double UpdateValue(double input, double desired, double deltaTime) {

            double error = desired - input;

            return UpdateValue(error, deltaTime);
        }

        public double UpdateValue(double error, double deltaTime) {

            double output = 0;

            output = ((Kp * error) + (Ki * integral)) + (Kd * derivative);

            if (clampOutput) {
                output = MathHelper.Clamp(output, minOutput, maxOutput);
            }

            if (clampOutput && output >= maxOutput || output <= minOutput) {
                integral += (error * deltaTime);
            }

            lastOutput = output;

            derivative = (error - lastError) / deltaTime;

            lastError = error;

            return output;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PlanetRadarSensor : PlanetSensor {

        public PlanetRadarSensor(BakurBlock component) : base(component) {
        }

        public override void Debug() {
            //MyAPIGateway.Session.GetWorld().Sector.Environment.SunAzimuth
            //MyAPIGateway.Session.GetWorld().Sector.sun

                      //Vector3 sunDirection;
            //Vector3.CreateFromAzimuthAndElevation(environmentBuilder.SunAzimuth, environmentBuilder.SunElevation, out sunDirection);
            //sunDirection.Normalize();

        }

        public override void Destroy() {
            
        }

        public override void Initialize() {
            
        }        
    }
}
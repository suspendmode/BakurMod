using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using Sandbox.Game.Entities;

namespace BakurRepulsorCorp {

    public class AntiGravityGenerator : EquipmentBase {

        public MyEntity3DSoundEmitter soundEmitter;
        string outputSoundId;
        MySoundPair outputSound;

        static Separator<AntiGravityGenerator> separator;
        static Label<AntiGravityGenerator> label;

        public AntiGravityGenerator(BakurBlock block) : base(block) {
        }

        #region output

        static AntiGravityGenerator_OutputSlider outputSlider;
        static AntiGravityGenerator_IncraseOutputAction incraseOutputAction;
        static AntiGravityGenerator_DecraseOutputAction decraseOutputAction;

        public static string OUTPUT_PROPERTY_NAME = "AntiGravityGenerator_Output";

        public double defaultOutput = 0.1;

        public double output
        {
            set
            {
                string id = GeneratatePropertyId(OUTPUT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(OUTPUT_PROPERTY_NAME);
                double result = defaultOutput;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultOutput;
            }
        }

        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {

        }

        public override void Debug() {

        }

        public override void Initialize() {
            if (separator == null) {
                separator = new Separator<AntiGravityGenerator>("AntiGravityGenerator_Separator");
                separator.Initialize();
            }

            if (label == null) {
                label = new Label<AntiGravityGenerator>("AntiGravityGenerator_Label", "AntiGravity Generator");
                label.Initialize();
            }


            // output

            if (outputSlider == null) {
                outputSlider = new AntiGravityGenerator_OutputSlider();
                outputSlider.Initialize();
            }

            if (incraseOutputAction == null) {
                incraseOutputAction = new AntiGravityGenerator_IncraseOutputAction();
                incraseOutputAction.Initialize();
            }

            if (decraseOutputAction == null) {
                decraseOutputAction = new AntiGravityGenerator_DecraseOutputAction();
                decraseOutputAction.Initialize();
            }

            // sounds
            /*
            if (soundEmitter == null) {
                soundEmitter = new MyEntity3DSoundEmitter(block as VRage.Game.Entity.MyEntity);
            }
            this.soundEmitter.Force3D = true;
            if (outputSoundId != null) {
                outputSound = PreloadSound(outputSoundId);
            }
            */
        }

        protected MySoundPair PreloadSound(string soundId) {
            MySoundPair pair = new MySoundPair(soundId);
            MyEntity3DSoundEmitter.PreloadSound(pair);
            return pair;
        }


        public override void Destroy() {

        }

        public float GetOutput(float supply) {

            //if (isPowered) {

            //}
            if (outputSound != null) {
                soundEmitter.PlaySound(outputSound, true);
            }

            return (float)output;
        }
    }
}
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorJammer : EquipmentBase {

        #region rangle

        public static Jammer_RangeSlider rangeSlider;
        static Jammer_IncraseRangeAction incraseRangeAction;
        static Jammer_DecraseRangeAction decraseRangeAction;

        public static string JAMMER_PROPERTY_NAME = "RepulsorJammer_Rangle";

        public double defaultRange = 20;

        public RepulsorJammer(BakurBlock block) : base(block) {
        }

        public double range
        {
            set
            {
                string id = GeneratatePropertyId(JAMMER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(JAMMER_PROPERTY_NAME);
                double result = defaultRange;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultRange;
            }

        }

        #endregion

        static Separator<RepulsorJammer> jammerSeparator;
        static Label<RepulsorJammer> jammerLabel;

        #region lifecycle

        public override void Initialize() {


            // jammer

            if (jammerSeparator == null) {
                jammerSeparator = new Separator<RepulsorJammer>("RepulsorJammer_Separator");
                jammerSeparator.Initialize();
            }

            if (jammerLabel == null) {
                jammerLabel = new Label<RepulsorJammer>("RepulsorJammer_Label", "Repulsor Jammer");
                jammerLabel.Initialize();
            }

            if (rangeSlider == null) {
                rangeSlider = new Jammer_RangeSlider();
                rangeSlider.Initialize();
            }

            if (incraseRangeAction == null) {
                incraseRangeAction = new Jammer_IncraseRangeAction();
                incraseRangeAction.Initialize();
            }

            if (decraseRangeAction == null) {
                decraseRangeAction = new Jammer_DecraseRangeAction();
                decraseRangeAction.Initialize();
            }

        }

        public override void Destroy() {

        }

        #endregion      

        #region update

        BoundingSphereD boundingSpehere;

        public void UpdateJammer() {

            if (!component.enabled) {
                return;
            }

            if (boundingSpehere == null) {
                boundingSpehere = new BoundingSphereD();
            }
            boundingSpehere.Center = block.WorldMatrix.Translation;
            boundingSpehere.Radius = range;

            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSpehere);
            foreach (IMyEntity entity in entities) {
                if (entity is IMyCubeGrid) {

                    IMyCubeGrid grid = entity as IMyCubeGrid;
                    if (grid == component.block.CubeGrid) {
                        continue;
                    }

                    List<IMySlimBlock> blocks = new List<IMySlimBlock>();
                    grid.GetBlocks(blocks, (IMySlimBlock block) => {

                        if (block == null || block.FatBlock == null) {
                            return false;
                        }

                        BakurBlock bakurBlock = block.FatBlock.GameLogic.GetAs<BakurBlock>();
                        if (bakurBlock == null) {
                            return false;
                        }

                        return true;

                    });
                    foreach (IMySlimBlock block in blocks) {
                        if (block == null || block.FatBlock == null) {
                            continue;
                        }
                        TypeDisableBlock(block.FatBlock);
                    }
                } else {
                    continue;
                }
            }
        }

        void TypeDisableBlock(IMyCubeBlock block) {
            TypeDisable<RepulsorCoilBlock>(block);
            TypeDisable<RepulsorGeneratorBlock>(block);
            TypeDisable<RepulsorLiftBlock>(block);
            TypeDisable<RepulsorLiftCoilBlock>(block);
            TypeDisable<RepulsorLiftGeneratorBlock>(block);
            TypeDisable<RepulsorSuspensionBlock>(block);
            TypeDisable<RepulsorChairBlock>(block);
        }

        void TypeDisable<T>(IMyCubeBlock block) where T : BakurBlock {
            T result = block.GameLogic.GetAs<T>();
            if (result != null && result.enabled) {
                result.enabled = false;
            }
        }


        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();

            customInfo.AppendLine("== Repulsor Jammer ==");
            customInfo.AppendLine("Range : " + range);
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }

            DebugDraw.DrawSphere(block.WorldMatrix.Translation, block.WorldMatrix.Forward, block.WorldMatrix.Up, Color.BlueViolet, (float)range);
        }
    }
}
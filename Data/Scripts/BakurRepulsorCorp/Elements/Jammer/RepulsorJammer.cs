using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorJammer : LogicElement
    {

        public RepulsorJammer(LogicComponent block) : base(block)
        {
        }

        #region use jammer

        public readonly string USE_JAMMER_PROPERTY_NAME = "RepulsorJammer_Use";

        public bool defaultUseJammer = true;

        public bool useJammer
        {
            set
            {
                string id = GeneratePropertyId(USE_JAMMER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_JAMMER_PROPERTY_NAME);
                bool result = defaultUseJammer;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseJammer;
            }

        }

        #endregion

        #region rangle

        public readonly string JAMMER_PROPERTY_NAME = "RepulsorJammer_Rangle";

        public double defaultRange = 20;

        public double range
        {
            set
            {
                string id = GeneratePropertyId(JAMMER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(JAMMER_PROPERTY_NAME);
                double result = defaultRange;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultRange;
            }

        }

        #endregion

        #region lifecycle

        public override void Initialize()
        {

        }

        public override void Destroy()
        {

        }

        #endregion      

        #region update

        BoundingSphereD boundingSpehere;

        public void UpdateJammer()
        {

            if (!logicComponent.enabled || !useJammer)
            {
                return;
            }

            if (boundingSpehere == null)
            {
                boundingSpehere = new BoundingSphereD();
            }
            boundingSpehere.Center = block.WorldMatrix.Translation;
            boundingSpehere.Radius = range;

            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSpehere);
            foreach (IMyEntity entity in entities)
            {
                if (entity is IMyCubeGrid)
                {

                    IMyCubeGrid grid = entity as IMyCubeGrid;
                    if (grid == logicComponent.block.CubeGrid)
                    {
                        continue;
                    }

                    List<IMySlimBlock> blocks = new List<IMySlimBlock>();
                    grid.GetBlocks(blocks, (IMySlimBlock block) =>
                    {

                        if (block == null || block.FatBlock == null)
                        {
                            return false;
                        }

                        LogicComponent bakurBlock = block.FatBlock.GameLogic.GetAs<LogicComponent>();
                        if (bakurBlock == null)
                        {
                            return false;
                        }

                        return true;

                    });
                    foreach (IMySlimBlock block in blocks)
                    {
                        if (block == null || block.FatBlock == null)
                        {
                            continue;
                        }
                        TypeDisableBlock(block.FatBlock);
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        void TypeDisableBlock(IMyCubeBlock block)
        {
            TypeDisable<AntiGravityGeneratorComponent>(block);
            TypeDisable<RepulsorCoilComponent>(block);
            TypeDisable<RepulsorDriveComponent>(block);
            TypeDisable<RepulsorLiftComponent>(block);
            TypeDisable<RepulsorLiftCoilComponent>(block);
            TypeDisable<RepulsorLiftDriveComponent>(block);
            TypeDisable<RepulsorSuspensionComponent>(block);
            TypeDisable<RepulsorChairComponent>(block);
        }

        void TypeDisable<T>(IMyCubeBlock block) where T : LogicComponent
        {
            T result = block.GameLogic.GetAs<T>();
            if (result != null && result.enabled)
            {
                result.enabled = false;
            }
        }


        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();

            customInfo.AppendLine("== Repulsor Jammer ==");
            customInfo.AppendLine("Range : " + range);
        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }

            DebugDraw.DrawSphere(block.WorldMatrix.Translation, block.WorldMatrix.Forward, block.WorldMatrix.Up, Color.BlueViolet, (float)range);
        }
    }
}

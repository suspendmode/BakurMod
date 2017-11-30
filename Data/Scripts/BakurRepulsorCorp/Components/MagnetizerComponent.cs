﻿using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;


namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockMagnetizer", "LargeBlockMagnetizer" })]
    public class MagnetizerComponent : LogicComponent
    {

        private static readonly string[] subTypeIds = { "SmallBlockMagnetizer", "LargeBlockMagnetizer" };

        Magnetizer magnetizer;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            magnetizer = new Magnetizer(this);
            AddEquipment(magnetizer);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(magnetizer);
            magnetizer = null;
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Magnetizer Component");
            base.AppendCustomInfo(block, customInfo);
        }



        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            magnetizer.UpdateMagnetizer(physicsDeltaTime);

            // magnetizer

        }


        protected override string[] soundIds
        {
            get
            {
                return new string[] { "magnetizer_working_start", "magnetizer_working_loop", "magnetizer_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("4bab8c9c-0b6b-401e-b40b-c73667e5cabb");
        }
    }

}
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class BlockSounds : EquipmentBase {

        public MyEntity3DSoundEmitter soundEmitter;
        string startSoundId;
        string loopSoundId;
        string endSoundId;

        MySoundPair startSound;
        MySoundPair loopSound;
        MySoundPair endSound;

        public BlockSounds(BakurBlock component, string startSoundId, string loopSoundId, string endSoundId) : base(component) {
            this.startSoundId = startSoundId;
            this.loopSoundId = loopSoundId;
            this.endSoundId = endSoundId;
        }

        #region lifecycle

        public override void Initialize() {
            playingStartSound = false;
            playingLoopSound = false;
            playingEndSound = false;

            if (soundEmitter == null) {
                soundEmitter = new MyEntity3DSoundEmitter(block as VRage.Game.Entity.MyEntity);
            }
            this.soundEmitter.Force3D = true;
            PreloadSounds();
        }

        public override void Destroy() {

            playingStartSound = false;
            playingLoopSound = false;
            playingEndSound = false;
            if (soundEmitter != null) {
                soundEmitter.StoppedPlaying -= StartSoundEnded;
                soundEmitter.StoppedPlaying -= EndSoundEnded;
                if (soundEmitter.IsPlaying) {
                    soundEmitter.StopSound(true);
                }
                soundEmitter = null;
            }
        }

        #endregion

        void PreloadSounds() {
            if (startSoundId != null) {
                startSound = PreloadSound(startSoundId);
            }

            if (loopSoundId != null) {
                loopSound = PreloadSound(loopSoundId);
            }

            if (endSoundId != null) {
                endSound = PreloadSound(endSoundId);
            }
        }

        protected MySoundPair PreloadSound(string soundId) {
            MySoundPair pair = new MySoundPair(soundId);
            MyEntity3DSoundEmitter.PreloadSound(pair);
            return pair;
        }

        bool playingStartSound;
        bool playingLoopSound;
        bool playingEndSound;

        public void StartSound() {
            if (playingStartSound || playingLoopSound) {
                return;
            }
            playingStartSound = true;
            soundEmitter.StoppedPlaying += StartSoundEnded;
            if (startSound != null) {
                soundEmitter.PlaySingleSound(startSound, true);
            } else {
                NoSoundMessage("_start");
                StartSoundEnded(soundEmitter);
            }
        }

        void StartSoundEnded(MyEntity3DSoundEmitter obj) {
            playingStartSound = false;
            playingLoopSound = true;
            soundEmitter.StoppedPlaying -= StartSoundEnded;
            if (loopSound != null) {
                soundEmitter.PlaySound(loopSound, true);
            }
        }

        public void StopSound() {
            if (playingEndSound || !playingLoopSound) {
                return;
            }
            playingLoopSound = false;
            playingEndSound = true;
            soundEmitter.StoppedPlaying += EndSoundEnded;
            if (endSound != null) {
                soundEmitter.PlaySingleSound(endSound, true);
            } else {
                NoSoundMessage("_end");
                EndSoundEnded(soundEmitter);
            }
        }

        private void NoSoundMessage(string type) {
            MyAPIGateway.Utilities.ShowMessage("Sound", "No sound in " + block.BlockDefinition.SubtypeId + " type " + type);
        }

        void EndSoundEnded(MyEntity3DSoundEmitter obj) {
            soundEmitter.StoppedPlaying -= EndSoundEnded;
            playingEndSound = false;
            playingEndSound = false;
            soundEmitter.StopSound(true);
        }

        public virtual void UpdateSound() {
            if (block.IsWorking && component.enabled) {
                StartSound();
            } else {
                StopSound();
            }
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {

        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }
    }
}
using FMOD;
using System;
using System.Collections.Generic;

namespace WeaponsGame
{
	public class Audio
	{
		private System.Collections.Generic.Dictionary<string, Sound> sounds;

		private FMOD.System system;

		private Channel effectsChannel;

		private Channel musicChannel;

		private RESULT result;

		public Audio()
		{
			this.sounds = new System.Collections.Generic.Dictionary<string, Sound>();
			this.CheckFMODResult(Factory.System_Create(ref this.system));
			this.CheckFMODResult(this.system.init(255, INITFLAGS.NORMAL, (IntPtr)null));
			this.effectsChannel = new Channel();
			this.effectsChannel.setMode(MODE.LOOP_OFF);
			this.musicChannel = new Channel();
			this.musicChannel.setMode(MODE.LOOP_NORMAL);
		}

		public void Update()
		{
			this.system.update();
		}

		public void PlaySound(string name)
		{
			this.system.update();
			Sound sound = null;
			if (!this.sounds.ContainsKey(name))
			{
				this.CheckFMODResult(this.system.createSound(".\\Content\\Sound\\" + name, MODE._2D, ref sound));
				this.sounds.Add(name, sound);
			}
			else
			{
				sound = this.sounds[name];
			}
			this.CheckFMODResult(this.system.playSound(CHANNELINDEX.FREE, sound, false, ref this.effectsChannel));
			this.effectsChannel.setVolume(0.5f);
		}

		public void PlaySong(string name)
		{
			this.PlaySong(name, false);
		}

		public void PlaySong(string name, bool loop)
		{
			Sound sound = null;
			if (!this.sounds.ContainsKey(name))
			{
				this.CheckFMODResult(this.system.createStream(".\\Content\\Sound\\" + name, loop ? MODE.LOOP_NORMAL : MODE.DEFAULT, ref sound));
				this.sounds.Add(name, sound);
			}
			else
			{
				sound = this.sounds[name];
			}
			this.CheckFMODResult(this.system.playSound(CHANNELINDEX.FREE, sound, false, ref this.musicChannel));
		}

		public void StopSounds()
		{
			this.CheckFMODResult(this.effectsChannel.stop());
			this.CheckFMODResult(this.musicChannel.stop());
		}

		private void CheckFMODResult(RESULT res)
		{
			this.result = res;
			if (res != RESULT.OK)
			{
				System.Console.WriteLine("FMOD ERR:" + res.ToString());
			}
		}

		private Sound CreateSoundInstance(string path)
		{
			Sound sound = null;
			this.CheckFMODResult(this.system.createSound(path, MODE.DEFAULT, ref sound));
			return sound;
		}
	}
}

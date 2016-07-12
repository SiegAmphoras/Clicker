using System;

namespace WeaponsGame.Game
{
	public class ActionTimer
	{
		public float targetTime;

		public bool Started = false;

		public bool Enabled = true;

		public event ActionTimerEventHandler OnStart;

		public event ActionTimerEventHandler OnFinish;

		public float currentTime
		{
			get;
			protected set;
		}

		public float endTime
		{
			get;
			protected set;
		}

		public ActionTimer(float targetTime)
		{
			this.targetTime = targetTime;
		}

		public void Begin()
		{
			if (this.Enabled)
			{
				this.currentTime = 0f;
				this.Start();
			}
		}

		public void BumpTime(float interval)
		{
			this.currentTime += interval;
			if (this.currentTime > this.targetTime)
			{
				this.currentTime = this.targetTime;
			}
		}

		private void Start()
		{
			this.Started = true;
			this.currentTime = 0f;
			if (this.OnStart != null)
			{
				this.OnStart(this);
			}
		}

		private void Finish()
		{
			this.Started = false;
			this.endTime = Engine.ElapsedTime;
			this.currentTime = 0f;
			if (this.OnFinish != null)
			{
				this.OnFinish(this);
			}
		}

		public void Reset()
		{
			this.Started = false;
			this.currentTime = 0f;
			this.endTime = 0f;
		}

		public void Update()
		{
			if (this.Started)
			{
				if (this.currentTime < this.targetTime)
				{
					this.currentTime += Engine.DeltaTime;
				}
				else
				{
					this.Finish();
				}
			}
		}
	}
}

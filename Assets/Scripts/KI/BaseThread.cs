using System;
using System.Threading;

public abstract class BaseThread
{
	private Thread _thread;

	protected BaseThread() { _thread = new Thread(new ThreadStart(this.RunThread)); }

	// Thread methods / properties
	public void Start() { _thread.Start(); }
	public void Join() { _thread.Join(); }
	public bool IsAlive { get { return _thread.IsAlive; } }

	// Override in base class
	public abstract void RunThread();
}
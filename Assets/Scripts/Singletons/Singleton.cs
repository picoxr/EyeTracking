using UnityEngine;

	/// <summary>
	/// Singleton 模板，可直接继承此模板.
    /// 普通单例模板
	/// </summary>
	public class Singleton<T> : MonoBehaviour	where T : Component
	{
		protected static T _instance;

		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T> ();
					if (_instance == null)
					{
						GameObject obj = new GameObject ();
						_instance = obj.AddComponent<T> ();
					}
				}
				return _instance;
			}
		}

	    /// <summary>
	    /// OnAwake To Init.
	    /// </summary>
	    protected virtual void Awake ()
		{
			_instance = this as T;			
		}
	}

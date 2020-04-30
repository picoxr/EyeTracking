using UnityEngine;

    /// <summary>
    /// D singleton.模板，可直接继承此模板
    /// 切换各个Scene中仍然存在的模板，保持第一次创建此单例模板类的唯一性。
    /// </summary>
    public class DSingleton<T> : MonoBehaviour	where T : Component
	{
		protected static T _instance;

		/// <summary>
		/// DSingleton 
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
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T> ();
					}
				}
				return _instance;
			}
		}

	    /// <summary>
	    /// On awake init.
	    /// </summary>
	    protected virtual void Awake ()
		{


			if(_instance == null)
			{
				//If I am the first instance, make me the Singleton
				_instance = this as T;
				DontDestroyOnLoad (transform.gameObject);
			}
			else
			{
				//If a Singleton already exists and you find
				//another reference in scene, destroy it!
				if(this != _instance)
				{
					Destroy(this.gameObject);
				}
			}
		}
	}

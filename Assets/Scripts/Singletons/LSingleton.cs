using UnityEngine;
using System;

    /// <summary>
    /// L singleton.模板，可直接继承此模板
    /// 切换各个Scene中仍然存在的模板，保持最后一次创建此单例模板类的唯一性。
    /// </summary>
    public class LSingleton<T> : MonoBehaviour	where T : Component
	{
		protected static T _instance;
		public float InitializationTime;

		/// <summary>
		/// LSingleton  
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
                        obj.hideFlags = HideFlags.HideAndDontSave;
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
			InitializationTime=Time.time;

			DontDestroyOnLoad (this.gameObject);
			// we check for existing objects of the same type
			T[] check = FindObjectsOfType<T>();
			foreach (T searched in check)
			{
				if (searched!=this)
				{
					// if we find another object of the same type (not this), and if it's older than our current object, we destroy it.
					if (searched.GetComponent<LSingleton<T>>().InitializationTime<InitializationTime)
					{
						Destroy (searched.gameObject);
					}
				}
			}

			if (_instance == null)
			{
				_instance = this as T;
			}
		}
	}

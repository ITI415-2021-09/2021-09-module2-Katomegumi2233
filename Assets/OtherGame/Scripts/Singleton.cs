using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG
{
  
    
   
   
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        private static object _lock = new object();

        public virtual bool IsDont { get { return false; } }

        public static T Instance
        {
            get
            {


                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();
                        
                            if(_instance.IsDont)
                            DontDestroyOnLoad(singleton);
                        }
                        _instance.Init();
                    }

                    return _instance;
                }
            }
            set
            {
                _instance = value;
            }
        }

        //  private static bool applicationIsQuitting = false;





        public virtual void Init() { }

        public void OnDestroy()
        {
            //  applicationIsQuitting = true;
        }
    }
}



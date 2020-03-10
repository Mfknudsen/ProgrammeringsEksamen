#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


    [System.Serializable]
    public class Database : MonoBehaviour
    {
        #region Public Data
        public string AI_Name;

        public string[] Continues_AI_Actions;
        public string[] Continues_Async_AI_Actions;
        #endregion

        #region Private Data
        Master Master;
        #endregion

        public Database(Database data)
        {
            AI_Name = data.AI_Name;
        }
    }


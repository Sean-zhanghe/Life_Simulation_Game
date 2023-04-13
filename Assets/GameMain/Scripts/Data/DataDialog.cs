using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class DialogData
    {
        private DRDialog dRDialog;

        public int Id { get { return dRDialog.Id; } }

        public string DialogContent { get { return dRDialog.DialogContent; } }

        public string Parameter { get { return dRDialog.Parameter; } }

        public int NextDialog { get { return dRDialog.NextDialogId; } }

        public DialogData(DRDialog dRDialog)
        {
            this.dRDialog = dRDialog;
        }
    }

    #endregion

    public sealed class DataDialog : DataBase
    {
        private IDataTable<DRDialog> dtDialog;
        private Dictionary<int, DialogData> dicDialogData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtDialog = GameEntry.DataTable.GetDataTable<DRDialog>();
            if (dtDialog == null)
                throw new System.Exception("Can not get data table Sound");

            dicDialogData = new Dictionary<int, DialogData>();

            DRDialog[] dRDialogs = dtDialog.GetAllDataRows();
            foreach (var dRDialog in dRDialogs)
            {
                DialogData dialogData = new DialogData(dRDialog);
                dicDialogData.Add(dialogData.Id, dialogData);
            }
        }

        public DialogData GetDialogDataById(int DialogId)
        {
            if (dicDialogData.ContainsKey(DialogId))
            {
                return dicDialogData[DialogId];
            }

            return null;
        }

        public DialogData[] GetAllDialogData()
        {
            int index = 0;
            DialogData[] results = new DialogData[dicDialogData.Count];
            foreach (var dialogData in dicDialogData.Values)
            {
                results[index++] = dialogData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRDialog>();

            dtDialog = null;
            dicDialogData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}
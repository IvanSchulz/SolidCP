// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
//using Microsoft.Deployment.WindowsInstaller;
using WixToolset.Dtf.WindowsInstaller;

namespace FuseCP.WIXInstaller.Common.Util
{
    internal interface IListCtrl
    {        
        ulong Count { get; }
        string Id { get; }
        void AddItem(Record Item);
    }

    internal abstract class ListCtrlBase : IListCtrl
    {
        private Session m_Ctx;
        private string m_CtrlType;
        private string m_CtrlId;        
        private View m_View;
        private ulong m_Count;

        public ListCtrlBase(Session session, string CtrlType, string CtrlId)
        {
            m_Ctx = session;
            m_CtrlType = CtrlType;
            m_CtrlId = CtrlId;
            m_View = null;
            m_Count = 0;
            Initialize();
        }

        ~ListCtrlBase()
        {
            if (m_View != null)
                m_View.Close();
        }

        public virtual ulong Count { get { return m_Count; } }

        public virtual string Id { get { return m_CtrlId; } }

        public virtual void AddItem(Record Item)
        {
            m_View.Execute(Item);
            ++m_Count;
        }

        private void Initialize()
        {
            m_Ctx.Database.Execute(string.Format("DELETE FROM `{0}` WHERE `Property`='{1}'", m_CtrlType, m_CtrlId)); 
            m_View = m_Ctx.Database.OpenView(m_Ctx.Database.Tables[m_CtrlType].SqlInsertString + " TEMPORARY");
        }        
    }

    class ListViewCtrl : ListCtrlBase
    {
        public ListViewCtrl(Session session, string WiXListID) : base(session, "ListView", WiXListID)
        {
            
        } 
       
        public void AddItem(bool Checked, string Value)
        {
            AddItem(new Record(new object[] { Id, Count, Value, Value, Checked ? "passmark" : "failmark" }));
        }
    }

    class ComboBoxCtrl : ListCtrlBase
    {
        public ComboBoxCtrl(Session session, string WiXComboID): base(session, "ComboBox", WiXComboID)
        {

        }

        public void AddItem(string Value)
        {
            AddItem(new Record(new object[] { Id, Count, Value, Value }));
        }
    }
}

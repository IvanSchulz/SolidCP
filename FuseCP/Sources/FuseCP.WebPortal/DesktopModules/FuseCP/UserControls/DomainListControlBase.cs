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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;


namespace FuseCP.Portal.UserControls
{
    public abstract class DomainListControlBase : FuseCPControlBase
    {
        # region Properties

        protected abstract Button AddButton { get; }
        protected abstract GridView Grid { get; }

        public Boolean DisplayNames
        {
            get { return Grid.Columns[ 0 ].Visible; }
            set { Grid.Columns[ 0 ].Visible = value; }
        }


        public String Value
        {
            get
            {
            var items = CollectFormData( false );
            return String.Join( ";", items.ToArray() );
            }

            set
            {
            var items = new List<String>();
            if ( !String.IsNullOrEmpty( value ) )
                {
                var parts = value.Split( ';' );
                items.AddRange( from part in parts where part.Trim() != "" select part.Trim() );
                }

            // save items
            _loaded_items = items.ToArray();

            if ( IsPostBack )
                {
                BindItems( _loaded_items );
                }
            }
        }

        # endregion


        protected void Page_Load( Object sender, EventArgs e )
        {
            if ( !IsPostBack )
                {
                BindItems( _loaded_items ); // empty list
                }
        }


        private void BindItems( IEnumerable items )
        {
            Grid.DataSource = items;
            Grid.DataBind();
        }


        public List<String> CollectFormData( Boolean include_empty )
        {
            var items = new List<String>();
            foreach ( GridViewRow row in Grid.Rows )
                {
                var txt_name = (TextBox)row.FindControl( _txt_control_name );
                var val = txt_name.Text.Trim();

                if ( include_empty || "" != val )
                    {
                    items.Add( val );
                    }
                }

            return items;
        }


        # region Events

        protected void BtnAddClick( Object sender, EventArgs e)
        {
            var items = CollectFormData( true );

            // add empty string
            items.Add( "" );

            // bind items
            BindItems( items.ToArray() );
        }


        protected void ListRowCommand( Object sender, GridViewCommandEventArgs e )
        {
            if ( "delete_item" != e.CommandName )
                {
                return;
                }

            var items = CollectFormData(true);

            // remove error
            items.RemoveAt( Utils.ParseInt( e.CommandArgument, 0 ) );

            // bind items
            BindItems(items.ToArray());
        }


        protected void ListRowDataBound( Object sender, GridViewRowEventArgs e )
        {
            var lbl_name = (Label)e.Row.FindControl( _lbl_control_name );
            var txt_name = (TextBox)e.Row.FindControl( _txt_control_name );
            var cmd_delete = (CPCC.StyleButton)e.Row.FindControl( _delete_control_name );

            if ( null == lbl_name )
                {
                return;
                }

            var val = (String)e.Row.DataItem;
            txt_name.Text = val;

            var pos = ( e.Row.RowIndex < 2 ) 
                ? e.Row.RowIndex.ToString( CultureInfo.InvariantCulture ) 
                : "";
            lbl_name.Text = GetLocalizedString( "Item" + pos + ".Text" );

            cmd_delete.CommandArgument = e.Row.RowIndex.ToString( CultureInfo.InvariantCulture );
        }

        # endregion


        # region Fields

        protected String[] _loaded_items = new String[] {};
        
        protected String _txt_control_name;
        protected String _lbl_control_name;
        protected String _delete_control_name;

        # endregion
    }
}

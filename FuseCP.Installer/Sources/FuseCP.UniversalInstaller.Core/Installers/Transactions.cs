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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace FuseCP.UniversalInstaller
{

	public class Transaction
	{
		protected Installer Installer { get; private set; }
		protected ExceptionDispatchInfo Error { get => Installer.Error; set => Installer.Error = value; }
		protected IEnumerable<Action> Undos { get => Installer.Undos; set => Installer.Undos = value; }
		protected bool HasError => Installer.HasError;
		public Transaction(Installer installer) => Installer = installer;

		public void WithRollback(params IEnumerable<Action> actions)
		{
			Undos = actions = actions.Concat(Undos);
			if (HasError)
			{
				Installer.OnError?.Invoke(Error.SourceException);

				foreach (var action in actions)
				{
					try
					{
						action?.Invoke();
					}
					catch { }
				}
				Undos = Enumerable.Empty<Action>();
				Error.Throw();
			}
		}
	}
	public partial class Installer
	{
		public CancellationTokenSource Cancel { get; set; } = new CancellationTokenSource();
		public ExceptionDispatchInfo Error { get; set; } = null;
		public bool HasError => Error != null || Cancel.IsCancellationRequested;
		public IEnumerable<Action> Undos { get; set; } = Enumerable.Empty<Action>();
		public Transaction Transaction(params IEnumerable<Action> actions)
		{
			foreach (var action in actions)
			{
				try
				{
					if (!Cancel.Token.IsCancellationRequested)
					{
						action?.Invoke();
					}

					Cancel.Token.ThrowIfCancellationRequested();
				}
				catch (Exception ex)
				{
					Log.WriteError(ex.Message, ex);
					Error = ExceptionDispatchInfo.Capture(ex);
					break;
				}
			}

			return new Transaction(this);
		}

	}
}
